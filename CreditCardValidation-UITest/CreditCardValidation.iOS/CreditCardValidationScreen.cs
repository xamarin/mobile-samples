using System;
using System.Drawing;

using MonoTouch.UIKit;

namespace CreditCardValidation.iOS
{
    public class CreditCardValidationScreen : UIViewController
    {
        UITextView _creditCardTextField;
        UILabel _errorMessagesTextField;
        UIButton _validateButton;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            Title = "Credit Card Validation";
            View.BackgroundColor = UIColor.White;

            _creditCardTextField = new UITextView(new RectangleF(10, 120, 300, 40));
            _creditCardTextField.SetAccessibilityId("CreditCardTextField");
            _creditCardTextField.Layer.BorderColor = UIColor.Black.CGColor;
            _creditCardTextField.Layer.BorderWidth = 0.5f;
            _creditCardTextField.Layer.CornerRadius = 5f;
            _creditCardTextField.Font = UIFont.SystemFontOfSize(20);

            _validateButton = new UIButton(new RectangleF(10, 165, 300, 40));
            _validateButton.SetTitle("Validate Credit Card", UIControlState.Normal);
            _validateButton.SetAccessibilityId("ValidateButton");
            _validateButton.SetTitleColor(UIColor.White, UIControlState.Normal);
            _validateButton.BackgroundColor = UIColor.FromRGB(52, 152, 219);
            _validateButton.Layer.CornerRadius = 5;

            _errorMessagesTextField = new UILabel(new RectangleF(10, 210, 300, 40));
            _errorMessagesTextField.SetAccessibilityId("ErrorMessagesTextField");
            _errorMessagesTextField.Text = String.Empty;

            _validateButton.TouchUpInside += (object sender, EventArgs e) =>{
                                                 _errorMessagesTextField.Text = String.Empty;

                                                 // perform a simple "required" validation
                                                 string errMessage;
                                                 if (!IsCCValid(out errMessage))
                                                 {
                                                     // need to update on the main thread to change the border color
                                                     InvokeOnMainThread(() =>{
                                                                            _creditCardTextField.BackgroundColor = UIColor.Yellow;
                                                                            _creditCardTextField.Layer.BorderColor = UIColor.Red.CGColor;
                                                                            _creditCardTextField.Layer.BorderWidth = 3;
                                                                            _creditCardTextField.Layer.CornerRadius = 5;
                                                                            _errorMessagesTextField.Text = errMessage;
                                                                        });
                                                 }
                                                 else
                                                 {
                                                     NavigationController.PushViewController(new CreditCardValidationSuccess(), true);
                                                 }
                                             };

            View.Add(_creditCardTextField);
            View.Add(_validateButton);
            View.Add(_errorMessagesTextField);
        }

        bool IsCCValid(out string errMessage)
        {
            errMessage = "";

            if (_creditCardTextField.Text.Length < 16)
            {
                errMessage = "Credit card number is to short.";
                return false;
            }
            if (_creditCardTextField.Text.Length > 16)
            {
                errMessage = "Credit card number is to long.";
                return false;
            }

            return true;
        }
    }
}
