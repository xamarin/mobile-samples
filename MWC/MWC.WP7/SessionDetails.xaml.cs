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
using MWC.WP7.ViewModels;
using MWC.BL.Managers;
using MWC.BL;
using Microsoft.Phone.Shell;

namespace MWC.WP7
{
    public partial class SessionDetails : PhoneApplicationPage
    {
        public SessionDetails ()
        {
            InitializeComponent ();
        }

        protected override void OnNavigatedTo (System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo (e);

            if (e.NavigationMode == System.Windows.Navigation.NavigationMode.New) {
                var vm = new SessionDetailsViewModel ();
                var session = default (Session);

                if (NavigationContext.QueryString.ContainsKey ("id")) {
                    var id = int.Parse (NavigationContext.QueryString["id"]);
                    session = SessionManager.GetSession (id);                    
                }
                else if (NavigationContext.QueryString.ContainsKey ("key")) {
                    var key = NavigationContext.QueryString["key"];
                    session = SessionManager.GetSessionWithKey (key);
                }

                if (session != null) {
                    vm.Update (session);
                }

                UpdateFavoriteButtonIcon (vm.IsFavorite);

                DataContext = vm;
            }
        }

        void UpdateFavoriteButtonIcon (bool fav)
        {
            if (fav) {
                ((ApplicationBarIconButton)ApplicationBar.Buttons[0]).IconUri = new Uri ("/Images/appbar.favs.removefrom.rest.png", UriKind.RelativeOrAbsolute);
                ((ApplicationBarIconButton)ApplicationBar.Buttons[0]).Text = "remove fav";
            }
            else {
                ((ApplicationBarIconButton)ApplicationBar.Buttons[0]).IconUri = new Uri ("/Images/appbar.favs.addto.rest.png", UriKind.RelativeOrAbsolute);
                ((ApplicationBarIconButton)ApplicationBar.Buttons[0]).Text = "add fav";
            }
        }

        private void HandleFavoriteClick (object sender, EventArgs e)
        {
            var vm = (SessionDetailsViewModel)DataContext;

            if (FavoritesManager.IsFavorite (vm.Key)) {
                FavoritesManager.RemoveFavoriteSession (vm.Key);
                UpdateFavoriteButtonIcon (false);
            }
            else {
                FavoritesManager.AddFavoriteSession (new Favorite {
                    SessionKey = vm.Key,
                });
                UpdateFavoriteButtonIcon (true);
            }

            vm.UpdateIsFavorite ();
        }

        private void HandlePinClick (object sender, EventArgs e)
        {
            var vm = (SessionDetailsViewModel)DataContext;

            var uri = "/SessionDetails.xaml?key=" + vm.Key;

            var foundTile = ShellTile.ActiveTiles.FirstOrDefault (x => x.NavigationUri.ToString ().Contains (uri));

            if (foundTile != null) {
                foundTile.Delete ();
            }

            ShellTile.Create (new Uri (uri, UriKind.Relative), new StandardTileData {
                Title = vm.Title,
                BackContent = string.Format ("{0:ddd} {0:t} {1}", vm.Start, vm.Room),
                BackTitle = vm.Title,
                BackgroundImage = new Uri ("/Background.png", UriKind.RelativeOrAbsolute),
            });
        }
    }
}
