using System;
using Foundation;
using UIKit;

namespace Example_StandardControls.Screens.iPhone.ProgressBars
{
	public partial class ProgressBars_iPhone : UIViewController
	{
		public ProgressBars_iPhone (IntPtr handle)
			: base (handle)
		{
		}

		public ProgressBars_iPhone ()
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			Title = "Progress Bars";
		}
	}
}