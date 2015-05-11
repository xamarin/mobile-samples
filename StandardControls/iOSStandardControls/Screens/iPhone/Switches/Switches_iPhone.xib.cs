using System;
using UIKit;

namespace Example_StandardControls.Screens.iPhone.Switches
{
	public partial class Switches_iPhone : UIViewController
	{
		public Switches_iPhone (IntPtr handle)
			: base (handle)
		{
		}

		public Switches_iPhone ()
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			Title = "Switches";

			swchOne.ValueChanged += delegate {
				new UIAlertView ("Switch one change!", "is on: " + swchOne.On.ToString (), null, "OK", null).Show ();
			};
			swchTwo.ValueChanged += delegate {
				new UIAlertView ("Switch two change!", "is on: " + swchTwo.On.ToString (), null, "OK", null).Show ();
			};
		}
	}
}