// WARNING
//
// This file has been generated automatically by MonoDevelop to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;

namespace DataAccess
{
	[Register ("StockDetailViewController")]
	partial class StockDetailViewController
	{
		[Outlet]
		UIKit.UIButton SaveButton { get; set; }

		[Outlet]
		UIKit.UIButton DeleteButton { get; set; }

		[Outlet]
		UIKit.UITextField Code { get; set; }

		[Outlet]
		UIKit.UITextField Name { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (SaveButton != null) {
				SaveButton.Dispose ();
				SaveButton = null;
			}

			if (DeleteButton != null) {
				DeleteButton.Dispose ();
				DeleteButton = null;
			}

			if (Code != null) {
				Code.Dispose ();
				Code = null;
			}

			if (Name != null) {
				Name.Dispose ();
				Name = null;
			}
		}
	}
}
