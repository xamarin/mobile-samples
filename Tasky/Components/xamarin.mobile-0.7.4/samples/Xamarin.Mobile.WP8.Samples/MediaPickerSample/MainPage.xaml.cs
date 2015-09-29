using Microsoft.Phone.Controls;

namespace MediaPickerSample
{
	public partial class MainPage : PhoneApplicationPage
	{
		public MainPage()
		{
			DataContext = new MainPageViewModel();
			InitializeComponent();
		}
	}
}