using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Core;
using Windows.UI.Input;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace WinPhone81
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        // Constructor
        public MainPage()
        {
            this.InitializeComponent();

            DataContext = new TaskListViewModel();
            Loaded += new RoutedEventHandler(MainPage_Loaded);
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            ((TaskListViewModel)DataContext).BeginUpdate(Dispatcher);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ((TaskListViewModel)DataContext).BeginUpdate(Dispatcher);
        }

        private void HandleTaskTap(object sender, TappedRoutedEventArgs e)
        {
            var item = ((Grid)sender).DataContext as TaskViewModel;

            if (item != null)
            {
                this.Frame.Navigate(typeof(WinPhone81.TaskDetailsPage), item.ID);
                //Navigate(new Uri("/TaskDetailsPage.xaml?id=" + item.ID, UriKind.RelativeOrAbsolute));
            }
        }

        private void HandleAdd(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(WinPhone81.TaskDetailsPage), -1);
        }

    }
}