using System;
using OpenTK;
using OpenTK.Graphics.ES30;
using OpenTK.Platform.iPhoneOS;
using Foundation;
using CoreAnimation;
using ObjCRuntime;
using OpenGLES;
using UIKit;

namespace GLKeysES30
{
	[Register ("OpenGLViewController")]
	public partial class OpenGLViewController : UIViewController
	{
		public OpenGLViewController (string nibName, NSBundle bundle) : base (nibName, bundle)
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
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
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

//		public override void DidRotate (UIInterfaceOrientation fromInterfaceOrientation)
//		{
//			base.DidRotate (fromInterfaceOrientation);
//			View.SetupProjection ();
//		}

		public override void WillAnimateSecondHalfOfRotation (UIInterfaceOrientation fromInterfaceOrientation, double duration)
		{
			base.WillAnimateSecondHalfOfRotation (fromInterfaceOrientation, duration);
			View.SetupProjection ();
		}
	}
}
