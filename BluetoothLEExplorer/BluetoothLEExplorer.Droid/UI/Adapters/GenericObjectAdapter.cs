using System;
using System.Collections.Generic;
using Android.App;
using Android.Views;
using Android.Widget;

namespace BluetoothLEExplorer.Droid
{

	/// <summary>
	/// Generic object adapter.
	/// 
	/// TODO: proper selection stuff. see answer #5 here: http://stackoverflow.com/questions/1446373/android-listview-setselection-does-not-seem-to-work
	/// </summary>
	public class GenericObjectAdapter<T> : GenericAdapterBase<T>
	{

		public GenericObjectAdapter (Activity context, int resource, IList<T> items) : 
			base(context, resource, items)
		{
		}

		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			View view = convertView; // re-use an existing view, if one is available
			if (view == null) // otherwise create a new one
				view = context.LayoutInflater.Inflate (resource, null);
			view.FindViewById<TextView>(Android.Resource.Id.Text1).Text = items[position].ToString();
			return view;
		}

	}
}

