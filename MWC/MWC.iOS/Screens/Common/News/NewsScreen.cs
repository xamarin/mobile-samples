using System;
using System.Collections.Generic;
using System.Diagnostics;
using MonoTouch.Dialog;
using MonoTouch.UIKit;
using MWC.BL;
using MWC.iOS.Screens.Common;
using MonoTouch.Foundation;
using MWC.iOS.UI.CustomElements;
using System.Drawing;
using MWC.iOS.Screens.iPad.News;

namespace MWC.iOS.Screens.Common.News
{
	/// <summary>
	/// News sourced from a google search
	/// </summary>
	public class NewsScreen : LoadingDialogViewController
	{
		static UIImage _calendarImage = UIImage.FromFile (AppDelegate.ImageCalendarTemplate);
		Dictionary<string, RSSEntry> _newsItems = new Dictionary<string, RSSEntry>();

		public IList<RSSEntry> NewsFeed;

 		public NewsScreen () : base (UITableViewStyle.Plain, new RootElement ("Loading..."))
 		{
			RefreshRequested += HandleRefreshRequested;
		}
		NewsSplitView _splitView;
		public NewsScreen (NewsSplitView splitView) : this()
		{
			_splitView = splitView;
		}
		
		/// <summary>
		/// Implement MonoTouch.Dialog's pull-to-refresh method
		/// </summary>
		void HandleRefreshRequested (object sender, EventArgs e)
		{
			BL.Managers.NewsManager.Update ();
		}
		void HandleUpdateStarted(object sender, EventArgs ea)
		{
			MonoTouch.UIKit.UIApplication.SharedApplication.NetworkActivityIndicatorVisible = true;
		}
		void HandleUpdateFinished(object sender, EventArgs ea)
		{	
			MonoTouch.UIKit.UIApplication.SharedApplication.NetworkActivityIndicatorVisible = false;
			// assume we can 'Get()' them, since update has finished
			NewsFeed = BL.Managers.NewsManager.Get ();
			this.InvokeOnMainThread(delegate {
				PopulateData ();
			});
		}
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			BL.Managers.NewsManager.UpdateStarted += HandleUpdateStarted;
			BL.Managers.NewsManager.UpdateFinished += HandleUpdateFinished;
		}
		public override void ViewDidUnload ()
		{
			base.ViewDidUnload ();
			RefreshRequested -= HandleRefreshRequested;
			BL.Managers.NewsManager.UpdateFinished -= HandleUpdateStarted;
			BL.Managers.NewsManager.UpdateStarted -= HandleUpdateFinished;
		}

		protected override void LoadData ()
		{
			// get the news 
			NewsFeed = BL.Managers.NewsManager.Get ();
			if (NewsFeed.Count == 0)
			{
				BL.Managers.NewsManager.Update ();	
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
		void PopulateData ()
		{
			if (NewsFeed.Count == 0)
			{
				var section = new Section("Network unavailable")
				{
					new StyledStringElement("News not available. Try again later.") 
				};
				Root = new RootElement ("News") { section };
			}
			else
			{
				var blogSection = new Section ();
				// creates the rows using MT.Dialog
				_newsItems.Clear();
				foreach (var post in NewsFeed)
				{
					var published = post.Published;
					var image = BadgeElement.MakeCalendarBadge (_calendarImage, published.ToString ("MMMM"), published.ToString ("dd"));
					var badgeRow = new NewsElement (post, image, _splitView);
	
					_newsItems.Add(post.Title, post); // collate posts so we can 'zoom in' to them

					blogSection.Add (badgeRow);
				}
				Root = new RootElement ("News") { blogSection };
			}
			StopLoadingScreen();
		}
		public override Source CreateSizingSource (bool unevenRows)
		{
			return new NewsScreenSizingSource(this);
		}
    }
	public class NewsScreenSizingSource : DialogViewController.SizingSource
	{
		NewsScreen _ns;
		public NewsScreenSizingSource (DialogViewController dvc) : base(dvc)
		{
			_ns = (NewsScreen)dvc;
		}
		public override float GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
		{
			if (_ns.NewsFeed.Count > indexPath.Row)
			{
				var t = _ns.NewsFeed[indexPath.Row];
				SizeF size = tableView.StringSize (t.Title
								, UIFont.FromName("Helvetica-Light",AppDelegate.Font16pt)
								, new SizeF (230, 400), UILineBreakMode.WordWrap);
				return size.Height + 20;
			}
			else return 40f;
		}
	}
}