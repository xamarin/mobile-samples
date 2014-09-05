// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;
using System.CodeDom.Compiler;

namespace CreditCardValidation.iOS
{
	[Register ("CreditCardValidationScreen")]
	partial class CreditCardValidationScreen
	{
		[Outlet]
		MonoTouch.UIKit.UITextField CreditCardTextField { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextView ErrorMessagesTextField { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton ValidateButton { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (CreditCardTextField != null) {
				CreditCardTextField.Dispose ();
				CreditCardTextField = null;
			}

			if (ValidateButton != null) {
				ValidateButton.Dispose ();
				ValidateButton = null;
			}

			if (ErrorMessagesTextField != null) {
				ErrorMessagesTextField.Dispose ();
				ErrorMessagesTextField = null;
			}
		}
	}
}
