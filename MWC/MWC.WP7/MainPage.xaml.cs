using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.Globalization;
using System.Windows.Markup;

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
    }
}
