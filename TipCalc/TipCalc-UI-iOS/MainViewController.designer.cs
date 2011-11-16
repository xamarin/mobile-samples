// WARNING
//
// This file has been generated automatically by MonoDevelop to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace TipCalcUIiOS
{
	[Register ("MainViewController")]
	partial class MainViewController
	{
		[Outlet]
		MonoTouch.UIKit.UITextField Subtotal { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextField ReceiptTotal { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextField TipPercent { get; set; }

		[Outlet]
		MonoTouch.UIKit.UISlider TipPercentSlider { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextField TipValue { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextField Total { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIScrollView ScrollView { get; set; }

		[Action ("showInfo:")]
		partial void showInfo (MonoTouch.Foundation.NSObject sender);
	}
}
