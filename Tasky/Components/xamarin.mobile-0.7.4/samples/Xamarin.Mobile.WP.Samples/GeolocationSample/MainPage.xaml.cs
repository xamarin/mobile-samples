using System.Threading;
using Microsoft.Phone.Controls;
using Xamarin.Geolocation;

namespace GeolocationSample
{
	public partial class MainPage : PhoneApplicationPage
	{
		// Constructor
		public MainPage()
		{
			DataContext = new MainPageViewModel (new Geolocator(), SynchronizationContext.Current);
			InitializeComponent();
		}
	}
}