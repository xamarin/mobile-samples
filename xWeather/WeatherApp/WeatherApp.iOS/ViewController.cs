using System;
using UIKit;

namespace WeatherApp.iOS
{
	public partial class ViewController : UIViewController
	{
		public ViewController (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad ()
		{
            base.ViewDidLoad();
            this.weatherBtn.Layer.BorderColor = UIColor.White.CGColor;
            this.weatherBtn.Layer.BorderWidth = 1;
        }

		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
			// Release any cached data, images, etc that aren't in use.
		}

        async partial void GetWeatherBtn_Click(UIButton sender)
        {
            if (!String.IsNullOrEmpty(this.zipCodeEntry.Text))
            {
                Weather weather = await Core.GetWeather(zipCodeEntry.Text);
                if (weather != null)
                {
                    locationText.Text = weather.Title;
                    tempText.Text = weather.Temperature;
                    windText.Text = weather.Wind;
                    visibilityText.Text = weather.Visibility;
                    humidityText.Text = weather.Humidity;
                    sunriseText.Text = weather.Sunrise;
                    sunsetText.Text = weather.Sunset;
                }
            }
        }
    }
}

