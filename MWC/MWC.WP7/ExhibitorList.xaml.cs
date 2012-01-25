using System;
using System.IO.IsolatedStorage;
using System.Threading;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using MWC.BL.Managers;
using MWC.WP7.ViewModels;

namespace MWC.WP7
{
    public partial class ExhibitorList : PhoneApplicationPage
    {
        public ExhibitorList ()
        {
            InitializeComponent ();
        }

        DateTime NextExhibitorsUpdateTimeUtc
        {
            get
            {
                return IsolatedStorageSettings.ApplicationSettings.Contains ("NextExhibitorsUpdateTimeUtc") ?
                    (DateTime)IsolatedStorageSettings.ApplicationSettings["NextExhibitorsUpdateTimeUtc"] :
                    DateTime.UtcNow.AddHours (-1);
            }
            set
            {
                IsolatedStorageSettings.ApplicationSettings["NextExhibitorsUpdateTimeUtc"] = value;
            }
        }

        protected override void OnNavigatedTo (System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo (e);

            if (e.NavigationMode == System.Windows.Navigation.NavigationMode.New) {
                var vm = new ExhibitorListViewModel ();

                //
                // Update the Exhibitors DB if it's time for an update
                //
                if (DateTime.UtcNow >= NextExhibitorsUpdateTimeUtc) {
                    ThreadPool.QueueUserWorkItem (delegate {
                        UpdateManager.UpdateExhibitorsFinished += delegate {
                            Dispatcher.BeginInvoke (delegate {
                                NextExhibitorsUpdateTimeUtc = DateTime.UtcNow.AddHours (1);
                                vm.BeginUpdate (Dispatcher);
                            });
                        };
                        UpdateManager.UpdateExhibitors ();
                    });
                }

                vm.BeginUpdate (Dispatcher);

                DataContext = vm;
            }
        }

        private void HandleExhibitorTap (object sender, System.Windows.Input.GestureEventArgs e)
        {
            var item = ((Grid)sender).DataContext as ExhibitorListItemViewModel;

            if (item != null) {
                NavigationService.Navigate (new Uri ("/ExhibitorDetails.xaml?id=" + item.ID, UriKind.RelativeOrAbsolute));
            }
        }
    }
}
