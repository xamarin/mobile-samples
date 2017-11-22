using System;
using System.ComponentModel;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

// for ClockModel.
using AnalogClock.Common;

namespace AnalogClock.Android
{
	[Activity (Label = "AndroidAnalogClock", MainLauncher = true)]
	public class MainActivity : Activity
	{
		ClockView clockView;
		ClockModel clockModel;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set content view to FrameLayout with ClockView.
			FrameLayout frameLayout = new FrameLayout (this);
			clockView = new ClockView (this);
			frameLayout.AddView (clockView);
			SetContentView (frameLayout);

			// Create ClockModel to keep clock updated.
			clockModel = new ClockModel {
				IsSweepSecondHand = true
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


