using System;
using Foundation;
using UIKit;

namespace Example_StandardControls.Screens.iPhone.Toolbar
{
	public partial class Toolbar1_iPhone : UIViewController
	{
		public Toolbar1_iPhone (IntPtr handle)
			: base (handle)
		{
		}

		public Toolbar1_iPhone ()
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			Title = "Toolbar";

			btnOne.Clicked += (s, e) => {
				new UIAlertView ("click!", "btnOne clicked", null, "OK", null).Show ();
			};
		}

		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			return true;
		}
	}
}

