using System.Threading;
using Windows.UI.Xaml.Controls;
using Xamarin.Geolocation;

namespace GeolocationSample
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
	        DataContext = new MainPageViewModel (new Geolocator(), SynchronizationContext.Current);
        }
    }
}
