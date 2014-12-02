// WARNING
//
// This file has been generated automatically by MonoDevelop to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;

namespace MultiThreading
{
	[Register ("MainScreen_iPhone")]
	partial class MainScreen_iPhone
	{
		[Outlet]
		UIKit.UIButton StartBackgroundTaskButton { get; set; }

		[Outlet]
		UIKit.UIButton StartBackgroundTaskNoUpdateButton { get; set; }

		[Outlet]
		UIKit.UILabel OutputLabel { get; set; }
		
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
