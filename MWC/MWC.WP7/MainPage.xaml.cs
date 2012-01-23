using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using Microsoft.Phone.Controls;
using MWC.WP7.ViewModels;

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
            App.ViewModel.LoadData (Dispatcher);
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
    }
}
