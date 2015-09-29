using System;
using Foundation;
using UIKit;

namespace Example_StandardControls.Screens.iPhone.DatePicker
{
	public partial class DatePickerSimple_iPhone : UIViewController
	{
		public DatePickerSimple_iPhone (IntPtr handle)
			: base (handle)
		{
		}

		public DatePickerSimple_iPhone ()
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			Title = "Simple Date Picker";

			pkrDate.ValueChanged += (s, e) => {
				lblDate.Text = ((UIDatePicker)s).Date.ToString ();
			};
		}
	}
}