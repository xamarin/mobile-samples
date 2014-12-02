using System;
using UIKit;
using Foundation;
using CoreBluetooth;
using System.Collections.Generic;
using BluetoothLEExplorer.iOS.UI.Controls;
using System.Threading.Tasks;
using MBProgressHUD;

namespace BluetoothLEExplorer.iOS.UI.Screens.Scanner.Home
{
	[Register("ScannerHome")]
	public partial class ScannerHome : UIViewController
	{
		ScanButton _scanButton;
		BleDeviceTableSource _tableSource;
		MTMBProgressHUD _connectingDialog;
		DeviceDetails.DeviceDetailsScreen _detailsScreen;

		public ScannerHome (IntPtr handle) : base (handle) 
		{
			this.Initialize ();
		}

		public ScannerHome ()
		{
			this.Initialize ();
		}

		protected void Initialize()
		{
			this.Title = "Scanner";

			// configure our scan button
			this._scanButton = new ScanButton ();
			this._scanButton.TouchUpInside += (s,e) => {
				if ( !BluetoothLEManager.Current.IsScanning ) {
					BluetoothLEManager.Current.BeginScanningForDevices ();
				} else {
					BluetoothLEManager.Current.StopScanningForDevices ();
				}
			};			 
			this.NavigationItem.SetRightBarButtonItem (new UIBarButtonItem (this._scanButton), false);

			// setup the table
			this._tableSource = new BleDeviceTableSource ();
			this._tableSource.PeripheralSelected += (object sender, BleDeviceTableSource.PeripheralSelectedEventArgs e) => {

				// stop scanning
				new Task( () => {
					if(BluetoothLEManager.Current.IsScanning) {
						Console.WriteLine ("Still scanning, stopping the scan and reseting the right button");
						BluetoothLEManager.Current.StopScanningForDevices();
						this._scanButton.SetState (ScanButton.ScanButtonState.Normal);
					}
				}).Start();

				// show our connecting... overlay
				this._connectingDialog.LabelText = "Connecting to " + e.SelectedPeripheral.Name;
				this._connectingDialog.Show(true);

				// when the peripheral connects, load our details screen
				BluetoothLEManager.Current.DeviceConnected += (object s, CBPeripheralEventArgs periphE) => {
					this._connectingDialog.Hide(false);

					this._detailsScreen = this.Storyboard.InstantiateViewController("DeviceDetailsScreen") as DeviceDetails.DeviceDetailsScreen;
					this._detailsScreen.ConnectedPeripheral = periphE.Peripheral;
					this.NavigationController.PushViewController ( this._detailsScreen, true);

				};

				// try and connect to the peripheral
				BluetoothLEManager.Current.CentralBleManager.ConnectPeripheral (e.SelectedPeripheral, new PeripheralConnectionOptions());
			};


		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			BleDevicesTable.Source = this._tableSource;

			// wire up the DiscoveredPeripheral event to update the table
			BluetoothLEManager.Current.DeviceDiscovered += (object sender, CBDiscoveredPeripheralEventArgs e) => {
				this._tableSource.Peripherals = BluetoothLEManager.Current.DiscoveredDevices;
				this.BleDevicesTable.ReloadData();
			};

			BluetoothLEManager.Current.ScanTimeoutElapsed += (sender, e) => {
				this._scanButton.SetState ( ScanButton.ScanButtonState.Normal );
			};

			// add our 'connecting' overlay
			this._connectingDialog = new MTMBProgressHUD (View) {
				LabelText = "Connecting to device...",
				RemoveFromSuperViewOnHide = false
			};
			this.View.AddSubview (this._connectingDialog);		
		}

		protected class BleDeviceTableSource : UITableViewSource
		{
			protected const string cellID = "BleDeviceCell";

			public event EventHandler<PeripheralSelectedEventArgs> PeripheralSelected = delegate {};

			public List<CBPeripheral> Peripherals
			{
				get { return this._peripherals; }
				set { this._peripherals = value; }
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
				return this._peripherals.Count;
			}

			public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
			{
				UITableViewCell cell = tableView.DequeueReusableCell (cellID);
				if (cell == null) {
					cell = new UITableViewCell (UITableViewCellStyle.Subtitle, cellID);
				}

				CBPeripheral peripheral = this._peripherals [indexPath.Row];
				//TODO: convert to async and update?
				peripheral.ReadRSSI ();
				cell.TextLabel.Text = peripheral.Name;
				cell.DetailTextLabel.Text = "UUID: " + peripheral.UUID.ToString () + ", Signal Strength: " + peripheral.RSSI;

				return cell;
			}

			public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
			{
				CBPeripheral peripheral = this._peripherals [indexPath.Row];
				tableView.DeselectRow (indexPath, false);
				this.PeripheralSelected (this, new PeripheralSelectedEventArgs (peripheral));
			}

			public class PeripheralSelectedEventArgs : EventArgs
			{
				public CBPeripheral SelectedPeripheral
				{
					get { return this._peripheral; }
				} protected CBPeripheral _peripheral;

				public PeripheralSelectedEventArgs (CBPeripheral peripheral)
				{
					this._peripheral = peripheral;
				}
			}
		}
	}

}

