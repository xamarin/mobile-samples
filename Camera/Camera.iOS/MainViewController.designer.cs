// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;
using System.CodeDom.Compiler;

namespace Camera.iOS
{
	[Register ("MainViewController")]
	partial class MainViewController
	{
		[Outlet]
		MonoTouch.UIKit.UIButton bntCamera { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel lblSuccess { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (bntCamera != null) {
				bntCamera.Dispose ();
				bntCamera = null;
			}

			if (lblSuccess != null) {
				lblSuccess.Dispose ();
				lblSuccess = null;
			}
		}
	}
}
