using System;
using System.Linq;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using MWC.BL;
using MWC.BL.Managers;
using MWC.WP7.ViewModels;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.IO.IsolatedStorage;

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
                var speaker = default (Speaker);

                if (NavigationContext.QueryString.ContainsKey ("id")) {
                    var id = int.Parse (NavigationContext.QueryString["id"]);
                    speaker = SpeakerManager.GetSpeaker (id);                    
                }
                else if (NavigationContext.QueryString.ContainsKey ("key")) {
                    var key = NavigationContext.QueryString["key"];
                    speaker = SpeakerManager.GetSpeakerWithKey (key);
                }

                if (speaker != null) {
                    vm.Update (speaker);
                }

                DataContext = vm;
            }
        }

        Uri SaveImageAsTile (Image image, string key)
        {
            WriteableBitmap bmp = new WriteableBitmap ((BitmapSource)image.Source);

            var iso = IsolatedStorageFile.GetUserStoreForApplication ();
            if (!iso.DirectoryExists ("Shared")) {
                iso.CreateDirectory ("Shared");
            }

            var dir = "Shared\\ShellContent";
            if (!iso.DirectoryExists (dir)) {
                iso.CreateDirectory (dir);
            }
            
            var path = dir + "\\" + key + ".jpg";

            using (var stream = iso.CreateFile (path)) {
                bmp.SaveJpeg (stream, 173, 173, 0, 100);
            }

            return new Uri ("isostore:/Shared/ShellContent/" + key + ".jpg", UriKind.RelativeOrAbsolute);
        }

        private void HandlePinClick (object sender, EventArgs e)
        {
            var vm = (SpeakerDetailsViewModel)DataContext;

            var uri = "/SpeakerDetails.xaml?key=" + vm.Key;

            var imageUri = default (Uri);
            try {
                imageUri = SaveImageAsTile (SpeakerImage, "Speaker-" + vm.ID);
            }
            catch (Exception) {
            }            

            var foundTile = ShellTile.ActiveTiles.FirstOrDefault (x => x.NavigationUri.ToString ().Contains (uri));

            if (foundTile != null) {
                foundTile.Delete ();
            }

            var tile = new StandardTileData {
                Title = vm.Name,
                BackContent = string.Format ("{0} at {1}", vm.Title, vm.Company),
                BackTitle = vm.Name,
                BackgroundImage = (imageUri != null) ? imageUri : new Uri ("/Background.png", UriKind.RelativeOrAbsolute),
            };

            ShellTile.Create (new Uri (uri, UriKind.Relative), tile);
        }
    }
}
