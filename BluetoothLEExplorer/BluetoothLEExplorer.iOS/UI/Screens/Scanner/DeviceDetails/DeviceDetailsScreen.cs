using System;
using System.Collections.Generic;
using CoreBluetooth;
using Foundation;
using UIKit;

namespace BluetoothLEExplorer.iOS
{
	[Register ("DeviceDetailsScreen")]
	public partial class DeviceDetailsScreen : UIViewController
	{
		readonly List<CBService> services = new List<CBService> ();
		ServiceDetailsScreen serviceDetailsScreen;
		ServiceTableSource tableSource;

		CBPeripheral connectedPeripheral;

		public CBPeripheral ConnectedPeripheral {
			get {
				return connectedPeripheral;
			}
			set {
				connectedPeripheral = value;
				InitializePeripheral ();
			}
		}

		public DeviceDetailsScreen (IntPtr handle)
			: base (handle)
		{
			Initialize ();
		}

		public DeviceDetailsScreen ()
		{
			Initialize ();
		}

		void Initialize ()
		{
			tableSource = new ServiceTableSource ();

			tableSource.ServiceSelected += (object sender, ServiceSelectedEventArgs e) => {
				serviceDetailsScreen = (ServiceDetailsScreen)Storyboard.InstantiateViewController ("ServiceDetailsScreen");
				serviceDetailsScreen.SetPeripheralAndService (connectedPeripheral, e.Service);
				NavigationController.PushViewController (serviceDetailsScreen, true);
			};
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			tableSource.Services = services;
			ServicesTableView.Source = tableSource;

			NavigationItem.SetLeftBarButtonItem (new UIBarButtonItem ("Disconnect", UIBarButtonItemStyle.Plain, (s, e) => {
				BluetoothLEManager.Current.DisconnectPeripheral (connectedPeripheral);
			}), false);
		}

		void InitializePeripheral ()
		{
			// update all our shit

			// > peripheral
			//   > service[s]
			//		> characteristic
			//			> value
			//			> descriptor[s]

			Title = connectedPeripheral.Name;

			// when a device disconnects, show an alert and unload this screen
			BluetoothLEManager.Current.DeviceDisconnected += HandleDeviceDisconnected;
			connectedPeripheral.DiscoveredService += HandleDiscoveredService;
			connectedPeripheral.DiscoverServices ();
		}

		void HandleDeviceDisconnected (object sender, CBPeripheralErrorEventArgs e)
		{
			string title = string.Format ("Peripheral Disconnected {0} disconnected", e.Peripheral.Name);

			var alert = new UIAlertView (title, null, null, "ok", null);
			alert.Clicked += (s, args) => {
				Console.WriteLine ("Alert.Clicked");
				NavigationController.PopToRootViewController (true);
			};
			alert.Show ();
		}

		void HandleDiscoveredService (object sender, NSErrorEventArgs e)
		{
			var peripheral = (CBPeripheral)sender;

			foreach (CBService service in peripheral.Services) {
				Console.WriteLine ("Discovered Service: {0}", service.Description);

				if (!services.Contains (service))
					services.Add (service);
			}

			ServicesTableView.ReloadData ();
		}
	}
}

