using System;
using System.Linq;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using MWC.BL;
using MWC.BL.Managers;
using MWC.WP7.Utilities;
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
                var exhibitor = default (Exhibitor);

                if (NavigationContext.QueryString.ContainsKey ("id")) {
                    var id = int.Parse (NavigationContext.QueryString["id"]);
                    exhibitor = ExhibitorManager.GetExhibitor (id);                    
                }
                else if (NavigationContext.QueryString.ContainsKey ("name")) {
                    var name = NavigationContext.QueryString["name"];
                    exhibitor = ExhibitorManager.GetExhibitorWithName (name);
                }

                if (exhibitor != null) {
                    vm.Update (exhibitor);
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

        private void HandlePinClick (object sender, EventArgs e)
        {
            var vm = (ExhibitorDetailsViewModel)DataContext;

            var uri = "/ExhibitorDetails.xaml?name=" + Uri.EscapeDataString (vm.Name);

            var imageUri = default (Uri);
            try {
                imageUri = ExhibitorImage.SaveAsTile ("Exhibitor-" + vm.ID);
            }
            catch (Exception) {
            }

            var foundTile = ShellTile.ActiveTiles.FirstOrDefault (x => x.NavigationUri.ToString ().Contains (uri));

            if (foundTile != null) {
                foundTile.Delete ();
            }

            //
            // Workaround bug in WP7 that interprets @ in a very odd way
            //
            var title = vm.Name;
            if (title.StartsWith ("@")) {
                title = title.Replace ("@", "at");
            }

            var tile = new StandardTileData {
                Title = title,
                BackContent = vm.Locations,
                BackTitle = title,
                BackgroundImage = (imageUri != null) ? imageUri : new Uri ("/Background.png", UriKind.RelativeOrAbsolute),
            };

            ShellTile.Create (new Uri (uri, UriKind.Relative), tile);
        }
    }
}
