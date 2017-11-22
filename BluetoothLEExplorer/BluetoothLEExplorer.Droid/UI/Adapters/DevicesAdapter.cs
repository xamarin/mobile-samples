using System;
using System.Collections.Generic;
using Android.App;
using Android.Bluetooth;
using Android.Views;
using Android.Widget;

namespace BluetoothLEExplorer.Droid.UI.Adapters
{
	public class DevicesAdapter : GenericAdapterBase<BluetoothDevice>
	{
		public DevicesAdapter (Activity context, IList<BluetoothDevice> items) 
			: base(context, Android.Resource.Layout.SimpleListItem2, items)
		{

		}

		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			View view = convertView; // re-use an existing view, if one is available
			if (view == null) // otherwise create a new one
				view = context.LayoutInflater.Inflate (resource, null);
			view.FindViewById<TextView> (Android.Resource.Id.Text1).Text = items [position].Name;
			view.FindViewById<TextView> (Android.Resource.Id.Text2).Text = "Address: " + items [position].Address;
			return view;
		}
	}
}

