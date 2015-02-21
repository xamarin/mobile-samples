// WARNING
//
// This file has been generated automatically by MonoDevelop to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;

namespace DataAccess
{
	[Register ("DataAccessViewController")]
	partial class DataAccessViewController
	{
		[Outlet]
		UIKit.UIButton AdoExampleButton { get; set; }

		[Outlet]
		UIKit.UIButton OrmExampleButton { get; set; }

		[Outlet]
		UIKit.UITextView OutputText { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (AdoExampleButton != null) {
				AdoExampleButton.Dispose ();
				AdoExampleButton = null;
			}

			if (OrmExampleButton != null) {
				OrmExampleButton.Dispose ();
				OrmExampleButton = null;
			}

			if (OutputText != null) {
				OutputText.Dispose ();
				OutputText = null;
			}
		}
	}
}
