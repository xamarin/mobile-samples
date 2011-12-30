using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Xml.Serialization;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MWC.SAL;
using System.Threading;
using System.Diagnostics;

namespace MWC.iOS.Screens.Common.Twitter
{
	[Obsolete("See MT.D implementation in iPhone folder; although this may be re-instated later")]
 	public class TableViewControllerBase : UIViewController
    {
        public UITableView tableView;
        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();

//			Uncomment these lines if a background is added behind the table
//
//			UIImageView imageView = new UIImageView(UIImage.FromFile("Background.png"));
//			imageView.Frame = new RectangleF (0, 0, this.View.Frame.Width, this.View.Frame.Height);
//			imageView.UserInteractionEnabled = true;

			// no XIB !
			tableView = new UITableView()
			{
			    AutoresizingMask = UIViewAutoresizing.FlexibleHeight|
			                       UIViewAutoresizing.FlexibleWidth,
//			    BackgroundColor = UIColor.Clear,
			    BackgroundColor = UIColor.White,
				Frame = new RectangleF (0, 0, this.View.Frame.Width, this.View.Frame.Height)
			};
//			imageView.AddSubview(tableView);
//			this.View.AddSubview(imageView);
			this.View.Add (tableView);
        }
    }



    public class TwitterViewController : TableViewControllerBase //UIViewController
    {
		public List<Tweet> TwitterFeed;
		public bool checkForRefresh;
		public bool reloading;
		public bool pullRefresh = false;
		bool didViewDidLoadJustRun = true;
		RefreshTableHeaderView headerView;
		Dictionary<int, TwitterViewCellController> controllers;
		TwitterParser<Tweet> twitterParser;
		UILoadingView loadingView;
		TableViewSource tvs;
		
		public override void ViewDidLoad ()
        {
			base.ViewDidLoad ();
			
			this.Title = "@mobileworldlive";

			// Set up the Refresh Table Header View.
			headerView = new RefreshTableHeaderView ();
			headerView.BackgroundColor = new UIColor (226f, 231f, 237f, 1f);
			tableView.AddSubview (headerView);

			
			TwitterFeed = new List<Tweet>();
			twitterParser = new TwitterParser<Tweet>(AppDelegate.TwitterUrl);
			
			controllers = new Dictionary<int, TwitterViewCellController> ();
			
			tableView.Frame = new RectangleF (0, 0, tableView.Frame.Width, tableView.Frame.Height);// - 95);
			tvs = new TableViewSource(this, controllers);
			tableView.Source =  tvs;

			StartLoadingScreen("Loading...");
			NSTimer.CreateScheduledTimer (TimeSpan.FromMilliseconds (5), delegate
			{
				LoadData();
			});
			
			didViewDidLoadJustRun = true;
        }
		
		private void LoadData(bool refresh)
		{
			var hasConnection = Reachability.IsHostReachable("twitter.com");
			if (hasConnection)
			{
				MonoTouch.UIKit.UIApplication.SharedApplication.NetworkActivityIndicatorVisible = true;
				twitterParser.Refresh(delegate {
					using (var pool = new NSAutoreleasePool())
					{
						MonoTouch.UIKit.UIApplication.SharedApplication.NetworkActivityIndicatorVisible = false;
						GenerateTwitterUI (twitterParser.AllItems);
					}
				});
			}
			else
			{
				this.InvokeOnMainThread(delegate {
					StopLoadingScreen();
					using (var alert = new UIAlertView("Network unavailable"
						,"Could not connect to " + AppDelegate.TwitterUrl
						,null,"OK",null))
					{
						alert.Show();
					}
				});
			}	
		}
		
		private void LoadData ()
		{
			var hasConnection = Reachability.IsHostReachable("twitter.com");
			if (hasConnection)
			{
				if(twitterParser.HasLocalData)
				{
					GenerateTwitterUI (twitterParser.AllItems);
				}

				MonoTouch.UIKit.UIApplication.SharedApplication.NetworkActivityIndicatorVisible = true;
				twitterParser.Refresh(delegate {
					using (var pool = new NSAutoreleasePool())
					{
						MonoTouch.UIKit.UIApplication.SharedApplication.NetworkActivityIndicatorVisible = false;
						GenerateTwitterUI (twitterParser.AllItems);
					}
				});
			}
			else if (!hasConnection && twitterParser.HasLocalData)
			{
				Debug.WriteLine ("No net - Has Local data so show it.");
				GenerateTwitterUI (twitterParser.AllItems);
			}
			else
			{
				this.InvokeOnMainThread(delegate {
					StopLoadingScreen();
					using (var alert = new UIAlertView("Network unavailable"
						,"Could not connect to " + AppDelegate.TwitterUrl
						,null,"OK",null))
					{
						alert.Show();
					}
				});
			}
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
							Debug.WriteLine ("Disposing of object.");
							loadingView.Dispose();
							loadingView = null;
						};
						loadingView.FadeOutAndRemove ();
						this.View.UserInteractionEnabled = true;
					}
				});
			}
		}
		
		private void GenerateTwitterUI (List<Tweet> tweetList, bool isPullRefresh)
		{
			this.InvokeOnMainThread (delegate {
				if (pullRefresh) {
					Debug.WriteLine ("it's a pull refresh");
					reloading = false;
					pullRefresh = false;
					headerView.FlipImageAnimated (false);
					headerView.ToggleActivityView ();
					
					UIView.BeginAnimations ("DoneReloadingData");
					UIView.SetAnimationDuration (0.3);
					tableView.ContentInset = new UIEdgeInsets (0f, 0f, 0f, 0f);
					headerView.SetStatus (RefreshTableHeaderView.RefreshStatus.PullToReloadStatus);
					UIView.CommitAnimations ();
					headerView.SetCurrentDate ();
				}
				Debug.WriteLine(String.Format("tweet count = {0}", tweetList.Count));  
				TwitterFeed = tweetList;
				
				tableView.ReloadData();						
				MonoTouch.UIKit.UIApplication.SharedApplication.NetworkActivityIndicatorVisible = false;
				StopLoadingScreen();
			});
		}
		
		private void GenerateTwitterUI (List<Tweet> tweetList)
		{
			GenerateTwitterUI(tweetList, false);
		}

		public override void ViewWillAppear (bool animated)
		{
			if (didViewDidLoadJustRun == true) {didViewDidLoadJustRun = false; return;}
			if (TwitterFeed.Count == 0)
			{	// try again, there should be some data
				ThreadPool.QueueUserWorkItem (delegate {
					LoadData();
				});
			}
		}

        private class TableViewSource : UITableViewSource
        {
			private TwitterViewController tvc;
			private RefreshTableHeaderView rthv;
			UITableView table;
			private int tag = 0;
			private Dictionary<int, TwitterViewCellController> controllers;
			
            public TableViewSource (TwitterViewController controller, Dictionary<int, TwitterViewCellController> controllers)
            {
				tvc = controller;
				rthv = controller.headerView;
				table = controller.tableView;
				tvc.checkForRefresh = false;
				tvc.reloading = false;
				this.controllers = controllers;
			}

            public override int RowsInSection (UITableView tableview, int section)
            {
                return tvc.TwitterFeed.Count;
            }

            public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
            {
				Console.WriteLine ("GetCell " + tvc.TwitterFeed [indexPath.Row].Title);
				TwitterViewCellController controller;
				UITableViewCell cell = tableView.DequeueReusableCell ("twitterCell");
				
				if (cell == null) {
					controller = new TwitterViewCellController ();
					NSBundle.MainBundle.LoadNib("TwitterViewCellController", controller, null);
					cell = controller.Cell;
//					AppDelegate.GetCellSelectedColor(cell);
					cell.Tag = tag++;
					controllers [cell.Tag] = controller;
				} else {
					controller = controllers [cell.Tag];
				}
				
				controller.Tweet = tvc.TwitterFeed [indexPath.Row];
				
                return cell;
            }
			
			public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
			{
				tvc.NavigationController.PushViewController (new TwitterDetailViewController (tvc.TwitterFeed [indexPath.Row]), true);
				tableView.DeselectRow (indexPath, false);
			}
			
			public override float GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
			{
				// No time but should replace with something from
				// http://stackoverflow.com/questions/129502/how-do-i-wrap-text-in-a-uitableviewcell-without-a-custom-cell
				return 80.0f;
			}
			
			#region UIScrollViewDelegate

			//[Export("scrollViewDidScroll:")]
			public override void Scrolled (UIScrollView scrollView)
			{
				if (tvc.checkForRefresh) {
					if (rthv.isFlipped && (table.ContentOffset.Y > -65f) && (table.ContentOffset.Y < 0f) && !tvc.reloading) {
						rthv.FlipImageAnimated (true);
						rthv.SetStatus (RefreshTableHeaderView.RefreshStatus.PullToReloadStatus);
					} else if ((!rthv.isFlipped) && (table.ContentOffset.Y < -65f)) {
						rthv.FlipImageAnimated (true);
						rthv.SetStatus (RefreshTableHeaderView.RefreshStatus.ReleaseToReloadStatus);
					}
				}
			}
	
			//[Export("scrollViewWillBeginDragging:")]
			public override void DraggingStarted (UIScrollView scrollView)
			{
				tvc.checkForRefresh = true;
			}
	
			//[Export("scrollViewDidEndDragging:willDecelerate:")]
			public override void DraggingEnded (UIScrollView scrollView, bool willDecelerate)
			{
				if (table.ContentOffset.Y <= -65f) {
					
					tvc.reloading = true;
					tvc.pullRefresh = true;
					ThreadPool.QueueUserWorkItem (delegate {
						tvc.LoadData(true);
					});
					rthv.ToggleActivityView ();
					UIView.BeginAnimations ("ReloadingData");
					UIView.SetAnimationDuration (0.2);
					table.ContentInset = new UIEdgeInsets (60f, 0f, 0f, 0f);
					UIView.CommitAnimations ();
				}
				
				tvc.checkForRefresh = false;
			}
			#endregion
        }		
    }
}