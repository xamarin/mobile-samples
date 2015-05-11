using System;
using Foundation;

using UIKit;
using Example_StandardControls.Controls;

namespace Example_StandardControls.Screens.iPhone.DatePicker
{
	public partial class DatePicker_iPhone : UIViewController
	{
		UIViewDatePicker actionSheetDatePicker;
		UIViewDatePicker actionSheetTimerPicker;

		public DatePicker_iPhone (IntPtr handle)
			: base (handle)
		{
		}

		public DatePicker_iPhone ()
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			Title = "Date Picker";

			// setup our custom action sheet date picker
			actionSheetDatePicker = new UIViewDatePicker (View) {
				Title = "Choose Date:"
			};
			actionSheetDatePicker.DatePicker.ValueChanged += OnValueChanged;
			actionSheetDatePicker.DatePicker.Mode = UIDatePickerMode.DateAndTime;
			btnChooseDate.TouchUpInside += (s, e) => {
				actionSheetDatePicker.Show ();
			};

			// setup our countdown timer
			actionSheetTimerPicker = new UIViewDatePicker (View);
			actionSheetTimerPicker.Title = "Choose Time:";
			actionSheetTimerPicker.DatePicker.Mode = UIDatePickerMode.CountDownTimer;
		}

		void OnValueChanged (object sender, EventArgs e)
		{
			lblDate.Text = ((UIDatePicker)sender).Date.ToString ();
		}
	}
}