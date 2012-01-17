using System;
using System.Diagnostics;
using MonoTouch.Dialog;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace MWC.iOS.Screens.Common
{
	/// <summary>
	/// Share a 'loading' screen between DialogViewControllers that
	/// are populated from network requests (Twitter and News)
	/// </summary>
	/// <remarks>
	/// This ViewController implements the data loading via a virtual
	/// method LoadData(), which must call StopLoadingScreen()
	/// </remarks>
	public class LoadingDialogViewController : DialogViewController
	{
		MWC.iOS.Screens.Common.UILoadingView loadingView;

		public LoadingDialogViewController (UITableViewStyle style, RootElement root) : base(style, root)
		{}
		
		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			StartLoadingScreen("Loading...");
			
			NSTimer.CreateScheduledTimer (TimeSpan.FromMilliseconds (1), delegate
			{
				LoadData();
			});
		}
		
		/// <summary>
		/// Implement this in the subclass to actually load the data.
		/// You MUST call StopLoadingScreen() at the end of your implementation!
		/// </summary>
		protected virtual void LoadData() {}
		
		/// <summary>
		/// Called automatically in ViewDidLoad()
		/// </summary>
		protected void StartLoadingScreen (string message)
		{
			using (var pool = new NSAutoreleasePool ()) {
				this.InvokeOnMainThread(delegate {
					loadingView = new UILoadingView (message);
					// because DialogViewController is a UITableViewController,
					// we need to step OVER the UITableView, otherwise the loadingOverlay
					// sits *in* the scrolling area of the table
					this.View.Superview.Add (loadingView);
					this.View.Superview.BringSubviewToFront (loadingView);
					this.View.UserInteractionEnabled = false;
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
				this.InvokeOnMainThread(delegate {
					if (loadingView != null)
					{
						Debug.WriteLine ("Fade out loading...");
						loadingView.OnFinishedFadeOutAndRemove += delegate {
							if (loadingView != null)
							{
								Debug.WriteLine ("Disposing of loadingView object..");
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
	}
}