using System;
using System.Device.Location;
using System.Globalization;
using System.IO;
using System.IO.IsolatedStorage;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using MWC.BL.Managers;
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

        bool IsDataSeeded
        {
            get
            {
                return IsolatedStorageSettings.ApplicationSettings.Contains ("IsDataSeeded") &&
                    (bool)IsolatedStorageSettings.ApplicationSettings["IsDataSeeded"];
            }
            set
            {
                IsolatedStorageSettings.ApplicationSettings["IsDataSeeded"] = value;
            }
        }

        DateTime NextConferenceUpdateTimeUtc
        {
            get
            {
                return IsolatedStorageSettings.ApplicationSettings.Contains ("NextConferenceUpdateTimeUtc") ?
                    (DateTime)IsolatedStorageSettings.ApplicationSettings["NextConferenceUpdateTimeUtc"] :
                    DateTime.UtcNow.AddHours (-1);
            }
            set
            {
                IsolatedStorageSettings.ApplicationSettings["NextConferenceUpdateTimeUtc"] = value;
            }
        }

        void UpdateDatabase ()
        {
            //
            // Seed the Conference DB
            //
            var isDataSeeded = IsDataSeeded;
            if (!isDataSeeded) {
                using (var s = Application.GetResourceStream (new Uri ("Assets\\SeedData.xml", UriKind.Relative)).Stream) {
                    using (var r = new StreamReader (s)) {
                        UpdateManager.UpdateFromFile (r.ReadToEnd ());
                    }
                }
                IsDataSeeded = true;
            }

            //
            // Update the Speakers & Session DB if it's time for an update
            //
            if (DateTime.UtcNow >= NextConferenceUpdateTimeUtc) {
                ThreadPool.QueueUserWorkItem (delegate {
                    UpdateManager.UpdateFinished += delegate {
                        Dispatcher.BeginInvoke (delegate {
                            NextConferenceUpdateTimeUtc = DateTime.UtcNow.AddHours (1);
                        });
                    };
                    UpdateManager.UpdateConference ();
                });
            }
        }

        bool _dbUpdated = false;

        void MainPage_Loaded (object sender, RoutedEventArgs e)
        {
            if (!_dbUpdated) {
                
                UpdateDatabase ();
                App.ViewModel.BeginUpdate (Dispatcher);

                _dbUpdated = true;
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

        private void HandleSpeakers (object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate (new Uri ("/SpeakerList.xaml", UriKind.RelativeOrAbsolute));
        }

        private void HandleExhibitors (object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate (new Uri ("/ExhibitorList.xaml", UriKind.RelativeOrAbsolute));
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

        private void Panorama_SelectionChanged (object sender, SelectionChangedEventArgs e)
        {
            var headerText = ((PanoramaItem)((Panorama)sender).SelectedItem).Header.ToString ();
            ApplicationBar.IsVisible = (headerText == "twitter" || headerText == "news");
        }

        private void HandleRefresh (object sender, EventArgs e)
        {
            var vm = (MainViewModel)DataContext;
            var headerText = ((PanoramaItem)MainPanorama.SelectedItem).Header.ToString ();

            if (headerText == "twitter") {
                vm.Twitter.BeginUpdate (Dispatcher);
            }
            else if (headerText == "news") {
                vm.News.BeginUpdate (Dispatcher);
            }
        }
    }
}
