// WARNING
//
// This file has been generated automatically by MonoDevelop to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace PhonewordiOS
{
	[Register ("PhonewordiOSViewController")]
	partial class PhonewordiOSViewController
	{
		[Outlet]
		MonoTouch.UIKit.UITextField PhoneNumberText { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton TranslateButton { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton CallButton { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (PhoneNumberText != null) {
				PhoneNumberText.Dispose ();
				PhoneNumberText = null;
			}

			if (TranslateButton != null) {
				TranslateButton.Dispose ();
				TranslateButton = null;
			}

			if (CallButton != null) {
				CallButton.Dispose ();
				CallButton = null;
			}
		}
	}
}
