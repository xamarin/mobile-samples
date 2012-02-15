using System;
using System.Collections.Generic;
using Android.App;
using Android.Views;
using Android.Widget;

namespace MWC.Adapters {
    /// <remarks>
    /// See /AL/DaysTableSource.cs in iOS solution
    /// </remarks>
    public class DaysListAdapter: BaseAdapter<String> {
        protected Activity context = null;
        protected IList<DateTime> days;

        public DaysListAdapter(Activity context)
            : base()
        {
            this.context = context;
            days = DaysManager.GetDays();
        }

        public override String this[int position]
        {
            get { return days[position].ToString("dddd"); }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override int Count
        {
            get { return days.Count; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {

            // Get our object for this position
            var item = days[position];

            //Try to reuse convertView if it's not  null, otherwise inflate it from our item layout
            // This gives us some performance gains by not always inflating a new view
            // This will sound familiar to MonoTouch developers with UITableViewCell.DequeueReusableCell()
            var view = (convertView ??
                    context.LayoutInflater.Inflate(
                    Resource.Layout.HomeListItem,
                    parent,
                    false)) as RelativeLayout;

            // Find references to each subview in the list item's view
            var dayNameTextView = view.FindViewById<TextView>(Resource.Id.BigTextView);
            var monthTextView = view.FindViewById<TextView>(Resource.Id.MonthTextView);
            var dayTextView = view.FindViewById<TextView>(Resource.Id.DayTextView);
            
            //Assign this item's values to the various subviews
            dayNameTextView.SetText(days[position].ToString("dddd"), TextView.BufferType.Normal);
            monthTextView.SetText(days[position].ToString("MMM").ToUpper(), TextView.BufferType.Normal);
            dayTextView.SetText(days[position].ToString("dd"), TextView.BufferType.Normal);
            
            //Finally return the view
            return view;
        }
    }
}