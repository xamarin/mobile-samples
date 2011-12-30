using System;
using System.Collections.Generic;
using System.Drawing;
using MonoTouch.Dialog;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MWC.SAL;

namespace MWC.iOS.Screens.iPhone.Twitter
{
	public partial class TwitterScreen : DialogViewController
	{
		protected TweetDetailsScreen _twitterDetailsScreen;
		TwitterParser<Tweet> twitterParser;
		TwitterScreenSizingSource sizingSource;

		public List<Tweet> TwitterFeed;

		public TwitterScreen () : base (UITableViewStyle.Plain, null)
		{
			Console.WriteLine("not updating, populating twitter.");
			//sizingSource = new TwitterScreenSizingSource(this);
			this.PopulatePage();
			RefreshRequested += HandleRefreshRequested;
		}
		
		public override Source CreateSizingSource (bool unevenRows)
		{
			return new TwitterScreenSizingSource(this);//sizingSource;
		}
		void HandleRefreshRequested (object sender, EventArgs e)
		{
			var dvc = (TwitterScreen)sender;
			Section section;
			UI.CustomElements.TweetElement twitterElement;
			//TODO: implement pull-to-refresh!!
			twitterParser.Refresh(delegate {
				using (var pool = new NSAutoreleasePool())
				{
					MonoTouch.UIKit.UIApplication.SharedApplication.NetworkActivityIndicatorVisible = false;
					var tweets = twitterParser.AllItems;	
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

					dvc.ReloadComplete ();
				}
			});

		}
		
		/// <summary>
		/// Populates the page with tweets
		/// </summary>
		public void PopulatePage()
		{
			// declare vars
			Section section;
			UI.CustomElements.TweetElement twitterElement;

			// get the tweets
			TwitterFeed = new List<Tweet>();
			twitterParser = new TwitterParser<Tweet>(AppDelegate.TwitterUrl);

			twitterParser.Refresh(delegate {
				using (var pool = new NSAutoreleasePool())
				{
					MonoTouch.UIKit.UIApplication.SharedApplication.NetworkActivityIndicatorVisible = false;
					TwitterFeed = twitterParser.AllItems;	
					// create a root element and a new section (MT.D requires at least one)
					this.Root = new RootElement ("Twitter");
					section = new Section();
		
					// for each exhibitor, add a custom ExhibitorElement to the elements collection
					foreach ( var tw in TwitterFeed )
					{
						var currentTweet = tw; //cloj
						twitterElement = new UI.CustomElements.TweetElement (currentTweet);
						section.Add(twitterElement);
					}
					
					// add the section to the root
					Root.Add(section);
				}
			});


			// create a root element and a new section (MT.D requires at least one)
			this.Root = new RootElement ("Twitter");
			section = new Section();
		
			// add the section to the root
			Root.Add(section);
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