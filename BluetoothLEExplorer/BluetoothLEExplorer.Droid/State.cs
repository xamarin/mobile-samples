using System;
using Android.Bluetooth;
using Java.Util;

namespace BluetoothLEExplorer.Droid
{
	public class State
	{
		public BluetoothDevice SelectedDevice { get; set; }
		public BluetoothGattService SelectedService { get; set; }

		public State ()
		{
			BluetoothLEManager.Current.DeviceDisconnected += (object sender, BluetoothLEManager.DeviceConnectionEventArgs e) => {
				this.ClearSelectedState();
			};
		}

		protected void ClearSelectedState()
		{
			this.SelectedDevice = null;
		}
	}
}

