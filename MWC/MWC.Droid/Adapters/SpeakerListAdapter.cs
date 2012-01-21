using System;
using System.Collections.Generic;
using Android.Widget;
using MWC.BL;
using Android.App;
using MWC;
using Android.Views;


namespace MWC.Adapters
{
    public class SpeakerListAdapter : BaseAdapter<Speaker>
    {
        protected Activity _context = null;
        protected IList<Speaker> _speakers = new List<Speaker>();

        public SpeakerListAdapter(Activity context, IList<Speaker> speakers)
            : base()
        {
            this._context = context;
            this._speakers = speakers;
        }

        public override Speaker this[int position]
        {
            get { return this._speakers[position]; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override int Count
        {
            get { return this._speakers.Count; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {

            // Get our object for this position
            var item = this._speakers[position];

            //Try to reuse convertView if it's not  null, otherwise inflate it from our item layout
            // This gives us some performance gains by not always inflating a new view
            // This will sound familiar to MonoTouch developers with UITableViewCell.DequeueReusableCell()
            var view = (convertView ??
                    this._context.LayoutInflater.Inflate(
                    Resource.Layout.SpeakerListItem,
                    parent,
                    false)) as LinearLayout;

            // Find references to each subview in the list item's view
            var _bigTextView = view.FindViewById<TextView>(Resource.Id.NameTextView);
            var _smallTextView = view.FindViewById<TextView>(Resource.Id.CompanyTextView);

            //Assign this item's values to the various subviews
            _bigTextView.SetText(this._speakers[position].Name, TextView.BufferType.Normal);
            _smallTextView.SetText(this._speakers[position].Title+", "+this._speakers[position].Company, TextView.BufferType.Normal);

            //Finally return the view
            return view;
        }
    }
}

