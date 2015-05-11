using System;
using UIKit;
using CoreGraphics;

namespace Example_StandardControls.Screens.iPhone.TextFields
{
	public partial class TextFields_iPhone : UIViewController
	{
		UITextField textField;

		public TextFields_iPhone (IntPtr handle)
			: base (handle)
		{
		}

		public TextFields_iPhone ()
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			Title = "UITextField";

			textField = new UITextField (new CGRect (20, 150, 280, 33)) {
				Font = UIFont.FromName ("Helvetica-Bold", 20),
				BorderStyle = UITextBorderStyle.Bezel,
				Placeholder = "edit me!",
			};
			View.AddSubview (textField);
		}
	}
}