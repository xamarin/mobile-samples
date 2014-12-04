using System;
using System.Collections.Generic;

using UIKit;
using CoreBluetooth;
using Foundation;

namespace BluetoothLEExplorer.iOS
{
	public class BleDeviceTableSource : UITableViewSource
	{
		public event EventHandler<PeripheralSelectedEventArgs> PeripheralSelected = delegate {};

		static readonly NSString cellID = (NSString)"BleDeviceCell";

		public List<CBPeripheral> Peripherals { get; set; }

		public BleDeviceTableSource (List<CBPeripheral> peripherals = null)
		{
			Peripherals = peripherals;
		}

		public override nint NumberOfSections (UITableView tableView)
		{
			return 1;
		}

		public override nint RowsInSection (UITableView tableview, nint section)
		{
			return Peripherals.Count;
		}

		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			UITableViewCell cell = tableView.DequeueReusableCell (cellID);
			cell = cell ?? new UITableViewCell (UITableViewCellStyle.Subtitle, cellID);

			CBPeripheral peripheral = Peripherals [indexPath.Row];

			//TODO: convert to async and update?
			peripheral.ReadRSSI ();
			cell.TextLabel.Text = peripheral.Name;
			cell.DetailTextLabel.Text = string.Format ("UUID: {0}, Signal Strength: {1}", GetIdentifier(peripheral), peripheral.RSSI);

			return cell;
		}

		NSUuid GetIdentifier(CBPeripheral peripheral)
		{
			// TODO: https://trello.com/c/t1dmC7hh
			if (UIDevice.CurrentDevice.CheckSystemVersion (7, 0))
				return peripheral.Identifier;
			else
				return new NSUuid (); // here we should get UUID instead of Identifier
		}

		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			CBPeripheral peripheral = Peripherals [indexPath.Row];
			tableView.DeselectRow (indexPath, false);
			PeripheralSelected (this, new PeripheralSelectedEventArgs (peripheral));
		}
	}
}

