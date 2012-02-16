using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Util;
using Android.Widget;
using MWC.BL;

namespace MWC.Android.Screens
{
    [Activity(Label = "Favorites", ScreenOrientation = ScreenOrientation.Portrait)]
    public class FavoritesScreen : BaseScreen
    {
        protected MWC.Adapters.FavoritesListAdapter _favoritesListAdapter;
        protected IList<Favorite> _favorites;
        protected IList<Session> _sessions;
        protected ListView _favoritesListView = null;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // set our layout to be the home screen
            this.SetContentView(Resource.Layout.FavoritesScreen);

            //Find our controls
            this._favoritesListView = FindViewById<ListView>(Resource.Id.FavoritesList);

            // wire up task click handler
            if (this._favoritesListView != null)
            {
                this._favoritesListView.ItemClick += (object sender, ItemEventArgs e) =>
                {
                    var sessionDetails = new Intent(this, typeof(SessionDetailsScreen));
                    sessionDetails.PutExtra("SessionID", this._favoritesListAdapter[e.Position].ID);
                    this.StartActivity(sessionDetails);
                };
            }
        }

        protected override void OnResume()
        {
            base.OnResume();

            this._favorites = MWC.BL.Managers.FavoritesManager.GetFavorites();

            if (this._favorites.Count > 0)
            {
                if (this._sessions == null || this._sessions.Count == 0)
                {   // don't re-get these
                    this._sessions = MWC.BL.Managers.SessionManager.GetSessions();
                }

                // create our adapter
                this._favoritesListAdapter = new MWC.Adapters.FavoritesListAdapter(this, this._favorites, this._sessions);

                //Hook up our adapter to our ListView
                this._favoritesListView.Adapter = this._favoritesListAdapter;
            }
            else
            {
                Log.Debug("MWC", "FAVORITES Clear out favorites rows");
            }
        }
    }
}