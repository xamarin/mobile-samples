// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace WeatherApp.iOS
{
    [Register ("ViewController")]
    partial class ViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel humidityLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel humidityText { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel locationLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel locationText { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel sunriseLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel sunriseText { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel sunsetLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel sunsetText { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel tempLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel tempText { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel visibilityLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel visibilityText { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton weatherBtn { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel windLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel windText { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField zipCodeEntry { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel zipCodeLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel zipCodeSearchLabl { get; set; }

        [Action ("WeatherBtn_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void GetWeatherBtn_Click (UIKit.UIButton sender);

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