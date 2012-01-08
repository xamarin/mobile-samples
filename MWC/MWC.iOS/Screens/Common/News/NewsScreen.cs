using System;
using System.Collections.Generic;
using System.Diagnostics;
using MonoTouch.Dialog;
using MonoTouch.UIKit;
using MWC.BL;
using MWC.iOS.Screens.Common;

namespace MWC.iOS.Screens.Common.News
{
	/// <summary>
	/// News sourced from a google search
	/// </summary>
	/// <remarks>
	/// http://softwareandservice.wordpress.com/2009/09/21/building-a-rss-reader-iphone-app-using-monotouch/
	/// </remarks>
	public class NewsScreen : LoadingDialogViewController
	{
		static UIImage _calendarImage = UIImage.FromFile (AppDelegate.ImageCalendarTemplate);
		NewsDetailsScreen _newsDetailsScreen;
		Dictionary<string, RSSEntry> _newsItems = new Dictionary<string, RSSEntry>();

		IList<RSSEntry> NewsFeed;

 		public NewsScreen () : base (UITableViewStyle.Grouped, new RootElement ("Loading..."))
 		{}
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
//			var hasConnection = Reachability.IsHostReachable(Constants.NewsBaseUrl);
//			if (hasConnection)
//			{
//				var timeSinceLastRefresh = (DateTime.UtcNow - _newsParser.GetLastRefreshTimeUtc());
//				if (timeSinceLastRefresh.TotalMinutes < 10) {
//					Debug.WriteLine ("Refreshed rss {0} minutes ago, not bothering", timeSinceLastRefresh.TotalMinutes);
//					GenerateBlogSectionUI (_newsParser.AllItems);
//				}
//				else
//				{
//					if(_newsParser.HasLocalData)
//					{
//						GenerateBlogSectionUI (_newsParser.AllItems);
//					}
//					MonoTouch.UIKit.UIApplication.SharedApplication.NetworkActivityIndicatorVisible = true;
//					_newsParser.Refresh(delegate {
//						using (var pool = new NSAutoreleasePool())
//						{
//							MonoTouch.UIKit.UIApplication.SharedApplication.NetworkActivityIndicatorVisible = false;
//							GenerateBlogSectionUI (_newsParser.AllItems);
//						}
//					});
//					
//				}
//			}
//			else if (!hasConnection && _newsParser.HasLocalData)
//			{
//				Debug.WriteLine ("No net - Has Local data so show it.");
//				GenerateBlogSectionUI (_newsParser.AllItems);
//			}
//			else
//			{
//				this.InvokeOnMainThread(delegate {
//					StopLoadingScreen();
//					using (var alert = new UIAlertView("Network unavailable"
//						,"Could not connect to " + Constants.NewsBaseUrl
//						,null,"OK",null))
//					{
//						alert.Show();
//					}
//				});
//			}
		}
		/// <summary>
		/// This could get called from main thread or background thread.
		/// Remember to InvokeOnMainThread if required
		/// </summary>
		void PopulateData ()
		{
			var blogSection = new Section ();
			// creates the rows using MT.Dialog
			_newsItems.Clear();
			foreach (var post in NewsFeed){
				var published = post.Published;
				var image = BadgeElement.MakeCalendarBadge (_calendarImage, published.ToString ("MMMM"), published.ToString ("dd"));
				var badgeRow = new BadgeElement (image, post.Title);
				badgeRow.Lines = 2;
				_newsItems.Add(post.Title, post); // collate posts so we can 'zoom in' to them
				badgeRow.Tapped += delegate
				{	// Show the actual post for this headline: assumes no duplicate headlines!
					Debug.WriteLine("tapped" + badgeRow.Caption + " " + post.Title);
					RSSEntry p = _newsItems[badgeRow.Caption]; 
					if (_newsDetailsScreen == null)
						_newsDetailsScreen = new NewsDetailsScreen(p.Title, p.Content);
					else
						_newsDetailsScreen.Update(p.Title, p.Content);
					_newsDetailsScreen.Title = p.Title;
					this.NavigationController.PushViewController(_newsDetailsScreen, true);
				};
				blogSection.Add (badgeRow);
			}
			Root = new RootElement ("News") { blogSection };
			StopLoadingScreen();
		}
    }
}