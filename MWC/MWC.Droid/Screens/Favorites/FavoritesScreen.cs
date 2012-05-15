using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Util;
using Android.Widget;
using MWC.BL;

namespace MWC.Android.Screens {
    [Activity(Label = "Favorites", ScreenOrientation = ScreenOrientation.Portrait)]
    public class FavoritesScreen : BaseScreen {
        protected MWC.Adapters.FavoritesListAdapter favoritesListAdapter;
        protected IList<Favorite> favorites;
        protected IList<Session> sessions;
        protected ListView favoritesListView = null;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // set our layout to be the home screen
            this.SetContentView(Resource.Layout.FavoritesScreen);

            //Find our controls
            favoritesListView = FindViewById<ListView>(Resource.Id.FavoritesList);

            // wire up task click handler
            if (favoritesListView != null) {
                favoritesListView.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) => {
                    var sessionDetails = new Intent(this, typeof(SessionDetailsScreen));
                    sessionDetails.PutExtra("SessionID", favoritesListAdapter[e.Position].ID);
                    StartActivity(sessionDetails);
                };
            }
        }
        // scroll back to the point where you last were in the list
        int lastScrollY = -1;
        protected override void OnPause()
        {
            base.OnPause();
            if (favoritesListView.FirstVisiblePosition < 5)
                lastScrollY = 0;
            else
                lastScrollY = (favoritesListView.FirstVisiblePosition + favoritesListView.LastVisiblePosition) / 2;
        }

        protected override void OnResume()
        {
            base.OnResume();

            favorites = MWC.BL.Managers.FavoritesManager.GetFavorites();

            if (favorites.Count >= 0) {
                if (sessions == null || this.sessions.Count == 0) {   // don't re-get these
                    sessions = MWC.BL.Managers.SessionManager.GetSessions();
                }

                // create our adapter
                favoritesListAdapter = new MWC.Adapters.FavoritesListAdapter(this, favorites, sessions);

                //Hook up our adapter to our ListView
                favoritesListView.Adapter = favoritesListAdapter;
            } else {
                Log.Debug("MWC", "FAVORITES Clear out favorites rows");
            }

            favoritesListView.SetSelectionFromTop(lastScrollY, 200);
        }
    }
}