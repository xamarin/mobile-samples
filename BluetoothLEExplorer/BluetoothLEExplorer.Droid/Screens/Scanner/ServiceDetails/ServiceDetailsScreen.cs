using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Bluetooth;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Util;

namespace BluetoothLEExplorer.Droid.Screens.Scanner.ServiceDetails
{
	[Activity (Label = "Characteristics")]			
	public class ServiceDetailsScreen : ListActivity
	{
		protected List<string> _characteristics;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);


			// create our adapter
			this.ListAdapter = new GenericObjectAdapter<BluetoothGattCharacteristic> (this, Android.Resource.Layout.SimpleListItem1, App.Current.State.SelectedService.Characteristics);

		}
	}
}

