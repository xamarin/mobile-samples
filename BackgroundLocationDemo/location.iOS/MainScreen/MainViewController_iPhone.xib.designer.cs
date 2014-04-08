// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace Location.iOS.MainScreen
{
	[Register ("MainViewController_iPhone")]
	partial class MainViewController_iPhone
	{
		[Outlet]
		MonoTouch.UIKit.UILabel lblAltitude { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel lblCourse { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel lblLatitude { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel lblLongitude { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel lblSpeed { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (lblAltitude != null) {
				lblAltitude.Dispose ();
				lblAltitude = null;
			}

			if (lblCourse != null) {
				lblCourse.Dispose ();
				lblCourse = null;
			}

			if (lblLatitude != null) {
				lblLatitude.Dispose ();
				lblLatitude = null;
			}

			if (lblLongitude != null) {
				lblLongitude.Dispose ();
				lblLongitude = null;
			}

			if (lblSpeed != null) {
				lblSpeed.Dispose ();
				lblSpeed = null;
			}
		}
	}
}
