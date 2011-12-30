using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System;
using System.Collections.Generic;
using MonoTouch.Dialog;
using System.Threading;
using System.Diagnostics;

using MWC.SAL;

namespace MWC.iOS.Screens.Common.News
{
	/// <summary>
	/// First view that users see - lists the top level of the hierarchy xml
	/// </summary>
	/// <remarks>
	/// LOADS data from the xml files into public properties (deserialization)
	/// then we pass around references to the MainViewController so other
	/// ViewControllers can access the data.
	/// 
	/// http://softwareandservice.wordpress.com/2009/09/21/building-a-rss-reader-iphone-app-using-monotouch/
	/// </remarks>
	[Register]
	public class NewsScreen : DialogViewController
	{
		static UIImage template = UIImage.FromFile ("Images/caltemplate.png");
		RSSParser<RSSEntry> BlogRepo;
		NewsDetailsScreen blogVC;
		Dictionary<string, RSSEntry> blogposts = new Dictionary<string, RSSEntry>();
		MWC.iOS.Screens.Common.UILoadingView loadingView;
		bool didViewDidLoadJustRun = true;
		
 		public NewsScreen () : base (new RootElement ("Placeholder"))
 		{
 			Style = UITableViewStyle.Grouped;
 		}
		
		public override void ViewDidLoad ()
        	{
			base.ViewDidLoad ();
			var rssUrl = AppDelegate.NewsUrl;
			if (rssUrl.Substring(0,4).ToLower() != "http")
				rssUrl = "http://" + AppDelegate.NewsUrl;
			BlogRepo = new RSSParser<RSSEntry>(rssUrl);
						
			StartLoadingScreen("Loading...");
			//ThreadPool.QueueUserWorkItem (delegate {
			NSTimer.CreateScheduledTimer (TimeSpan.FromMilliseconds (1), delegate
			{
				LoadData();
			});
			//});
			didViewDidLoadJustRun = true;
		}

		private void LoadData ()
		{
			var hasConnection = Reachability.IsHostReachable("pipes.yahoo.com");//AppDelegate.ConferenceData.BaseUrl);
			if (hasConnection)
			{
				var timeSinceLastRefresh = (DateTime.UtcNow - BlogRepo.GetLastRefreshTimeUtc());
				if (timeSinceLastRefresh.TotalMinutes < 10) {
					Debug.WriteLine ("Refreshed rss {0} minutes ago, not bothering", timeSinceLastRefresh.TotalMinutes);
					GenerateBlogSectionUI (BlogRepo.AllItems);
				}
				else
				{
					if(BlogRepo.HasLocalData)
						GenerateBlogSectionUI (BlogRepo.AllItems);
					
					MonoTouch.UIKit.UIApplication.SharedApplication.NetworkActivityIndicatorVisible = true;
					BlogRepo.Refresh(delegate {
						using (var pool = new NSAutoreleasePool())
						{
							MonoTouch.UIKit.UIApplication.SharedApplication.NetworkActivityIndicatorVisible = false;
							GenerateBlogSectionUI (BlogRepo.AllItems);
						}
					});
					
				}
			}
			else if (!hasConnection && BlogRepo.HasLocalData)
			{
				Debug.WriteLine ("No net - Has Local data so show it.");
				GenerateBlogSectionUI (BlogRepo.AllItems);
			}
			else
			{
				this.InvokeOnMainThread(delegate {
					StopLoadingScreen();
					using (var alert = new UIAlertView("Network unavailable"
						,"Could not connect to " + AppDelegate.NewsUrl
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
				blogposts.Clear();
				foreach (var post in posts){
					var published = post.Published;
					var image = BadgeElement.MakeCalendarBadge (template, published.ToString ("MMMM"), published.ToString ("dd"));
					var badgeRow = new BadgeElement (image, post.Title);
					badgeRow.Lines = 2;
					blogposts.Add(post.Title, post); // collate posts so we can 'zoom in' to them
					badgeRow.Tapped += delegate
					{	// Show the actual post for this headline: assumes no duplicate headlines!
						Debug.WriteLine("tapped" + badgeRow.Caption + " " + post.Title);
						RSSEntry p = blogposts[badgeRow.Caption]; 
						if (blogVC == null)
							blogVC = new NewsDetailsScreen(p.Title, p.Content);
						else
							blogVC.Update(p.Title, p.Content);
						blogVC.Title = p.Title;
						this.NavigationController.PushViewController(blogVC, true);
					};
					blogSection.Add (badgeRow);
				}
				Root = new RootElement ("News") { blogSection };
				StopLoadingScreen();
			});
		}
		
		void StartLoadingScreen (string message)
		{
			using (var pool = new NSAutoreleasePool ()) {
				this.InvokeOnMainThread(delegate {
					loadingView = new UILoadingView (message);
					this.View.BringSubviewToFront (loadingView);
					this.View.AddSubview (loadingView);
					this.View.UserInteractionEnabled = false;
				});
			}
		}
		
		/// <summary>
		/// If a loading screen exists, it will fade it out.
		/// </summary>
		void StopLoadingScreen ()
		{
			using (var pool = new NSAutoreleasePool ()) {
				this.InvokeOnMainThread(delegate {
					if (loadingView != null)
					{
						Debug.WriteLine ("Fade out loading...");
						loadingView.OnFinishedFadeOutAndRemove += delegate {
							if (loadingView != null)
							{
								Debug.WriteLine ("Disposing of object..");
								loadingView.Dispose();
								loadingView = null;
							}
						};
						loadingView.FadeOutAndRemove ();
						this.View.UserInteractionEnabled = true;
					}
				});
			}
		}
		
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