using System;
using System.Collections.Generic;
using System.Drawing;
using MonoTouch.Dialog;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MWC.iOS.Screens.Common;
using MWC.SAL;

namespace MWC.iOS.Screens.iPhone.Twitter
{
	public partial class TwitterScreen : LoadingDialogViewController
	{
		TwitterParser<Tweet> _twitterParser;
		public List<Tweet> TwitterFeed;
		
		/// <remarks>
		/// When Style=Plain, white row artefacts are visible when the 'loading...' placeholder
		/// is displayed. These artefacts do not appear when Style=Grouped. TODO: fix!!
		/// </remarks>
		public TwitterScreen () : base (UITableViewStyle.Plain, new RootElement ("Loading placeholder"))
		{
			RefreshRequested += HandleRefreshRequested;
		}
		public override Source CreateSizingSource (bool unevenRows)
		{
			return new TwitterScreenSizingSource(this);
		}
		/// <summary>
		/// Implement MonoTouch.Dialog's pull-to-refresh method
		/// </summary>
		void HandleRefreshRequested (object sender, EventArgs e)
		{
			var twitterScreen = (TwitterScreen)sender;
			Section section;
			UI.CustomElements.TweetElement twitterElement;
			
			_twitterParser.Refresh(delegate {
				using (var pool = new NSAutoreleasePool())
				{
					MonoTouch.UIKit.UIApplication.SharedApplication.NetworkActivityIndicatorVisible = false;
					var tweets = _twitterParser.AllItems;	
					// create a root element and a new section (MT.D requires at least one)
					this.Root = new RootElement ("Twitter");
					section = new Section();
		
					// for each exhibitor, add a custom ExhibitorElement to the elements collection
					foreach ( var tw in tweets )
					{
						var currentTweet = tw; //cloj
						twitterElement = new UI.CustomElements.TweetElement (currentTweet);
						section.Add(twitterElement);
					}
					
					Root.Clear ();
					// add the section to the root
					Root.Add(section);

					twitterScreen.ReloadComplete ();
				}
			});

		}
		
		/// <summary>
		/// Populates the page with tweets
		/// </summary>
		protected override void LoadData()
		{
			// declare vars
			Section section;
			UI.CustomElements.TweetElement twitterElement;

			// get the tweets
			TwitterFeed = new List<Tweet>();
			_twitterParser = new TwitterParser<Tweet>(Constants.TwitterUrl);

			_twitterParser.Refresh(delegate {
				using (var pool = new NSAutoreleasePool())
				{
					MonoTouch.UIKit.UIApplication.SharedApplication.NetworkActivityIndicatorVisible = false;
					TwitterFeed = _twitterParser.AllItems;	
					// create a root element and a new section (MT.D requires at least one)
					this.Root = new RootElement ("Twitter");
					section = new Section();
		
					// for each exhibitor, add a custom ExhibitorElement to the elements collection
					foreach ( var tw in TwitterFeed )
					{
						twitterElement = new UI.CustomElements.TweetElement (tw);
						section.Add(twitterElement);
					}
					
					// add the section to the root
					Root.Add(section);
					base.StopLoadingScreen();	// hide the 'loading' animation (from base)
				}
			});
		}
	}

	/// <summary>
	/// Implement variable row height here, since when it is implemented on the TweetCell
	/// itself the variable heights are not returned after a pull-to-refresh.
	/// </summary>
	public class TwitterScreenSizingSource : DialogViewController.SizingSource
	{
		TwitterScreen _ts;
		public TwitterScreenSizingSource (DialogViewController dvc) : base(dvc)
		{
			_ts = (TwitterScreen)dvc;
		}
		public override float GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
		{
			if (_ts.TwitterFeed.Count > indexPath.Row)
			{
				var t = _ts.TwitterFeed[indexPath.Row];
				SizeF size = tableView.StringSize (t.Title
								, UIFont.SystemFontOfSize (14)
								, new SizeF (263, 65), UILineBreakMode.WordWrap);
				return size.Height + 15 + 3;	// 15 is the height of the 'name/date' UILabels, 3 is the bottom padding
			} else return 40f;
		}
	}
}