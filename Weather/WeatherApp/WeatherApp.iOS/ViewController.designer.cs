// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace WeatherApp.iOS
{
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel humidityLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel humidityText { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel locationLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel locationText { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel sourceLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel sourceText { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel sunriseLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel sunriseText { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel sunsetLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel sunsetText { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel tempLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel tempText { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel visibilityLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel visibilityText { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton weatherBtn { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel windLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel windText { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextField zipCodeEntry { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel zipCodeLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel zipCodeSearchLabl { get; set; }

		[Action ("GetWeatherBtn_Click:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void GetWeatherBtn_Click (UIButton sender);

		void ReleaseDesignerOutlets ()
		{
			if (humidityLabel != null) {
				humidityLabel.Dispose ();
				humidityLabel = null;
			}
			if (humidityText != null) {
				humidityText.Dispose ();
				humidityText = null;
			}
			if (locationLabel != null) {
				locationLabel.Dispose ();
				locationLabel = null;
			}
			if (locationText != null) {
				locationText.Dispose ();
				locationText = null;
			}
			if (sourceLabel != null) {
				sourceLabel.Dispose ();
				sourceLabel = null;
			}
			if (sourceText != null) {
				sourceText.Dispose ();
				sourceText = null;
			}
			if (sunriseLabel != null) {
				sunriseLabel.Dispose ();
				sunriseLabel = null;
			}
			if (sunriseText != null) {
				sunriseText.Dispose ();
				sunriseText = null;
			}
			if (sunsetLabel != null) {
				sunsetLabel.Dispose ();
				sunsetLabel = null;
			}
			if (sunsetText != null) {
				sunsetText.Dispose ();
				sunsetText = null;
			}
			if (tempLabel != null) {
				tempLabel.Dispose ();
				tempLabel = null;
			}
			if (tempText != null) {
				tempText.Dispose ();
				tempText = null;
			}
			if (visibilityLabel != null) {
				visibilityLabel.Dispose ();
				visibilityLabel = null;
			}
			if (visibilityText != null) {
				visibilityText.Dispose ();
				visibilityText = null;
			}
			if (weatherBtn != null) {
				weatherBtn.Dispose ();
				weatherBtn = null;
			}
			if (windLabel != null) {
				windLabel.Dispose ();
				windLabel = null;
			}
			if (windText != null) {
				windText.Dispose ();
				windText = null;
			}
			if (zipCodeEntry != null) {
				zipCodeEntry.Dispose ();
				zipCodeEntry = null;
			}
			if (zipCodeLabel != null) {
				zipCodeLabel.Dispose ();
				zipCodeLabel = null;
			}
			if (zipCodeSearchLabl != null) {
				zipCodeSearchLabl.Dispose ();
				zipCodeSearchLabl = null;
			}
		}
	}
}
