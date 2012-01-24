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

        void HandleSessionSelectionChanged (object sender, EventArgs e)
        {
            var selectedItem = (ListBoxItem)((ListBox)sender).SelectedItem;
            var selectedText = ((TextBlock)selectedItem.Content).Text;

            var args = "";

            if (selectedItem.DataContext != null && selectedItem.DataContext is DateTime) {
                args = "?dayOfWeek=" + ((DateTime)selectedItem.DataContext).DayOfWeek;
            }

            NavigationService.Navigate (new Uri ("/SessionList.xaml" + args, UriKind.RelativeOrAbsolute));
        }

        private void HandleSpeakerSelectionChanged (object sender, SelectionChangedEventArgs e)
        {
            var item = e.AddedItems
                .OfType<LongListSelector.LongListSelectorItem> ()
                .Where (x => x.ItemType == LongListSelector.LongListSelectorItemType.Item)
                .Select (x => x.Item)
                .OfType<SpeakerListItemViewModel> ()
                .FirstOrDefault ();

            if (item != null) {
                NavigationService.Navigate (new Uri ("/SpeakerDetails.xaml?id=" + item.ID, UriKind.RelativeOrAbsolute));
            }
        }

        private void HandleNewsSelectionChanged (object sender, SelectionChangedEventArgs e)
        {
            var item = e.AddedItems
                .OfType<NewsItemViewModel> ()
                .FirstOrDefault ();

            if (item != null) {
                var task = new WebBrowserTask {
                    Uri = new Uri (item.Url, UriKind.RelativeOrAbsolute),
                };
                task.Show ();
            }
        }

        private void HandleTwitterSelectionChanged (object sender, SelectionChangedEventArgs e)
        {
            var item = e.AddedItems
                .OfType<TweetViewModel> ()
                .FirstOrDefault ();

            if (item != null) {
                var task = new WebBrowserTask {
                    Uri = new Uri (item.Url, UriKind.RelativeOrAbsolute),
                };
                task.Show ();
            }
        }

        private void HandleExhibitorSelectionChanged (object sender, SelectionChangedEventArgs e)
        {
            var item = e.AddedItems
                .OfType<LongListSelector.LongListSelectorItem> ()
                .Where (x => x.ItemType == LongListSelector.LongListSelectorItemType.Item)
                .Select (x => x.Item)
                .OfType<ExhibitorListItemViewModel> ()
                .FirstOrDefault ();

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
