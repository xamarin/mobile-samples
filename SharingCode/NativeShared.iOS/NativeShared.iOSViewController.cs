using System;
using System.Drawing;

using Foundation;
using UIKit;
using CoreGraphics;
using NativeShared;

namespace NativeShared.iOS
{
	public partial class NativeShared_iOSViewController : UIViewController
	{
		public NativeShared_iOSViewController (IntPtr handle) : base (handle)
		{
		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.

		}

		#region View lifecycle

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			// Testing
			CGRect rect = new CGRect (0, 0, 200, 200);
			Console.WriteLine ("Rectangle Area: {0}", Transformations.CalculateArea (rect));
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
		}

		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);
		}

		public override void ViewDidDisappear (bool animated)
		{
			base.ViewDidDisappear (animated);
		}

		#endregion
	}
}

