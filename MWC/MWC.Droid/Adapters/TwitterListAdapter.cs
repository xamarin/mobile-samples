using System;
using System.Collections.Generic;
using Android.Widget;
using MWC.BL;
using Android.App;
using MWC;
using Android.Views;
using MWC.SAL;
using Android.Util;


namespace MWC.Adapters
{                                                         //HACK: this is a bad spot to implement, just playing with it
    public class TwitterListAdapter : BaseAdapter<Tweet>, MonoTouch.Dialog.Utilities.IImageUpdated
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
            var nametextview = view.FindViewById<TextView>(Resource.Id.NameTextView);
            var timetextview = view.FindViewById<TextView>(Resource.Id.TimeTextView);
            var handletextview = view.FindViewById<TextView>(Resource.Id.HandleTextView);
            var contenttextview = view.FindViewById<TextView>(Resource.Id.ContentTextView);
            imageview = view.FindViewById<ImageView>(Resource.Id.TwitterImageView);

            //assign this item's values to the various subviews
            nametextview.SetText(this._tweets[position].RealName, TextView.BufferType.Normal);
            timetextview.SetText(this._tweets[position].FormattedTime, TextView.BufferType.Normal);
            handletextview.SetText(this._tweets[position].FormattedAuthor, TextView.BufferType.Normal);
            contenttextview.SetText(this._tweets[position].Title, TextView.BufferType.Normal);

            //HACK: of course this is a bad place to implement a callback, since an Adapter isn't the
            // same as a 'cell' in iOS; but it proves that the ImageLoader code works (even if the wrong
            // images appear as you scroll). Needs refactoring!!
            var uri = new Uri(this._tweets[position].ImageUrl);
            try
            {
                var drawable = MonoTouch.Dialog.Utilities.ImageLoader.DefaultRequestImage(uri, this);
                if (drawable != null)
                    imageview.SetImageDrawable(drawable);
            }
            catch (Exception ex)
            {
                Log.Debug("TWITTER", ex.ToString());
            }

            
            //Finally return the view
            return view;
        }
        ImageView imageview;
        public void UpdatedImage(Uri uri)
        {
            var drawable = MonoTouch.Dialog.Utilities.ImageLoader.DefaultRequestImage(uri, this);
            imageview.SetImageDrawable(drawable);
        }
    }
}

