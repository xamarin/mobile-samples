using System;
using CoreBluetooth;

namespace BluetoothLEExplorer.iOS
{
	public class CharacteristicSelectedEventArgs : EventArgs
	{
		public CBCharacteristic Characteristic { get; set; }

		public CharacteristicSelectedEventArgs (CBCharacteristic characteristic)
		{
			Characteristic = characteristic;
		}
	}
}

