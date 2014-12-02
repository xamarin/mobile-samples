using System;
using System.Collections.Generic;
using System.Drawing;
using MonoTouch.Dialog;
using Foundation;
using UIKit;
using MWC.BL;
using MWC.iOS.Screens.Common;
using MWC.iOS.Screens.iPad.News;
using MWC.iOS.UI.CustomElements;
using CoreGraphics;

namespace MWC.iOS.Screens.Common.News
{
	/// <summary>
	/// News sourced from a google search, this MT.D-based list is used on both iPhone and iPad
	/// </summary>
	public class NewsScreen : LoadingDialogViewController
	{
		static UIImage calendarImage = UIImage.FromFile (AppDelegate.ImageCalendarPad);
		Dictionary<string, RSSEntry> newsItems = new Dictionary<string, RSSEntry> ();
		public IList<RSSEntry> NewsFeed;

		public NewsScreen () : base (UITableViewStyle.Plain, new RootElement ("Loading..."))
		{
			RefreshRequested += HandleRefreshRequested;
		}

		NewsSplitView splitView;

		public NewsScreen (NewsSplitView splitView) : this ()
		{
			this.splitView = splitView;
		}

		/// <summary>
		/// Implement Dialog's pull-to-refresh method
		/// </summary>
		void HandleRefreshRequested (object sender, EventArgs e)
		{
			BL.Managers.NewsManager.Update ();
		}

		void HandleUpdateStarted (object sender, EventArgs ea)
		{
			UIKit.UIApplication.SharedApplication.NetworkActivityIndicatorVisible = true;
		}

		void HandleUpdateFinished (object sender, EventArgs ea)
		{	
			// assume we can 'Get()' them, since update has finished
			NewsFeed = BL.Managers.NewsManager.Get ();
			InvokeOnMainThread (delegate {
				UIKit.UIApplication.SharedApplication.NetworkActivityIndicatorVisible = false;
				PopulateData ();
			});
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			BL.Managers.NewsManager.UpdateStarted += HandleUpdateStarted;
			BL.Managers.NewsManager.UpdateFinished += HandleUpdateFinished;
		}

		[Obsolete]
		public override void ViewDidUnload ()
		{
			base.ViewDidUnload ();
			RefreshRequested -= HandleRefreshRequested;
			BL.Managers.NewsManager.UpdateStarted -= HandleUpdateStarted;
			BL.Managers.NewsManager.UpdateFinished -= HandleUpdateFinished;
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

		protected override void LoadData ()
		{
			// get the news 
			NewsFeed = BL.Managers.NewsManager.Get ();
			if (NewsFeed.Count == 0) {
				BL.Managers.NewsManager.Update ();	
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
			if (NewsFeed.Count == 0) {
				var section = new Section ("Network unavailable") {
					new StyledStringElement ("News not available. Try again later.") 
				};
				Root = new RootElement ("News") { section };
			} else {
				var blogSection = new Section ();
				// creates the rows using MT.Dialog
				newsItems.Clear ();
				foreach (var post in NewsFeed) {
					var published = post.Published;
					var image = MWC.iOS.UI.CustomElements.CustomBadgeElement.MakeCalendarBadge (calendarImage
														, published.ToString ("MMM").ToUpper ()
														, published.ToString ("dd"));
					var badgeRow = new NewsElement (post, image, splitView);
	
					newsItems.Add (post.Title, post); // collate posts so we can 'zoom in' to them

					blogSection.Add (badgeRow);
				}
				Root = new RootElement ("News") { blogSection };
			}
			base.StopLoadingScreen ();
			this.ReloadComplete ();
		}

		public override Source CreateSizingSource (bool unevenRows)
		{
			return new NewsScreenSizingSource (this);
		}
	}

	public class NewsScreenSizingSource : DialogViewController.SizingSource
	{
		NewsScreen _ns;

		public NewsScreenSizingSource (DialogViewController dvc) : base (dvc)
		{
			_ns = (NewsScreen)dvc;
		}

		public override nfloat GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
		{
			if (_ns.NewsFeed.Count > indexPath.Row) {
				var t = _ns.NewsFeed [indexPath.Row];
				CGSize size = tableView.StringSize (t.Title
								, UIFont.FromName ("Helvetica-Light", AppDelegate.Font16pt)
								, new SizeF (230, 400), UILineBreakMode.WordWrap);
				return size.Height + 20;
			} else
				return 40f;
		}
	}
}