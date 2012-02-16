using System;
using System.Collections.Generic;
using Android.Widget;
using MWC.BL;
using Android.App;
using MWC;
using Android.Views;
using System.Linq;

namespace MWC.Adapters {
    /// <remarks>
    /// <seealso cref="MWC.Adapters.SessionTimeslotListAdapter"/> and the iOS FavoritesScreen
    /// </remarks>
    public class FavoritesListAdapter : BaseAdapter<Session> {
        protected Activity context = null;
        
        private readonly IList<object> rows;

        public FavoritesListAdapter(Activity context, IList<Favorite> favorites, IList<Session> allSessions)
            : base()
        {
            this.context = context;
           
            List<string> favoriteIDs = new List<string>();
			foreach (var f in favorites) favoriteIDs.Add (f.SessionKey);

            var timeslots = from s in allSessions
							where favoriteIDs.Contains(s.Key)
							group s by s.Start.Ticks into g
							orderby g.Key
							select new SessionTimeslot
                                (new DateTime (g.Key).ToString ("dddd HH:mm")
                                   , from hs in g select hs);

            rows = new List<object>();
            // flatten groups into single 'list'
            foreach (var time in timeslots) {
                rows.Add(time.Timeslot);
                foreach (var session in time.Sessions) {
                    rows.Add(session);
                }
            }
        }

        public override Session this[int position]{
            get { // this'll break if called with a 'header' position
                return (Session)this.rows[position];
            }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override int Count
        {
            get { return this.rows.Count; }
        }

        /// <summary>
        /// Grouped list: view could be a 'section heading' or a 'data row'
        /// </summary>
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            // Get our object for this position
            var item = this.rows[position];
            View view = null;

            if (item is string) {   // header
                view = context.LayoutInflater.Inflate(Resource.Layout.SessionTimeslotListItem, null);
                view.Clickable = false;
                view.LongClickable = false;
                view.SetOnClickListener(null);

                view.FindViewById<TextView>(Resource.Id.TitleTextView).Text = (string)item;
            } else {   //session
                view = context.LayoutInflater.Inflate(Resource.Layout.SessionListItem, null);

                // Find references to each subview in the list item's view
                var titleTextView = view.FindViewById<TextView>(Resource.Id.TitleTextView);
                var roomTextView = view.FindViewById<TextView>(Resource.Id.RoomTextView);
                var favoriteImageView = view.FindViewById<ImageView>(Resource.Id.FavoriteImageView);
                //var favoriteButton = view.FindViewById<Button>(Resource.Id.FavoriteButton);

                var session = (Session)item;
                var isFavorite = BL.Managers.FavoritesManager.IsFavorite(session.Key);
                //Assign this item's values to the various subviews
                titleTextView.SetText(session.Title, TextView.BufferType.Normal);
                roomTextView.SetText(session.Room, TextView.BufferType.Normal);
                if (isFavorite)
                    favoriteImageView.SetImageResource(Resource.Drawable.star_gold);
                    //favoriteButton.SetBackgroundResource(Resource.Drawable.star_gold_selector);
                else
                    favoriteImageView.SetImageResource(Resource.Drawable.star_grey);
                    //favoriteButton.SetBackgroundResource(Resource.Drawable.star_grey_selector);
            }
            //Finally return the view
            return view;
        }
    }
}

