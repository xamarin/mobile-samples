using System;
using UIKit;
using Foundation;
using CoreBluetooth;
using System.Collections.Generic;
using System.Threading.Tasks;
using MBProgressHUD;

namespace BluetoothLEExplorer.iOS
{
	[Register("ScannerHome")]
	public partial class ScannerHome : UIViewController
	{
		ScanButton _scanButton;
		BleDeviceTableSource _tableSource;
		MTMBProgressHUD _connectingDialog;
		DeviceDetailsScreen _detailsScreen;

		public ScannerHome (IntPtr handle) : base (handle) 
		{
			Initialize ();
		}

		public ScannerHome ()
		{
			Initialize ();
		}

		protected void Initialize()
		{
			Title = "Scanner";

			// configure our scan button
			_scanButton = new ScanButton ();
			_scanButton.TouchUpInside += (s,e) => {
				if (BluetoothLEManager.Current.IsScanning)
					BluetoothLEManager.Current.StopScanningForDevices ();
				else
					BluetoothLEManager.Current.BeginScanningForDevices ();
			};
			NavigationItem.SetRightBarButtonItem (new UIBarButtonItem (_scanButton), false);

			// setup the table
			_tableSource = new BleDeviceTableSource ();
			_tableSource.PeripheralSelected += (object sender, BleDeviceTableSource.PeripheralSelectedEventArgs e) => {

				// stop scanning
				new Task( () => {
					if(BluetoothLEManager.Current.IsScanning) {
						Console.WriteLine ("Still scanning, stopping the scan and reseting the right button");
						BluetoothLEManager.Current.StopScanningForDevices();
						_scanButton.SetState (ScanButton.ScanButtonState.Normal);
					}
				}).Start();

				// show our connecting... overlay
				_connectingDialog.LabelText = "Connecting to " + e.SelectedPeripheral.Name;
				_connectingDialog.Show(true);

				// when the peripheral connects, load our details screen
				BluetoothLEManager.Current.DeviceConnected += (object s, CBPeripheralEventArgs periphE) => {
					_connectingDialog.Hide(false);

					_detailsScreen = Storyboard.InstantiateViewController("DeviceDetailsScreen") as DeviceDetailsScreen;
					_detailsScreen.ConnectedPeripheral = periphE.Peripheral;
					NavigationController.PushViewController (_detailsScreen, true);

				};

				// try and connect to the peripheral
				BluetoothLEManager.Current.CentralBleManager.ConnectPeripheral (e.SelectedPeripheral, new PeripheralConnectionOptions());
			};


		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			BleDevicesTable.Source = _tableSource;

			// wire up the DiscoveredPeripheral event to update the table
			BluetoothLEManager.Current.DeviceDiscovered += (object sender, CBDiscoveredPeripheralEventArgs e) => {
				_tableSource.Peripherals = BluetoothLEManager.Current.DiscoveredDevices;
				BleDevicesTable.ReloadData();
			};

			BluetoothLEManager.Current.ScanTimeoutElapsed += (sender, e) => {
				_scanButton.SetState ( ScanButton.ScanButtonState.Normal );
			};

			// add our 'connecting' overlay
			_connectingDialog = new MTMBProgressHUD (View) {
				LabelText = "Connecting to device...",
				RemoveFromSuperViewOnHide = false
			};
			View.AddSubview (_connectingDialog);
		}

		protected class BleDeviceTableSource : UITableViewSource
		{
			protected const string cellID = "BleDeviceCell";

			public event EventHandler<PeripheralSelectedEventArgs> PeripheralSelected = delegate {};

			public List<CBPeripheral> Peripherals
			{
				get { return _peripherals; }
				set { _peripherals = value; }
			}
			protected List<CBPeripheral> _peripherals = new List<CBPeripheral> ();

			public BleDeviceTableSource () {}

			public BleDeviceTableSource (List<CBPeripheral> peripherals)
			{
				_peripherals = peripherals;
			}

			public override nint NumberOfSections (UITableView tableView)
			{
				return 1;
			}

			public override nint RowsInSection (UITableView tableview, nint section)
			{
				return _peripherals.Count;
			}

			public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
			{
				UITableViewCell cell = tableView.DequeueReusableCell (cellID);
				if (cell == null) {
					cell = new UITableViewCell (UITableViewCellStyle.Subtitle, cellID);
				}

				CBPeripheral peripheral = _peripherals [indexPath.Row];
				//TODO: convert to async and update?
				peripheral.ReadRSSI ();
				cell.TextLabel.Text = peripheral.Name;

				// TODO: https://trello.com/c/t1dmC7hh
				if (UIDevice.CurrentDevice.CheckSystemVersion (7, 0))
					cell.DetailTextLabel.Text = string.Format ("UUID: {0}, Signal Strength: {1}", peripheral.Identifier, peripheral.RSSI);
				else
					; // here we should get UUID instead of Identifier

				return cell;
			}

			public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
			{
				CBPeripheral peripheral = _peripherals [indexPath.Row];
				tableView.DeselectRow (indexPath, false);
				PeripheralSelected (this, new PeripheralSelectedEventArgs (peripheral));
			}

			public class PeripheralSelectedEventArgs : EventArgs
			{
				public CBPeripheral SelectedPeripheral { get; private set; }

				public PeripheralSelectedEventArgs (CBPeripheral peripheral)
				{
					SelectedPeripheral = peripheral;
				}
			}
		}
	}

}

