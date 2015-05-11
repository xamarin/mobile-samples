using System;
using Foundation;
using UIKit;

namespace Example_StandardControls.Screens.iPhone.Buttons
{
	public partial class ButtonsScreen_iPhone : UIViewController
	{
		public ButtonsScreen_iPhone (IntPtr handle) : base (handle)
		{
		}

		public ButtonsScreen_iPhone ()
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			Title = "Buttons";

			btnOne.TouchUpInside += HandleBtnOneTouchUpInside;
			btnTwo.TouchUpInside += delegate {
				new UIAlertView ("button two click!", "TouchUpInside Handled", null, "OK", null).Show ();
			};

			UIButton button = UIButton.FromType (UIButtonType.RoundedRect);
			button.SetTitle ("My Button", UIControlState.Normal);
		}

		protected void HandleBtnOneTouchUpInside (object sender, EventArgs e)
		{
			new UIAlertView ("button one click!", "TouchUpInside Handled", null, "OK", null).Show ();
		}
	}
}