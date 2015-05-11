using System;
using UIKit;
using CoreGraphics;

namespace Example_StandardControls.Screens.iPhone.SegmentedControl
{
	public partial class SegmentedControls2_iPhone : UIViewController
	{
		UISegmentedControl segControl;

		public SegmentedControls2_iPhone (IntPtr handle)
			: base (handle)
		{
		}

		public SegmentedControls2_iPhone ()
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			Title = "Programmatic Segmented Controls";

			segControl = new UISegmentedControl {
				Frame = new CGRect (20, 20, 280, 44),
				ControlStyle = UISegmentedControlStyle.Bordered,
				SelectedSegment = 1,
			};
			segControl.InsertSegment ("One", 0, false);
			segControl.InsertSegment ("Two", 1, false);
			segControl.SetWidth (100f, 1);
			View.AddSubview (segControl);

			segControl.ValueChanged += (object sender, EventArgs e) => {
				var selectedSegment = ((UISegmentedControl)sender).SelectedSegment;
				Console.WriteLine (string.Format ("Item {0} selected", selectedSegment));
			};
		}
	}
}