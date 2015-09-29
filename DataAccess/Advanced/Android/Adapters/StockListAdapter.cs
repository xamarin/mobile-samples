using System;
using System.Collections.Generic;
using Android.Widget;
using Android.App;
using Android;

namespace DataAccess {
	public class StockListAdapter : BaseAdapter<Stock> {
		Activity context = null;
		IList<Stock> stocks = new List<Stock>();
		
		public StockListAdapter (Activity context, IList<Stock> stocks) : base ()
		{
			this.context = context;
			this.stocks = stocks;
		}
		
		public override Stock this[int position]
		{
			get { return stocks[position]; }
		}
		
		public override long GetItemId (int position)
		{
			return position;
		}
		
		public override int Count
		{
			get { return stocks.Count; }
		}
		
		public override Android.Views.View GetView (int position, Android.Views.View convertView, Android.Views.ViewGroup parent)
		{
			// Get our object for position
			var item = stocks[position];			

			//Try to reuse convertView if it's not  null, otherwise inflate it from our item layout
			// gives us some performance gains by not always inflating a new view
			// will sound familiar to MonoTouch developers with UITableViewCell.DequeueReusableCell()
			var view = (convertView ?? 
					context.LayoutInflater.Inflate(
					Android.Resource.Layout.SimpleListItem1,
					parent, 
					false)) as TextView;

			view.SetText (item.Name==""?"<new Stock>":item.Name, TextView.BufferType.Normal);

			//Finally return the view
			return view;
		}
	}
}