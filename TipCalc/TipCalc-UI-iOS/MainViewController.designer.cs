// WARNING
//
// This file has been generated automatically by MonoDevelop to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;

namespace TipCalcUIiOS
{
	[Register ("MainViewController")]
	partial class MainViewController
	{
		[Outlet]
		UIKit.UITextField Subtotal { get; set; }

		[Outlet]
		UIKit.UITextField ReceiptTotal { get; set; }

		[Outlet]
		UIKit.UITextField TipPercent { get; set; }

		[Outlet]
		UIKit.UISlider TipPercentSlider { get; set; }

		[Outlet]
		UIKit.UITextField TipValue { get; set; }

		[Outlet]
		UIKit.UITextField Total { get; set; }

		[Outlet]
		UIKit.UIScrollView ScrollView { get; set; }

		[Action ("showInfo:")]
		partial void showInfo (Foundation.NSObject sender);
	}
}
