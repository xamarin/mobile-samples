// WARNING
//
// This file has been generated automatically by MonoDevelop to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace MultiThreading
{
	[Register ("MainScreen_iPhone")]
	partial class MainScreen_iPhone
	{
		[Outlet]
		MonoTouch.UIKit.UIButton StartBackgroundTaskButton { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton StartBackgroundTaskNoUpdateButton { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel OutputLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (StartBackgroundTaskButton != null) {
				StartBackgroundTaskButton.Dispose ();
				StartBackgroundTaskButton = null;
			}

			if (StartBackgroundTaskNoUpdateButton != null) {
				StartBackgroundTaskNoUpdateButton.Dispose ();
				StartBackgroundTaskNoUpdateButton = null;
			}

			if (OutputLabel != null) {
				OutputLabel.Dispose ();
				OutputLabel = null;
			}
		}
	}
}
