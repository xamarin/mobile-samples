using System;
using System.Collections.Generic;
using System.Drawing;
using MonoTouch.Dialog;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MWC.iOS.Screens.Common;

namespace MWC.iOS.Screens.iPhone.Twitter
{
	public partial class TwitterScreen : LoadingDialogViewController
	{
		public IList<BL.Tweet> TwitterFeed;
		
		/// <remarks>
		/// When Style=Plain, white row artefacts are visible when the 'loading...' placeholder
		/// is displayed. These artefacts do not appear when Style=Grouped. TODO: fix!!
		/// </remarks>
		public TwitterScreen () : base (UITableViewStyle.Plain, new RootElement ("Loading..."))
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
			BL.Managers.TwitterFeedManager.Update ();
		}
		void HandleUpdateStarted(object sender, EventArgs ea)
		{
			MonoTouch.UIKit.UIApplication.SharedApplication.NetworkActivityIndicatorVisible = true;
		}
		void HandleUpdateFinished(object sender, EventArgs ea)
		{	
			MonoTouch.UIKit.UIApplication.SharedApplication.NetworkActivityIndicatorVisible = false;
			// assume we can 'Get()' them, since update has finished
			TwitterFeed = BL.Managers.TwitterFeedManager.GetTweets ();
			this.InvokeOnMainThread(delegate {
				PopulateData ();
			});
		}
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			BL.Managers.TwitterFeedManager.UpdateStarted += HandleUpdateStarted;
			BL.Managers.TwitterFeedManager.UpdateFinished += HandleUpdateFinished;
		}
		public override void ViewDidUnload ()
		{
			base.ViewDidUnload ();
			RefreshRequested -= HandleRefreshRequested;
			BL.Managers.TwitterFeedManager.UpdateFinished -= HandleUpdateStarted;
			BL.Managers.TwitterFeedManager.UpdateStarted -= HandleUpdateFinished;
		}

		/// <summary>
		/// Called by the base class when loading the page. Gets Tweets from 'cache'
		// and calls PopulateData
		/// and if there are none, calls Update().
		/// </summary>
		protected override void LoadData()
		{
			// get the tweets 
			TwitterFeed = BL.Managers.TwitterFeedManager.GetTweets ();
			if (TwitterFeed.Count == 0)
			{
				BL.Managers.TwitterFeedManager.Update ();	
			}			
			else
			{
				PopulateData ();
			}
		}
		/// <summary>
		/// This could get called from main thread or background thread.
		/// Remember to InvokeOnMainThread if required
		/// </summary>
		void PopulateData()
		{
			Section section;
			UI.CustomElements.TweetElement twitterElement;
			
			// create a root element and a new section (MT.D requires at least one)
			this.Root = new RootElement ("Twitter");
			section = new Section();

			// for each tweet, add a custom TweetElement to the MT.D elements collection
			foreach ( var tw in TwitterFeed )
			{
				var currentTweet = tw; //cloj
				twitterElement = new UI.CustomElements.TweetElement (currentTweet);
				section.Add(twitterElement);
			}
			
			Root.Clear ();
			// add the section to the root
			Root.Add(section);
			base.StopLoadingScreen();	// hide the 'loading' animation (from base)
			this.ReloadComplete ();
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