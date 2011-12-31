using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using MonoTouch.Dialog;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MWC.iOS.Screens.Common;
using MWC.SAL;

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
		static UIImage _calendarImage = UIImage.FromFile ("Images/caltemplate.png");
		RSSParser<RSSEntry> _newsParser;
		NewsDetailsScreen _newsDetailsScreen;
		Dictionary<string, RSSEntry> _newsItems = new Dictionary<string, RSSEntry>();

		bool didViewDidLoadJustRun = true;
		
 		public NewsScreen () : base (UITableViewStyle.Grouped, new RootElement ("Loading placeholder"))
 		{}
		
		public override void ViewDidLoad ()
        {
			var rssUrl = AppDelegate.NewsUrl;
			if (rssUrl.Substring(0,4).ToLower() != "http")
				rssUrl = "http://" + AppDelegate.NewsUrl;
			_newsParser = new RSSParser<RSSEntry>(rssUrl);
						
			base.ViewDidLoad ();

			didViewDidLoadJustRun = true;
		}

		protected override void LoadData ()
		{
			var hasConnection = Reachability.IsHostReachable(AppDelegate.NewsBaseUrl);
			if (hasConnection)
			{
				var timeSinceLastRefresh = (DateTime.UtcNow - _newsParser.GetLastRefreshTimeUtc());
				if (timeSinceLastRefresh.TotalMinutes < 10) {
					Debug.WriteLine ("Refreshed rss {0} minutes ago, not bothering", timeSinceLastRefresh.TotalMinutes);
					GenerateBlogSectionUI (_newsParser.AllItems);
				}
				else
				{
					if(_newsParser.HasLocalData)
					{
						GenerateBlogSectionUI (_newsParser.AllItems);
					}
					MonoTouch.UIKit.UIApplication.SharedApplication.NetworkActivityIndicatorVisible = true;
					_newsParser.Refresh(delegate {
						using (var pool = new NSAutoreleasePool())
						{
							MonoTouch.UIKit.UIApplication.SharedApplication.NetworkActivityIndicatorVisible = false;
							GenerateBlogSectionUI (_newsParser.AllItems);
						}
					});
					
				}
			}
			else if (!hasConnection && _newsParser.HasLocalData)
			{
				Debug.WriteLine ("No net - Has Local data so show it.");
				GenerateBlogSectionUI (_newsParser.AllItems);
			}
			else
			{
				this.InvokeOnMainThread(delegate {
					StopLoadingScreen();
					using (var alert = new UIAlertView("Network unavailable"
						,"Could not connect to " + AppDelegate.NewsBaseUrl
						,null,"OK",null))
					{
						alert.Show();
					}
				});
			}
		}
		
		private void GenerateBlogSectionUI (List<RSSEntry> posts)
		{
			this.InvokeOnMainThread(delegate {
				var blogSection = new Section ();
				// creates the rows using MT.Dialog
				_newsItems.Clear();
				foreach (var post in posts){
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
			});
		}
		
//		void StartLoadingScreen (string message)
//		{
//			using (var pool = new NSAutoreleasePool ()) {
//				this.InvokeOnMainThread(delegate {
//					loadingView = new UILoadingView (message);
//					this.View.BringSubviewToFront (loadingView);
//					this.View.AddSubview (loadingView);
//					this.View.UserInteractionEnabled = false;
//				});
//			}
//		}
//		
//		/// <summary>
//		/// If a loading screen exists, it will fade it out.
//		/// </summary>
//		void StopLoadingScreen ()
//		{
//			using (var pool = new NSAutoreleasePool ()) {
//				this.InvokeOnMainThread(delegate {
//					if (loadingView != null)
//					{
//						Debug.WriteLine ("Fade out loading...");
//						loadingView.OnFinishedFadeOutAndRemove += delegate {
//							if (loadingView != null)
//							{
//								Debug.WriteLine ("Disposing of object..");
//								loadingView.Dispose();
//								loadingView = null;
//							}
//						};
//						loadingView.FadeOutAndRemove ();
//						this.View.UserInteractionEnabled = true;
//					}
//				});
//			}
//		}
		
		public override void ViewWillAppear (bool animated)
		{
			if (didViewDidLoadJustRun == true) {
				  didViewDidLoadJustRun = false;
				  return;
			}
			if (Root != null && Root.Count == 0 )
			{
				ThreadPool.QueueUserWorkItem (delegate {
					LoadData();
				});
			}
		}
    }
}