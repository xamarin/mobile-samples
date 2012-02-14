using System;
using System.Collections.Generic;
using Android.Widget;
using MWC.BL;
using Android.App;
using MWC;
using Android.Views;
using MWC.SAL;
using Android.Util;

namespace MWC.Adapters {
    public class TwitterListAdapter : BaseAdapter<Tweet> {
        protected Activity context = null;
        protected IList<Tweet> tweets = new List<Tweet>();

        public TwitterListAdapter(Activity context, IList<Tweet> tweets)
            : base()
        {
            this.context = context;
            this.tweets = tweets;
        }

        public override Tweet this[int position]
        {
            get { return tweets[position]; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override int Count
        {
            get { return tweets.Count; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            // Get our object for this position
            var item = this.tweets[position];

            //Try to reuse convertView if it's not  null, otherwise inflate it from our item layout
            // This gives us some performance gains by not always inflating a new view
            // This will sound familiar to MonoTouch developers with UITableViewCell.DequeueReusableCell()
            var view = (convertView ??
                    this.context.LayoutInflater.Inflate(
                    Resource.Layout.TwitterListItem,
                    parent,
                    false)) as LinearLayout;

            // Find references to each subview in the list item's view
            var nametextview = view.FindViewById<TextView>(Resource.Id.NameTextView);
            var timetextview = view.FindViewById<TextView>(Resource.Id.TimeTextView);
            var handletextview = view.FindViewById<TextView>(Resource.Id.HandleTextView);
            var contenttextview = view.FindViewById<TextView>(Resource.Id.ContentTextView);
            var imageview = view.FindViewById<ImageView>(Resource.Id.TwitterImageView);

            //assign this item's values to the various subviews
            nametextview.SetText(this.tweets[position].RealName, TextView.BufferType.Normal);
            timetextview.SetText(this.tweets[position].FormattedTime, TextView.BufferType.Normal);
            handletextview.SetText(this.tweets[position].FormattedAuthor, TextView.BufferType.Normal);
            contenttextview.SetText(this.tweets[position].Title, TextView.BufferType.Normal);

            var uri = new Uri(this.tweets[position].ImageUrl);
            var iw = new AL.ImageWrapper(imageview, context);
            imageview.Tag = uri.ToString();
            try {
                var drawable = MonoTouch.Dialog.Utilities.ImageLoader.DefaultRequestImage(uri, iw);
                if (drawable != null)
                    imageview.SetImageDrawable(drawable);
            } catch (Exception ex) {
                Log.Debug("TWITTER", ex.ToString());
            }

            //Finally return the view
            return view;
        }
    }
}

