using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using MWC.BL;

namespace MWC.Android.Screens {
    [Activity(Label = "Session")]
    public class SessionDetailsScreen : BaseScreen {
        Session session;
        bool isFavorite = false;
        Button favouriteButton;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.SessionDetailsScreen);

            favouriteButton = FindViewById<Button>(Resource.Id.FavouriteButton);
            favouriteButton.Click += new EventHandler(_favouriteButton_Click);

            var id = Intent.GetIntExtra("SessionID", -1);

            if (id >= 0) {
                session = BL.Managers.SessionManager.GetSession(id);
                if (session != null) {
                    FindViewById<TextView>(Resource.Id.TitleTextView).Text = session.Title;
                    FindViewById<TextView>(Resource.Id.SpeakersTextView).Text = session.SpeakerNames;
                    FindViewById<TextView>(Resource.Id.DateTimeTextView).Text = session.Start.ToString("dddd H:mm")
                                                                    + " - " + session.End.ToString("H:mm");

                    if (session.Room != "")
                        FindViewById<TextView>(Resource.Id.RoomTextView).Text = session.Room;
                    
                    FindViewById<TextView>(Resource.Id.OverviewTextView).Text = session.Overview;

                    isFavorite = BL.Managers.FavoritesManager.IsFavorite(session.Key);

                    if (isFavorite) { 
                        favouriteButton.Text = "Un favorite";
                        favouriteButton.SetCompoundDrawablesWithIntrinsicBounds(Resources.GetDrawable(Resource.Drawable.favorited), null, null, null);
                    } else { 
                        favouriteButton.Text = "Add favorite";
                        favouriteButton.SetCompoundDrawablesWithIntrinsicBounds(Resources.GetDrawable(Resource.Drawable.favorite), null, null, null);
                    }
                } else {   // shouldn't happen...
                    FindViewById<TextView>(Resource.Id.TitleTextView).Text = "Session not found: " + id;
                }
            }
        }


        void _favouriteButton_Click(object sender, EventArgs e)
        {
            isFavorite = !isFavorite;

            if (isFavorite) {
                favouriteButton.Text = "Un favorite";
                favouriteButton.SetCompoundDrawablesWithIntrinsicBounds(Resources.GetDrawable(Resource.Drawable.favorited), null, null, null);
                var fav = new Favorite { SessionID = session.ID, SessionKey = session.Key };
                BL.Managers.FavoritesManager.AddFavoriteSession(fav);
            } else {
                favouriteButton.Text = "Add favorite";
                favouriteButton.SetCompoundDrawablesWithIntrinsicBounds(Resources.GetDrawable(Resource.Drawable.favorite), null, null, null);
                BL.Managers.FavoritesManager.RemoveFavoriteSession(session.Key);
            }   
        }
    }
}