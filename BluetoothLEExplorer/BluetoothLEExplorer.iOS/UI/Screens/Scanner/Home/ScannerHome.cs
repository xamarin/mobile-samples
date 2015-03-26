using System;
using UIKit;
using Foundation;
using CoreBluetooth;
using System.Collections.Generic;
using System.Threading.Tasks;
using MBProgressHUD;

namespace BluetoothLEExplorer.iOS
{
	[Register ("ScannerHome")]
	public partial class ScannerHome : UIViewController
	{
		ScanButton scanButton;
		BleDeviceTableSource tableSource;
		MTMBProgressHUD connectingDialog;
		DeviceDetailsScreen detailsScreen;

		public ScannerHome (IntPtr handle)
			: base (handle)
		{
			Initialize ();
		}

		public ScannerHome ()
		{
			Initialize ();
		}

		void Initialize ()
		{
			Title = "Scanner";

			// configure our scan button
			scanButton = new ScanButton ();
			scanButton.TouchUpInside += HandleTouchUpInside;
			NavigationItem.SetRightBarButtonItem (new UIBarButtonItem (scanButton), false);

			// setup the table
			tableSource = new BleDeviceTableSource ();
			tableSource.PeripheralSelected += HandlePeripheralSelected;
		}

		void HandleTouchUpInside (object sender, EventArgs e)
		{
			if (BluetoothLEManager.Current.IsScanning)
				BluetoothLEManager.Current.StopScanningForDevices ();
			else
				BluetoothLEManager.Current.BeginScanningForDevices ();
		}

		void HandlePeripheralSelected (object sender, PeripheralSelectedEventArgs e)
		{
			StopScanning ();

			// show our connecting... overlay
			connectingDialog.LabelText = string.Format("Connecting to {0}", e.SelectedPeripheral.Name);
			connectingDialog.Show (true);

			// when the peripheral connects, load our details screen
			BluetoothLEManager.Current.DeviceConnected += HandleDeviceConnected;

			// try and connect to the peripheral
			BluetoothLEManager.Current.CentralBleManager.ConnectPeripheral (e.SelectedPeripheral, new PeripheralConnectionOptions ());
		}

		void StopScanning()
		{
			Task.Factory.StartNew (() => {
				if (!BluetoothLEManager.Current.IsScanning)
					return;

				Console.WriteLine ("Still scanning, stopping the scan and reseting the right button");
				BluetoothLEManager.Current.StopScanningForDevices ();
				InvokeOnMainThread (() => scanButton.SetState (ScanButton.ScanButtonState.Normal));
			});
		}

		void HandleDeviceConnected (object sender, CBPeripheralEventArgs e)
		{
			connectingDialog.Hide (false);

			detailsScreen = Storyboard.InstantiateViewController ("DeviceDetailsScreen") as DeviceDetailsScreen;
			detailsScreen.ConnectedPeripheral = e.Peripheral;
			NavigationController.PushViewController (detailsScreen, true);
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			BleDevicesTable.Source = tableSource;

			// wire up the DiscoveredPeripheral event to update the table
			BluetoothLEManager.Current.DeviceDiscovered += (object sender, CBDiscoveredPeripheralEventArgs e) => {
				tableSource.Peripherals = BluetoothLEManager.Current.DiscoveredDevices;
				BleDevicesTable.ReloadData ();
			};

			BluetoothLEManager.Current.ScanTimeoutElapsed += (sender, e) => {
				InvokeOnMainThread (() => scanButton.SetState (ScanButton.ScanButtonState.Normal));
			};

			// add our 'connecting' overlay
			connectingDialog = new MTMBProgressHUD (View) {
				LabelText = "Connecting to device...",
				RemoveFromSuperViewOnHide = false
			};
			View.AddSubview (connectingDialog);
		}
	}
}