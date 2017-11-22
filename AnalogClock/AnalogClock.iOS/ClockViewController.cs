using System;
using System.ComponentModel;
using UIKit;

using AnalogClock.Common;

namespace AnalogClock.iOS
{
	public class ClockViewController : UIViewController
	{
		ClockView clockView;
		ClockModel clockModel;

		public ClockViewController ()
		{
		}

		public override void LoadView ()
		{
			base.LoadView ();

			// Instantiate the ClockView.
			clockView = new ClockView {
				Frame = UIScreen.MainScreen.Bounds
			};

			// Set it to the view of this controller.
			this.View = clockView;

			// Create the Clock model.
			clockModel = new ClockModel {
				NeedsRadians = true
			};

			// Initialize clock.
			SetClockHandAngles ();

			clockModel.PropertyChanged += (object sender, PropertyChangedEventArgs e) => 
			{
				// Update clock.
				SetClockHandAngles();
			};
		}

		void SetClockHandAngles()
		{
			clockView.SetClockHandAngles(clockModel.HourAngle, 
										 clockModel.MinuteAngle, 
										 clockModel.SecondAngle);
		}
	}
}

