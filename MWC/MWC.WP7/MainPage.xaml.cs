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

namespace MWC.WP7
{
    public partial class MainPage : PhoneApplicationPage
    {
        public MainPage ()
        {
            InitializeComponent();

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

            switch (selectedText) {
                case "monday":
                    args = "?dayOfWeek=" + DayOfWeek.Monday;
                    break;
                case "tuesday":
                    args = "?dayOfWeek=" + DayOfWeek.Tuesday;
                    break;
                case "wednesday":
                    args = "?dayOfWeek=" + DayOfWeek.Wednesday;
                    break;
                case "thursday":
                    args = "?dayOfWeek=" + DayOfWeek.Thursday;
                    break;
                case "friday":
                    args = "?dayOfWeek=" + DayOfWeek.Friday;
                    break;
                case "saturday":
                    args = "?dayOfWeek=" + DayOfWeek.Saturday;
                    break;
                case "sunday":
                    args = "?dayOfWeek=" + DayOfWeek.Sunday;
                    break;
                default:                    
                    break;
            }

            NavigationService.Navigate (new Uri ("/SessionList.xaml" + args, UriKind.RelativeOrAbsolute));
        }
    }
}
