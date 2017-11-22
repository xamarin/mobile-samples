using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using UIKit;

namespace Example_StandardControls.Screens.iPad.DatePicker
{
	public partial class DatePickerSimple_iPad : UIViewController
	{
		public DatePickerSimple_iPad (IntPtr handle)
			: base (handle)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			Title = "Simple Date Picker";

			pkrDate.ValueChanged += (s, e) => {
				lblDate.Text = (s as UIDatePicker).Date.ToString ();
			};
		}
	}
}