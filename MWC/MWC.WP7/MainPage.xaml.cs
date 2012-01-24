using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using Microsoft.Phone.Controls;
using MWC.WP7.ViewModels;
using Microsoft.Phone.Tasks;
using System.Device.Location;

namespace MWC.WP7
{
    public partial class MainPage : PhoneApplicationPage
    {
        public MainPage ()
        {
            InitializeComponent();

            Language = XmlLanguage.GetLanguage (CultureInfo.CurrentUICulture.Name);

            DataContext = App.ViewModel;
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
        }

        void MainPage_Loaded (object sender, RoutedEventArgs e)
        {
            if (!App.ViewModel.HasBeenUpdated) {
                App.ViewModel.BeginUpdate (Dispatcher);
            }
        }

        private void HandleSessionTap (object sender, System.Windows.Input.GestureEventArgs e)
        {
            var item = (ListBoxItem)sender;

            var args = "";

            if (item.DataContext != null && item.DataContext is DateTime) {
                args = "?dayOfWeek=" + ((DateTime)item.DataContext).DayOfWeek;
            }
            else if (item.DataContext != null && item.DataContext is string) {
                args = "?" + Uri.EscapeDataString ((string)item.DataContext);
            }

            NavigationService.Navigate (new Uri ("/SessionList.xaml" + args, UriKind.RelativeOrAbsolute));
        }

        private void HandleSpeakerTap (object sender, System.Windows.Input.GestureEventArgs e)
        {
            var item = ((Grid)sender).DataContext as SpeakerListItemViewModel;

            if (item != null) {
                NavigationService.Navigate (new Uri ("/SpeakerDetails.xaml?id=" + item.ID, UriKind.RelativeOrAbsolute));
            }
        }

        private void HandleNewsItemTap (object sender, System.Windows.Input.GestureEventArgs e)
        {
            var item = ((Grid)sender).DataContext as NewsItemViewModel;

            if (item != null) {
                var task = new WebBrowserTask {
                    Uri = new Uri (item.Url, UriKind.RelativeOrAbsolute),
                };
                task.Show ();
            }
        }

        private void HandleTwitterTap (object sender, System.Windows.Input.GestureEventArgs e)
        {
            var item = ((Grid)sender).DataContext as TweetViewModel;

            if (item != null) {
                NavigationService.Navigate (new Uri ("/TweetDetails.xaml?id=" + item.ID, UriKind.RelativeOrAbsolute));
            }
        }

        private void HandleExhibitorTap (object sender, System.Windows.Input.GestureEventArgs e)
        {
            var item = ((Grid)sender).DataContext as ExhibitorListItemViewModel;

            if (item != null) {
                NavigationService.Navigate (new Uri ("/ExhibitorDetails.xaml?id=" + item.ID, UriKind.RelativeOrAbsolute));
            }
        }

        private void HandleMap (object sender, System.Windows.Input.GestureEventArgs e)
        {
            var task = new BingMapsTask {
                Center = new GeoCoordinate (41.374377, 2.152226),
                SearchTerm = "Fira de Barcelona",
            };
            task.Show ();
        }

        private void HandleAboutXamarin (object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate (new Uri ("/AboutXamarin.xaml", UriKind.RelativeOrAbsolute));
        }
    }
}
