using System;
using Microsoft.Phone.Controls;
using MWC.BL.Managers;
using MWC.WP7.ViewModels;

namespace MWC.WP7
{
    public partial class SpeakerDetails : PhoneApplicationPage
    {
        public SpeakerDetails ()
        {
            InitializeComponent ();
        }

        protected override void OnNavigatedTo (System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo (e);

            if (e.NavigationMode == System.Windows.Navigation.NavigationMode.New) {
                var vm = new SpeakerDetailsViewModel ();

                if (NavigationContext.QueryString.ContainsKey ("id")) {
                    var id = int.Parse (NavigationContext.QueryString["id"]);
                    var speaker = SpeakerManager.GetSpeaker (id);
                    if (speaker != null) {
                        vm.Update (speaker);
                    }
                }

                DataContext = vm;
            }
        }
    }
}
