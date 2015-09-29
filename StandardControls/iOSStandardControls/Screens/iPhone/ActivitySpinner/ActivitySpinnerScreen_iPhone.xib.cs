using System;
using UIKit;
using Foundation;

namespace Example_StandardControls.Screens.iPhone.ActivitySpinner
{
	public partial class ActivitySpinnerScreen_iPhone : UIViewController
	{
		public ActivitySpinnerScreen_iPhone (IntPtr handle)
			: base (handle)
		{
		}

		public ActivitySpinnerScreen_iPhone ()
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			Title = "Activity Spinners";
		}
	}
}