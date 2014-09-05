using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace CreditCardValidation.iOS
{
	public partial class CreditCardValidationScreen : UIViewController
	{
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
				if (!IsCCValid(out errMessage))
				{
					// need to update on the main thread to change the border color
					InvokeOnMainThread(() =>
					{
						this.CreditCardTextField.BackgroundColor = UIColor.Yellow;
						this.CreditCardTextField.Layer.BorderColor = UIColor.Red.CGColor;
						this.CreditCardTextField.Layer.BorderWidth = 3;
						this.CreditCardTextField.Layer.CornerRadius = 5;

						ErrorMessagesTextField.Text = errMessage;

					});
				}
				else
				{
					this.NavigationController.PushViewController(new CreditCardValidationSuccess(), true);
				}

			};
		}

		protected bool IsCCValid(out string errMessage)
		{
			errMessage = "";

			if (CreditCardTextField.Text.Length < 16)
			{
				errMessage = "Credit card number is too short.";
				return false;
			}
			else if (CreditCardTextField.Text.Length > 16)
			{
				errMessage = "Credit card number is too long.";
				return false;
			}

			return true;

		}
	}
}

