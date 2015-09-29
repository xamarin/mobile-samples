using System;

using UIKit;
using Foundation;
using CoreGraphics;

namespace Example_StandardControls.Controls
{
	[Register ("TapZoomScrollView")]
	public class TapZoomScrollView : UIScrollView
	{
		public TapZoomScrollView (IntPtr handle) : base (handle)
		{
		}

		[Export ("initWithCoder:")]
		public TapZoomScrollView (NSCoder coder) : base (coder)
		{
		}

		public TapZoomScrollView ()
		{
		}

		public TapZoomScrollView (CGRect frame) : base (frame)
		{
		}

		public override void TouchesBegan (Foundation.NSSet touches, UIEvent evt)
		{
			base.TouchesBegan (touches, evt);

			var touch = (UITouch)touches.AnyObject;

			if (touch.TapCount == 2) {
				if (ZoomScale >= 2)
					SetZoomScale (1, true);
				else
					SetZoomScale (3, true);
			}
		}
	}
}