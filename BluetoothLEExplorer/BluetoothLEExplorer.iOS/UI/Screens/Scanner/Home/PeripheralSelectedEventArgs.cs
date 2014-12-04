using System;
using CoreBluetooth;

namespace BluetoothLEExplorer.iOS
{
	public class PeripheralSelectedEventArgs : EventArgs
	{
		public CBPeripheral SelectedPeripheral { get; private set; }

		public PeripheralSelectedEventArgs (CBPeripheral peripheral)
		{
			SelectedPeripheral = peripheral;
		}
	}
}