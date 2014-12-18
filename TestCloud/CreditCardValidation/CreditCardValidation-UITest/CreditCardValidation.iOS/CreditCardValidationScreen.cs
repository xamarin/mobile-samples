using System;
using System.Drawing;

using CreditCardValidation.Common;

using MonoTouch.UIKit;

namespace CreditCardValidation.iOS
{
    public class CreditCardValidationScreen : UIViewController
    {
        static readonly ICreditCardValidator _validator = new CreditCardValidator();
        UITextField _creditCardTextField;
        UILabel _errorMessagesTextField;
        UIButton _validateButton;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            Title = "Credit Card Validation";
            View.BackgroundColor = UIColor.White;

            _creditCardTextField = new UITextField(new RectangleF(10, 120, 300, 40))
                                   {
                                       AccessibilityIdentifier = "CreditCardTextField",
                                       Font = UIFont.SystemFontOfSize(20)
                                   };
            _creditCardTextField.Layer.BorderColor = UIColor.Black.CGColor;
            _creditCardTextField.Layer.BorderWidth = 0.5f;
            _creditCardTextField.Layer.CornerRadius = 5f;

            _validateButton = new UIButton(new RectangleF(10, 165, 300, 40))
                              {
                                  AccessibilityIdentifier = "ValidateButton",
                                  BackgroundColor = UIColor.FromRGB(52, 152, 219)
                              };
            _validateButton.SetTitle("Validate Credit Card", UIControlState.Normal);
            _validateButton.SetTitleColor(UIColor.White, UIControlState.Normal);
            _validateButton.Layer.CornerRadius = 5;

            _errorMessagesTextField = new UILabel(new RectangleF(10, 210, 300, 40))
                                      {
                                          AccessibilityIdentifier = "ErrorMessagesTextField",
                                          Text = String.Empty
                                      };

            _validateButton.TouchUpInside += (sender, e) =>{
                                                 _errorMessagesTextField.Text = String.Empty;

                                                 // perform a simple "required" validation
                                                 string errMessage;
                                                 var isValid = _validator.IsCCValid(_creditCardTextField.Text, out errMessage);

                                                 if (!isValid)
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
    }
}
