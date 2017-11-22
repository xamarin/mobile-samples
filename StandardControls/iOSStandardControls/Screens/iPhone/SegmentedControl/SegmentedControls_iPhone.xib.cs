using System;
using Foundation;
using UIKit;

namespace Example_StandardControls.Screens.iPhone.SegmentedControl
{
	public partial class SegmentedControls_iPhone : UIViewController
	{
		public SegmentedControls_iPhone (IntPtr handle)
			: base (handle)
		{
		}

		public SegmentedControls_iPhone ()
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			Title = "Segmented Controls";
		}
	}
}