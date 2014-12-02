using System;
using System.Collections.Generic;
using System.Drawing;
using MonoTouch.Dialog;
using Foundation;
using UIKit;
using MWC.iOS.Screens.Common;
using MWC.iOS.Screens.iPad.Twitter;
using CoreGraphics;

namespace MWC.iOS.Screens.iPhone.Twitter
{
	/// <summary>
	/// List of tweets, this MT.D-based list is used on both iPhone and iPad
	/// </summary>
	public partial class TwitterScreen : LoadingDialogViewController
	{
		public IList<BL.Tweet> TwitterFeed;
		TwitterSplitView splitView;

		public TwitterScreen () : base (UITableViewStyle.Plain, new RootElement ("Loading..."))
		{
			RefreshRequested += HandleRefreshRequested;
		}

		public TwitterScreen (TwitterSplitView twitterSplitView) : this ()
		{
			splitView = twitterSplitView;
		}

		public override Source CreateSizingSource (bool unevenRows)
		{
			return new TwitterScreenSizingSource (this);
		}

		/// <summary>
		/// Implement Dialog's pull-to-refresh method
		/// </summary>
		void HandleRefreshRequested (object sender, EventArgs e)
		{
			BL.Managers.TwitterFeedManager.Update ();
		}

		void HandleUpdateStarted (object sender, EventArgs ea)
		{
			InvokeOnMainThread (delegate {
				UIKit.UIApplication.SharedApplication.NetworkActivityIndicatorVisible = true;
			});
		}

		void HandleUpdateFinished (object sender, EventArgs ea)
		{	
			// assume we can 'Get()' them, since update has finished
			TwitterFeed = BL.Managers.TwitterFeedManager.GetTweets ();
			InvokeOnMainThread (delegate {
				UIKit.UIApplication.SharedApplication.NetworkActivityIndicatorVisible = false;
				PopulateData ();
			});
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			BL.Managers.TwitterFeedManager.UpdateStarted += HandleUpdateStarted;
			BL.Managers.TwitterFeedManager.UpdateFinished += HandleUpdateFinished;
		}

		[Obsolete]
		public override void ViewDidUnload ()
		{
			base.ViewDidUnload ();
			RefreshRequested -= HandleRefreshRequested;
			BL.Managers.TwitterFeedManager.UpdateStarted -= HandleUpdateStarted;
			BL.Managers.TwitterFeedManager.UpdateFinished -= HandleUpdateFinished;
		}
		// hack to keep the selection, for some reason DidLayoutSubviews is getting called twice and i don't know wh
		NSIndexPath tempIndexPath;

		public override void ViewDidLayoutSubviews ()
		{
			base.ViewDidLayoutSubviews ();
			if (TableView.IndexPathForSelectedRow != null)
				tempIndexPath = TableView.IndexPathForSelectedRow;
			else if (tempIndexPath != null) {
				TableView.SelectRow (tempIndexPath, false, UITableViewScrollPosition.None);
				tempIndexPath = null;
			}
		}

		/// <summary>
		/// Called by the base class when loading the page. Gets Tweets from 'cache'
		// and calls PopulateData
		/// and if there are none, calls Update().
		/// </summary>
		protected override void LoadData ()
		{
			// get the tweets 
			TwitterFeed = BL.Managers.TwitterFeedManager.GetTweets ();
			if (TwitterFeed.Count == 0) {
				BL.Managers.TwitterFeedManager.Update ();	
			} else {
				PopulateData ();
			}
		}

		/// <summary>
		/// This could get called from main thread or background thread.
		/// Remember to InvokeOnMainThread if required
		/// </summary>
		void PopulateData ()
		{
			if (TwitterFeed.Count == 0) {
				var section = new Section ("Network unavailable") {
					new StyledStringElement ("Twitter not available. Try again later.")
				};
				Root = new RootElement ("Twitter") { section };
			} else {
				Section section;
				UI.CustomElements.TweetElement twitterElement;
				
				// create a root element and a new section (MT.D requires at least one)
				Root = new RootElement ("Twitter");
				section = new Section ();
	
				// for each tweet, add a custom TweetElement to the MT.D elements collection
				foreach (var tw in TwitterFeed) {
					var currentTweet = tw; 
					twitterElement = new UI.CustomElements.TweetElement (currentTweet, splitView);
					section.Add (twitterElement);
				}
				
				Root.Clear ();
				// add the section to the root
				Root.Add (section);
			}
			base.StopLoadingScreen ();	// hide the 'loading' animation (from base)
			ReloadComplete ();
		}
	}

	/// <summary>
	/// Implement variable row height here, since when it is implemented on the TweetCell
	/// itself the variable heights are not returned after a pull-to-refresh.
	/// </summary>
	public class TwitterScreenSizingSource : DialogViewController.SizingSource
	{
		TwitterScreen twitterScreen;

		public TwitterScreenSizingSource (DialogViewController dvc) : base (dvc)
		{
			twitterScreen = (TwitterScreen)dvc;
		}

		public override nfloat GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
		{
			if (twitterScreen.TwitterFeed.Count > indexPath.Row) {
				var t = twitterScreen.TwitterFeed [indexPath.Row];
				CGSize size = tableView.StringSize (t.Title
								, UIFont.FromName ("Helvetica-Light", AppDelegate.Font10_5pt)
								, new SizeF (239, 140), UILineBreakMode.WordWrap);
				return 14 + 21 + 22 + size.Height + 8;
			} else
				return 40f;
		}
	}
}