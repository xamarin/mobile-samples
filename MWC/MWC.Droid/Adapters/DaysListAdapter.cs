using System;
using System.Collections.Generic;
using Android.Widget;
using MWC.BL;
using Android.App;
using MWC;
using Android.Views;

namespace MWC.Adapters
{
    /// <remarks>
    /// See /AL/DaysTableSource.cs in iOS solution
    /// </remarks>
    public class DaysListAdapter: BaseAdapter<String>
    {
        protected Activity _context = null;
        protected IList<String> _days = new List<String>();

        public DaysListAdapter(Activity context)
            : base()
        {
            this._context = context;
            this._days = new List<String>() { "Monday", "Tuesday", "Wednesday", "Thursday" };
        }

        public override String this[int position]
        {
            get { return this._days[position]; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override int Count
        {
            get { return this._days.Count; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {

            // Get our object for this position
            var item = this._days[position];

            //Try to reuse convertView if it's not  null, otherwise inflate it from our item layout
            // This gives us some performance gains by not always inflating a new view
            // This will sound familiar to MonoTouch developers with UITableViewCell.DequeueReusableCell()
            var view = (convertView ??
                    this._context.LayoutInflater.Inflate(
                    Resource.Layout.HomeListItem,
                    parent,
                    false)) as LinearLayout;

            // Find references to each subview in the list item's view
            var _bigTextView = view.FindViewById<TextView>(Resource.Id.BigTextView);
            
            //Assign this item's values to the various subviews
            _bigTextView.SetText(this._days[position], TextView.BufferType.Normal);
            
            //Finally return the view
            return view;
        }
    }
}