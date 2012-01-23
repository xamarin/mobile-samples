using System;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using MWC.BL.Managers;
using MWC.WP7.ViewModels;

namespace MWC.WP7
{
    public partial class ExhibitorDetails : PhoneApplicationPage
    {
        public ExhibitorDetails ()
        {
            InitializeComponent ();
        }

        protected override void OnNavigatedTo (System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo (e);

            if (e.NavigationMode == System.Windows.Navigation.NavigationMode.New) {
                var vm = new ExhibitorDetailsViewModel ();

                if (NavigationContext.QueryString.ContainsKey ("id")) {
                    var id = int.Parse (NavigationContext.QueryString["id"]);
                    var exhibitor = ExhibitorManager.GetExhibitor (id);
                    if (exhibitor != null) {
                        vm.Update (exhibitor);
                    }
                }

                if (string.IsNullOrWhiteSpace (vm.Email)) {
                    ApplicationBar.Buttons.RemoveAt (0);
                    ApplicationBar.IsVisible = false;
                }

                DataContext = vm;
            }
        }

        private void HandleEmail (object sender, EventArgs e)
        {
            var vm = (ExhibitorDetailsViewModel)DataContext;

            if (!string.IsNullOrWhiteSpace (vm.Email)) {
                var task = new EmailComposeTask {
                    To = vm.Email,
                    Subject = "Mobile World Congress",
                };
                task.Show ();
            }
        }

        private void HandleMap (object sender, EventArgs e)
        {
            var vm = (ExhibitorDetailsViewModel)DataContext;

            if (!string.IsNullOrWhiteSpace (vm.Address)) {
                var task = new BingMapsTask {
                    SearchTerm = vm.Address,
                };
                task.Show ();
            }
        }
    }
}
