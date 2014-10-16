using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using CreditCardValidation.Common;

namespace CreditCardValidation.iOS
{
	public partial class CreditCardValidationScreen : UIViewController
	{
        static readonly ICreditCardValidator _creditCardValidator = new CreditCardValidator();
		protected bool _valid;

		public CreditCardValidationScreen() : base("CreditCardValidationScreen", null)
		{
		}

		public override void DidReceiveMemoryWarning()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning();
			
			// Release any cached data, images, etc that aren't in use.
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			this.ValidateButton.TouchUpInside += (object sender, EventArgs e) =>
			{
				ErrorMessagesTextField.Text = String.Empty;

				// perform a simple "required" validation
				string errMessage;
                bool isValid = _creditCardValidator.IsCCValid(CreditCardTextField.Text, out errMessage);
				if (!isValid)
				{
					// need to update on the main thread to change the border color
					InvokeOnMainThread(() =>
					{
						CreditCardTextField.BackgroundColor = UIColor.Yellow;
						CreditCardTextField.Layer.BorderColor = UIColor.Red.CGColor;
						CreditCardTextField.Layer.BorderWidth = 3;
						CreditCardTextField.Layer.CornerRadius = 5;

						ErrorMessagesTextField.Text = errMessage;

					});
				}
				else
				{
					this.NavigationController.PushViewController(new CreditCardValidationSuccess(), true);
				}

			};
		}
	}
}

