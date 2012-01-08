using System;
using System.Collections.Generic;
using Android.Widget;
using MWC.BL;
using Android.App;
using MWC;
using Android.Views;
using MWC.SAL;


namespace MWC.Adapters
{
    public class TwitterListAdapter : BaseAdapter<Tweet>
    {
        protected Activity _context = null;
        protected IList<Tweet> _tweets = new List<Tweet>();

        public TwitterListAdapter(Activity context, IList<Tweet> tweets)
            : base()
        {
            this._context = context;
            this._tweets = tweets;
        }

        public override Tweet this[int position]
        {
            get { return this._tweets[position]; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override int Count
        {
            get { return this._tweets.Count; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {

            // Get our object for this position
            var item = this._tweets[position];

            //Try to reuse convertView if it's not  null, otherwise inflate it from our item layout
            // This gives us some performance gains by not always inflating a new view
            // This will sound familiar to MonoTouch developers with UITableViewCell.DequeueReusableCell()
            var view = (convertView ??
                    this._context.LayoutInflater.Inflate(
                    Resource.Layout.TwitterListItem,
                    parent,
                    false)) as LinearLayout;

            // Find references to each subview in the list item's view
            var _bigTextView = view.FindViewById<TextView>(Resource.Id.BigTextView);
            var _smallTextView = view.FindViewById<TextView>(Resource.Id.SmallTextView);

            //Assign this item's values to the various subviews
            _bigTextView.SetText(this._tweets[position].FormattedAuthor + " " + this._tweets[position].FormattedTime, TextView.BufferType.Normal);
            _smallTextView.SetText(this._tweets[position].Title, TextView.BufferType.Normal);

            //Finally return the view
            return view;
        }
    }
}

