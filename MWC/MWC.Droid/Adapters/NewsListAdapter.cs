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
    public class NewsListAdapter : BaseAdapter<RSSEntry>
    {
        protected Activity _context = null;
        protected IList<RSSEntry> _news = new List<RSSEntry>();

        public NewsListAdapter(Activity context, IList<RSSEntry> news)
            : base()
        {
            this._context = context;
            this._news = news;
        }

        public override RSSEntry this[int position]
        {
            get { return this._news[position]; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override int Count
        {
            get { return this._news.Count; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {

            // Get our object for this position
            var item = this._news[position];

            //Try to reuse convertView if it's not  null, otherwise inflate it from our item layout
            // This gives us some performance gains by not always inflating a new view
            // This will sound familiar to MonoTouch developers with UITableViewCell.DequeueReusableCell()
            var view = (convertView ??
                    this._context.LayoutInflater.Inflate(
                    Resource.Layout.NewsListItem,
                    parent,
                    false)) as LinearLayout;

            // Find references to each subview in the list item's view
            var _bigTextView = view.FindViewById<TextView>(Resource.Id.BigTextView);
            var _smallTextView = view.FindViewById<TextView>(Resource.Id.SmallTextView);

            //Assign this item's values to the various subviews
            _bigTextView.SetText(this._news[position].Title, TextView.BufferType.Normal);
            _smallTextView.SetText(this._news[position].Published.ToString("d MMM yy"), TextView.BufferType.Normal);

            //Finally return the view
            return view;
        }
    }
}

