using System;
using System.Diagnostics;
using System.Drawing;
using MonoTouch.Dialog;
using Foundation;
using UIKit;
using CoreGraphics;

namespace MWC.iOS.Screens.Common {
	/// <summary>
	/// Share a 'loading' screen between DialogViewControllers that
	/// are populated from network requests (Twitter and News)
	/// </summary>
	/// <remarks>
	/// This ViewController implements the data loading via a virtual
	/// method LoadData(), which must call StopLoadingScreen()
	/// </remarks>
	public class LoadingDialogViewController : DialogViewController {
		MWC.iOS.Screens.Common.UILoadingView loadingView;
		
		/// <summary>
		/// Set pushing=true so that the UINavCtrl 'back' button is enabled
		/// </summary>
		public LoadingDialogViewController (UITableViewStyle style, RootElement root) : base(style, root, true)
		{
		}
		
		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
			if (Root == null || Root.Count == 0) {
				StartLoadingScreen("Loading...");
			
				NSTimer.CreateScheduledTimer (TimeSpan.FromMilliseconds (1), delegate {
					LoadData();
				});
			} else ConsoleD.WriteLine ("Dialog data already populated");
		}
		
		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			return AppDelegate.IsPad;
		}

		/// <summary>
		/// Implement this in the subclass to actually load the data.
		/// You MUST call StopLoadingScreen() at the end of your implementation!
		/// </summary>
		protected virtual void LoadData() 
		{
		}
		
		/// <summary>
		/// Called automatically in ViewDidLoad()
		/// </summary>
		protected void StartLoadingScreen (string message)
		{
			using (var pool = new NSAutoreleasePool ()) {
				this.InvokeOnMainThread(delegate {
					
					var bounds = new CGRect(0,0,768,1004);
					if (InterfaceOrientation == UIInterfaceOrientation.LandscapeLeft
					|| InterfaceOrientation == UIInterfaceOrientation.LandscapeRight) {
						bounds = new CGRect(0,0,1024,748);	
					} 

					if (AppDelegate.IsPhone)
						bounds = new CGRect(0,0,320,460);

					loadingView = new UILoadingView (message, bounds);
					// because DialogViewController is a UITableViewController,
					// we need to step OVER the UITableView, otherwise the loadingOverlay
					// sits *in* the scrolling area of the table
					View.Superview.Add (loadingView);
					View.Superview.BringSubviewToFront (loadingView);
					View.UserInteractionEnabled = false;
				});
			}
		}
		
		/// <summary>
		/// If a loading screen exists, it will fade it out.
		/// Your subclass MUST call this method once data has loaded (or a loading error occurred)
		/// to make the loading screen disappear and return control to the user
		/// </summary>
		protected void StopLoadingScreen ()
		{
			using (var pool = new NSAutoreleasePool ()) {
				InvokeOnMainThread(delegate {
					if (loadingView != null) {
						Debug.WriteLine ("Fade out loading...");
						loadingView.OnFinishedFadeOutAndRemove += delegate {
							if (loadingView != null) {
								Debug.WriteLine ("Disposing of loadingView object..");
								loadingView.Dispose();
								loadingView = null;
							}
						};
						loadingView.FadeOutAndRemove ();
						View.UserInteractionEnabled = true;
					}
				});
			}
		}
	}
}