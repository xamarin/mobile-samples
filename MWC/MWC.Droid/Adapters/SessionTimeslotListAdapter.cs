using System.Collections.Generic;
using Android.App;
using Android.Views;
using Android.Widget;
using MWC.BL;


namespace MWC.Adapters {
    public class SessionTimeslotListAdapter : BaseAdapter<Session> {
        protected Activity context = null;
        private readonly IList<object> rows;
        
        public SessionTimeslotListAdapter(Activity context, IList<SessionTimeslot> timeslots)
            : base()
        {
            this.context = context;
            rows = new List<object>();
            // flatten groups into single 'list'
            foreach (var time in timeslots) {
                rows.Add(time.Timeslot);
                foreach (var session in time.Sessions) {
                    rows.Add(session);
                }
            }
        }

        public override Session this[int position] {
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

                var session = (Session)item;
                var isFavorite = BL.Managers.FavoritesManager.IsFavorite(session.Key);
                //Assign this item's values to the various subviews
                titleTextView.SetText(session.Title, TextView.BufferType.Normal);
                roomTextView.SetText(session.Room, TextView.BufferType.Normal);
                if (isFavorite)
                    favoriteImageView.SetImageResource(Resource.Drawable.favorited);
                else
                    favoriteImageView.SetImageResource(Resource.Drawable.favorite);
            }
            //Finally return the view
            return view;
        }
    }
}