using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Bluetooth;
using Java.Util;

namespace BluetoothLEExplorer.Droid.Screens.Scanner.DeviceDetails
{
	[Activity (Label = "Services")]			
	public class DeviceDetailsScreen : ListActivity
	{
		// members
		//protected IList<string> _services = new List<string>();
		protected IList<BluetoothGattService> _services = new List<BluetoothGattService>();

		// external handlers
		EventHandler<BluetoothLEManager.ServiceDiscoveredEventArgs> serviceDiscoveredHandler;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// set the activity title
			base.Title = App.Current.State.SelectedDevice.Name;

			// create our adapter
			this.ListAdapter = new ServicesAdapter (this, this._services);

			BluetoothLEManager.Current.ConnectedDevices [App.Current.State.SelectedDevice].DiscoverServices ();
		}

		protected override void OnResume ()
		{
			base.OnResume ();
			this.WireUpLocalHandlers ();
			this.WireUpExternalHandlers ();
		}

		protected override void OnPause ()
		{
			base.OnPause ();
			this.RemoveExternalHandlers ();
		}

		protected void WireUpLocalHandlers ()
		{
			this.ListView.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) => {
				// set the selection
				this.ListView.SetSelection(e.Position);

				// persist the selected service service
				App.Current.State.SelectedService = this._services[e.Position];

				// launch the service details screen
				StartActivity (typeof(ServiceDetails.ServiceDetailsScreen));
			};
		}

		protected void WireUpExternalHandlers ()
		{
			serviceDiscoveredHandler = (object sender, BluetoothLEManager.ServiceDiscoveredEventArgs e) => {
				if (BluetoothLEManager.Current.ConnectedDevices[App.Current.State.SelectedDevice] != null) {

					this._services = BluetoothLEManager.Current.Services[App.Current.State.SelectedDevice];

					//TODO: why doens't upddate work? is it because i'm replacing the reference?
					this.RunOnUiThread( () => {
						this.ListAdapter = new ServicesAdapter (this, this._services);
					});
				}
			};
			BluetoothLEManager.Current.ServiceDiscovered += serviceDiscoveredHandler;
		}

		protected void RemoveExternalHandlers()
		{
			BluetoothLEManager.Current.ServiceDiscovered -= serviceDiscoveredHandler;
		}

		public override void OnBackPressed ()
		{
			base.OnBackPressed ();

			// disconnect from device
			if (App.Current.State.SelectedDevice != null)
				BluetoothLEManager.Current.DisconnectDevice (App.Current.State.SelectedDevice);
		}

	}
}

