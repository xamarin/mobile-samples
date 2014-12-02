using System.Drawing;

using MonoTouch.UIKit;

namespace CreditCardValidation.iOS
{
    public class CreditCardValidationSuccess : UIViewController
    {
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            View.BackgroundColor = UIColor.White;
            Title = "Valid Credit Card";

            UILabel successLabel = new UILabel(new RectangleF(10, 165, 300, 40));
            successLabel.Text = "The credit card number is valid!";
            successLabel.TextAlignment = UITextAlignment.Center;
            successLabel.AccessibilityLabel = "CreditCardIsValidLabel";

            View.Add(successLabel);
        }
    }
}
