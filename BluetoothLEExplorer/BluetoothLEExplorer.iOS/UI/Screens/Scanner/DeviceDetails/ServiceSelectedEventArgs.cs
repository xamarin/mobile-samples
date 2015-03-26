using System;
using CoreBluetooth;

namespace BluetoothLEExplorer.iOS
{
	public class ServiceSelectedEventArgs : EventArgs
	{
		public CBService Service { get; set; }

		public ServiceSelectedEventArgs (CBService service)
		{
			Service = service;
		}
	}
}

