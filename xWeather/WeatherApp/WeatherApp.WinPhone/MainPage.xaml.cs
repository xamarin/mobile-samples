using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace WeatherApp.WinPhone
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private async void GetWeatherButton_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(zipCodeEntry.Text))
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

                    weatherBtn.Content = "Search Again";
                }
            }
        }
    }
}
