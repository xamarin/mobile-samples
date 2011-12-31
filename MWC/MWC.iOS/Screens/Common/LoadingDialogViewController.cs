using System;
using System.Diagnostics;
using MonoTouch.Dialog;
using MonoTouch.Foundation;
using MonoTouch.UIKit;


namespace MWC.iOS.Screens.Common
{
	/// <summary>
	/// Share a 'loading' screen between DialogViewControllers that
	/// are populated from network requests
	/// </summary>
	public class LoadingDialogViewController : DialogViewController
	{
		MWC.iOS.Screens.Common.UILoadingView loadingView;

		public LoadingDialogViewController (UITableViewStyle style, RootElement root) : base(style, root)
		{}
		
		public override void ViewDidLoad ()
        {
			base.ViewDidLoad ();
						
			StartLoadingScreen("Loading...");
			//ThreadPool.QueueUserWorkItem (delegate {
			NSTimer.CreateScheduledTimer (TimeSpan.FromMilliseconds (1), delegate
			{
				LoadData();
			});
			//});
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
					this.View.AddSubview (loadingView);
					this.View.BringSubviewToFront (loadingView);
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