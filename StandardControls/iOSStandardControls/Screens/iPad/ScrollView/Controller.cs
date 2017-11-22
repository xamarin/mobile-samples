using System;
using UIKit;
using System.Drawing;
using CoreGraphics;

namespace Example_StandardControls.Screens.iPad.ScrollView
{
	public class Controller : UIViewController
	{
		UIScrollView scrollView;

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			View.BackgroundColor = UIColor.White;

			scrollView = new UIScrollView (View.Frame);
			View.AddSubview (scrollView);
		}
	}
}
