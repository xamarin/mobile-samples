using System;
using Foundation;
using UIKit;
using CoreGraphics;

namespace Example_StandardControls.Screens.iPhone.Labels
{
	public partial class LabelsScreen_iPhone : UIViewController
	{
		UILabel customLabel;

		public LabelsScreen_iPhone (IntPtr handle)
			: base (handle)
		{
		}

		public LabelsScreen_iPhone ()
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			Title = "UILabels";

			customLabel = new UILabel (new CGRect (20, 300, 280, 40)) {
				Text = "A label created programatically",
				TextColor = UIColor.Blue,
				Font = UIFont.FromName ("Helvetica-Bold", 20),
				AdjustsFontSizeToFitWidth = true,
				MinimumFontSize = 12,
				LineBreakMode = UILineBreakMode.TailTruncation,
				Lines = 1,
			};

			View.AddSubview (customLabel);
		}
	}
}