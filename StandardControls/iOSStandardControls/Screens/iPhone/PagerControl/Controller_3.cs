using System;
using UIKit;
using CoreGraphics;

namespace Example_StandardControls.Screens.iPhone.PagerControl
{
	public class Controller_3 : UIViewController
	{
		UILabel lblMain;

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();


			lblMain = new UILabel (new CGRect (20, 200, 280, 33)) {
				Text = "Controller 3",
				BackgroundColor = UIColor.Clear,
			};
			View.AddSubview (lblMain);
			View.BackgroundColor = UIColor.FromRGB (.5f, .5f, 1);
		}
	}
}