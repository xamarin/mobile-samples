using System;
using UIKit;

namespace Example_StandardControls.Screens.iPhone.Sliders
{
	public partial class Sliders_iPhone : UIViewController
	{
		public Sliders_iPhone (IntPtr handle)
			: base (handle)
		{
		}

		public Sliders_iPhone ()
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			Title = "Sliders";

			sldrWithImages.SetThumbImage (UIImage.FromFile ("Images/Icons/29_icon.png"), UIControlState.Normal);
		}
	}
}