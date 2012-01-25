using System;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using MWC.WP7.ViewModels;

namespace MWC.WP7
{
    public partial class SpeakerList : PhoneApplicationPage
    {
        public SpeakerList ()
        {
            InitializeComponent ();
        }

        protected override void OnNavigatedTo (System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo (e);

            if (e.NavigationMode == System.Windows.Navigation.NavigationMode.New) {
                var vm = new SpeakerListViewModel ();

                vm.BeginUpdate (Dispatcher);

                DataContext = vm;
            }
        }

        private void HandleSpeakerTap (object sender, System.Windows.Input.GestureEventArgs e)
        {
            var item = ((Grid)sender).DataContext as SpeakerListItemViewModel;

            if (item != null) {
                NavigationService.Navigate (new Uri ("/SpeakerDetails.xaml?id=" + item.ID, UriKind.RelativeOrAbsolute));
            }
        }
    }
}
