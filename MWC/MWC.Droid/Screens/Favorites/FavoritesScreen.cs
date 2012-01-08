using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using MWC.BL;
using MWC;
using System;
using MWC.SAL;

namespace MWC.Android.Screens
{
    [Activity(Label = "Favorites")]
    public class FavoritesScreen : BaseScreen
    {
        protected MWC.Adapters.FavoritesListAdapter _favoritesList;
        protected IList<Favorite> _favorites;
        protected ListView _favoritesListView = null;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // set our layout to be the home screen
            this.SetContentView(Resource.Layout.SpeakersScreen);

            //Find our controls
            this._favoritesListView = FindViewById<ListView>(Resource.Id.SpeakerList);

            // wire up task click handler
            if (this._favoritesListView != null)
            {
                this._favoritesListView.ItemClick += (object sender, ItemEventArgs e) =>
                {
                    var sessionDetails = new Intent(this, typeof(SessionDetailsScreen));
                    sessionDetails.PutExtra("SessionID", this._favorites[e.Position].SessionID);
                    this.StartActivity(sessionDetails);
                };
            }
        }

        protected override void OnResume()
        {
            base.OnResume();

            this._favorites = MWC.BL.Managers.FavoritesManager.GetFavorites();

            // create our adapter
            this._favoritesList = new MWC.Adapters.FavoritesListAdapter(this, this._favorites);

            //Hook up our adapter to our ListView
            this._favoritesListView.Adapter = this._favoritesList;
        }
    }
}