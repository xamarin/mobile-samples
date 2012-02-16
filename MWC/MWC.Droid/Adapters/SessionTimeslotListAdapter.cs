using System;
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
        public override bool AreAllItemsEnabled()
        {
            return true;
        }
        public override bool IsEnabled(int position)
        {
            return !(rows[position] is string);
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
                var roomPanel = view.FindViewById<LinearLayout>(Resource.Id.RoomPanel);

                var session = (Session)item;
                var isFavorite = BL.Managers.FavoritesManager.IsFavorite(session.Key);
                //Assign this item's values to the various subviews
                titleTextView.SetText(session.Title, TextView.BufferType.Normal);
                if (String.IsNullOrEmpty(session.Room)) {
                    roomPanel.Visibility = ViewStates.Gone;
                    roomTextView.SetText("", TextView.BufferType.Normal);
                } else {
                    roomPanel.Visibility = ViewStates.Visible;
                    roomTextView.SetText(session.Room, TextView.BufferType.Normal);
                }
                
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