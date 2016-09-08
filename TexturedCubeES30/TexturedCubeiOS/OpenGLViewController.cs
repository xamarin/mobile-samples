using System;
using Foundation;
using UIKit;

namespace TexturedCubeiOS
{
	[Register ("OpenGLViewController")]
	public class OpenGLViewController : UIViewController
	{
		public OpenGLViewController (IntPtr handle) : base (handle)
		{
		}

		new EAGLView View { get { return (EAGLView)base.View; } }

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			NSNotificationCenter.DefaultCenter.AddObserver (UIApplication.WillResignActiveNotification, a => {
				if (IsViewLoaded && View.Window != null)
					View.StopAnimating ();
			}, this);
			NSNotificationCenter.DefaultCenter.AddObserver (UIApplication.DidBecomeActiveNotification, a => {
				if (IsViewLoaded && View.Window != null)
					View.StartAnimating ();
			}, this);
			NSNotificationCenter.DefaultCenter.AddObserver (UIApplication.WillTerminateNotification, a => {
				if (IsViewLoaded && View.Window != null)
					View.StopAnimating ();
			}, this);
		}

		protected override void Dispose (bool disposing)
		{
			base.Dispose (disposing);
			
			NSNotificationCenter.DefaultCenter.RemoveObserver (this);
		}

		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			View.StartAnimating ();
		}

		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);
			View.StopAnimating ();
		}
	}
}
