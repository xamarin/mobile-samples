using System;
using System.Collections.Generic;
using Android.App;
using Android.Views;
using Android.Widget;
using MWC;
using MWC.BL;


namespace MWC.Adapters
{
    public class ExhibitorListAdapter : BaseAdapter<Exhibitor>
    {
        protected Activity _context = null;
        protected IList<Exhibitor> _exhibitors = new List<Exhibitor>();

        public ExhibitorListAdapter(Activity context, IList<Exhibitor> exhibitors)
            : base()
        {
            this._context = context;
            this._exhibitors = exhibitors;
        }

        public override Exhibitor this[int position]
        {
            get { return this._exhibitors[position]; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override int Count
        {
            get { return this._exhibitors.Count; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {

            // Get our object for this position
            var item = this._exhibitors[position];

            //Try to reuse convertView if it's not  null, otherwise inflate it from our item layout
            // This gives us some performance gains by not always inflating a new view
            // This will sound familiar to MonoTouch developers with UITableViewCell.DequeueReusableCell()
            var view = (convertView ??
                    this._context.LayoutInflater.Inflate(
                    Resource.Layout.ExhibitorListItem,
                    parent,
                    false)) as LinearLayout;

            // Find references to each subview in the list item's view
            var nameTextView = view.FindViewById<TextView>(Resource.Id.NameTextView);
            var countryTextView = view.FindViewById<TextView>(Resource.Id.CountryTextView);
            var locationTextView = view.FindViewById<TextView>(Resource.Id.LocationTextView);

            //Assign this item's values to the various subviews
            nameTextView.SetText(this._exhibitors[position].Name, TextView.BufferType.Normal);
            countryTextView.SetText(this._exhibitors[position].City + ", " + this._exhibitors[position].Country, TextView.BufferType.Normal);
            locationTextView.SetText(this._exhibitors[position].Locations, TextView.BufferType.Normal);
            //Finally return the view
            return view;
        }
    }
}

