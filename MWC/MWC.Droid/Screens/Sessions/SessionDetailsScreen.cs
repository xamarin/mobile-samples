using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using MWC.BL;
using MWC;
using System;

namespace MWC.Android.Screens
{
    [Activity(Label = "Session")]
    public class SessionDetailsScreen : BaseScreen
    {
        Session _session;
        bool _isFavorite = false;
        Button _favouriteButton;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.SessionDetailsScreen);

            _favouriteButton = FindViewById<Button>(Resource.Id.FavouriteButton);
            _favouriteButton.Click += new EventHandler(_favouriteButton_Click);

            var id = Intent.GetIntExtra("SessionID", -1);

            if (id >= 0)
            {
                _session = BL.Managers.SessionManager.GetSession(id);
                if (_session != null)
                {
                    FindViewById<TextView>(Resource.Id.TitleTextView).Text = _session.Title;
                    FindViewById<TextView>(Resource.Id.SpeakersTextView).Text = _session.Speakers;
                    FindViewById<TextView>(Resource.Id.DateTimeTextView).Text = _session.Start.ToString("dddd H:mm")
                                                                    + " - " + _session.End.ToString("H:mm");

                    if (_session.Room != "")
                        FindViewById<TextView>(Resource.Id.RoomTextView).Text = _session.Room;
                    
                    FindViewById<TextView>(Resource.Id.OverviewTextView).Text = _session.Overview;

                    _isFavorite = BL.Managers.FavoritesManager.IsFavorite(_session.Title);

                    if (_isFavorite) _favouriteButton.Text = "Un favorite";
                    else _favouriteButton.Text = "Add favorite";
                }
                else
                {   // shouldn't happen...
                    FindViewById<TextView>(Resource.Id.TitleTextView).Text = "Session not found: " + id;
                }
            }
        }


        void _favouriteButton_Click(object sender, EventArgs e)
        {
            _isFavorite = !_isFavorite;

            if (_isFavorite)
            {
                _favouriteButton.Text = "Un favorite";
                var fav = new Favorite { SessionID = _session.ID, SessionName = _session.Title };
                BL.Managers.FavoritesManager.AddFavoriteSession(fav);
            }
            else
            {
                _favouriteButton.Text = "Add favorite";
                BL.Managers.FavoritesManager.RemoveFavoriteSession(_session.Title);
            }   
        }
    }
}