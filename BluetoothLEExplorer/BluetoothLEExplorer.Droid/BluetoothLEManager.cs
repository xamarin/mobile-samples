using System;
using Android;
using Android.Bluetooth;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BluetoothLEExplorer.Droid
{
	public class BluetoothLEManager : Java.Lang.Object, BluetoothAdapter.ILeScanCallback
	{
		public event EventHandler ScanTimeoutElapsed = delegate {};
		public event EventHandler<DeviceDiscoveredEventArgs> DeviceDiscovered = delegate {};
		public event EventHandler<DeviceConnectionEventArgs> DeviceConnected = delegate {};
		public event EventHandler<DeviceConnectionEventArgs> DeviceDisconnected = delegate {};
		public event EventHandler<ServiceDiscoveredEventArgs> ServiceDiscovered = delegate {};


		protected BluetoothManager _manager;
		protected BluetoothAdapter _adapter;
		protected GattCallback _gattCallback;

		/// <summary>
		/// Whether or not we're currently scanning for peripheral devices
		/// </summary>
		/// <value><c>true</c> if this instance is scanning; otherwise, <c>false</c>.</value>
		public bool IsScanning
		{
			get { return this._isScanning; }
		} protected bool _isScanning = false;
		protected const int _scanTimeout = 10000;

				/// <summary>
		/// Gets the discovered peripherals.
		/// </summary>
		/// <value>The discovered peripherals.</value>
		public List<BluetoothDevice> DiscoveredDevices
		{
			get { return this._discoveredDevices; }
		}
		List<BluetoothDevice> _discoveredDevices = new List<BluetoothDevice>();

		/// <summary>
		/// Gets the connected peripherals.
		/// TODO: in the xplat API, make sure to combine the GATT into a single 
		/// IDevice object so it isn't necessary to create a dictionary to track them.
		/// </summary>
		/// <value>The discovered peripherals.</value>
		public Dictionary<BluetoothDevice, BluetoothGatt> ConnectedDevices
		{
			get { return this._connectedDevices; }
		} protected Dictionary<BluetoothDevice, BluetoothGatt> _connectedDevices = new Dictionary<BluetoothDevice, BluetoothGatt>();

		/// <summary>
		/// Gets the services.
		/// </summary>
		/// <value>The services.</value>
		public Dictionary<BluetoothDevice, IList<BluetoothGattService>> Services
		{
			get { return this._services; }
		} protected Dictionary<BluetoothDevice, IList<BluetoothGattService>> _services = new Dictionary<BluetoothDevice, IList<BluetoothGattService>>();

//		/// <summary>
//		/// Need to have this because the google BLE API is terrible. in it, we cache the device's
//		/// GATT when we call Connect, so that later, we can add it to _connectedDevices, because
//		/// we're not given a reference to the device when it connects
//		/// </summary>
//		protected Dictionary<BluetoothDevice, BluetoothGatt> _connectingDevices = new Dictionary<BluetoothDevice, BluetoothGatt>();

		public static BluetoothLEManager Current
		{
			get { return current; }
		} private static BluetoothLEManager current;

		static BluetoothLEManager ()
		{
			current = new BluetoothLEManager();
		}

		protected BluetoothLEManager ()
		{
			var appContext = Android.App.Application.Context;
			// get a reference to the bluetooth system service
			this._manager = (BluetoothManager) appContext.GetSystemService("bluetooth");
			this._adapter = this._manager.Adapter;

			this._gattCallback = new GattCallback (this);
		}

		public async Task BeginScanningForDevices()
		{
			Console.WriteLine ("BluetoothLEManager: Starting a scan for devices.");

			// clear out the list
			this._discoveredDevices = new List<BluetoothDevice> ();

			// start scanning
			this._isScanning = true;
			_adapter.StartLeScan(this);

			// in 10 seconds, stop the scan
			await Task.Delay (10000);

			// if we're still scanning
			if (this._isScanning) {
				Console.WriteLine ("BluetoothLEManager: Scan timeout has elapsed.");
				this._adapter.StopLeScan (this);
				this.ScanTimeoutElapsed (this, new EventArgs ());
			}
		}

		/// <summary>
		/// Stops the Central Bluetooth Manager from scanning for more devices. Automatically
		/// called after 10 seconds to prevent battery drain. 
		/// </summary>
		public void StopScanningForDevices()
		{
			Console.WriteLine ("BluetoothLEManager: Stopping the scan for devices.");
			this._isScanning = false;	
			this._adapter.StopLeScan (this);
		}

		public void OnLeScan (BluetoothDevice device, int rssi, byte[] scanRecord)
		{
			Console.WriteLine ("LeScanCallback: " + device.Name);
			// TODO: for some reason, this doesn't work, even though they have the same pointer,
			// it thinks that the item doesn't exist. so i had to write my own implementation
//			if(!this._discoveredDevices.Contains(device) ) {
//				this._discoveredDevices.Add (device );
//			}		
			if (!DeviceExistsInDiscoveredList (device))
				this._discoveredDevices.Add	(device);
			// TODO: in the cross platform API, cache the RSSI
			this.DeviceDiscovered (this, new DeviceDiscoveredEventArgs { Device = device, Rssi = rssi, ScanRecord = scanRecord });
		}

		protected bool DeviceExistsInDiscoveredList(BluetoothDevice device)
		{
			foreach (var d in this._discoveredDevices) {
				// TODO: verify that address is unique
				if (device.Address == d.Address)
					return true;
			}
			return false;
		}

		//TODO: this really should be async. in the xplat API, make sure to asyncify
		// Q: how to return in same context (requires a callback)
		public void ConnectToDevice (BluetoothDevice device)
		{
			// returns the BluetoothGatt, which is the API for BLE stuff
			// TERRIBLE API design on the part of google here.
			device.ConnectGatt (Android.App.Application.Context, true, this._gattCallback);
		}

		public void DisconnectDevice (BluetoothDevice device)
		{
			this.ConnectedDevices [device].Disconnect ();
			this.ConnectedDevices [device].Close ();
		}

		public BluetoothDevice GetConnectedDeviceByName (string deviceName)
		{
			foreach (var item in this._connectedDevices) {
				if (item.Key.Name == deviceName)
					return item.Key;
			}
			// if we got here we didn't find it.
			return null;
		}

//		public void Dispose ()
//		{
//			if (this._adapter != null)
//				this._adapter.Dispose ();
//			if (this._manager != null)
//				this._manager.Dispose ();
//		}


		public class DeviceDiscoveredEventArgs : EventArgs
		{
			public BluetoothDevice Device;
			public int Rssi;
			public byte[] ScanRecord;

			public DeviceDiscoveredEventArgs() : base()
			{}
		}

		public class DeviceConnectionEventArgs : EventArgs
		{
			public BluetoothDevice Device;

			public DeviceConnectionEventArgs() : base()
			{}
		}

		public class ServiceDiscoveredEventArgs : EventArgs
		{
			public BluetoothGatt Gatt;

			public ServiceDiscoveredEventArgs() : base ()
			{}
		}

		protected class GattCallback : BluetoothGattCallback
		{
			protected BluetoothLEManager _parent;

			public GattCallback (BluetoothLEManager parent)
			{
				this._parent = parent;
			}

			public override void OnConnectionStateChange (BluetoothGatt gatt, GattStatus status, ProfileState newState)
			{
				Console.WriteLine ("OnConnectionStateChange: ");
				base.OnConnectionStateChange (gatt, status, newState);

				switch (newState) {
				// disconnected
				case ProfileState.Disconnected:
					Console.WriteLine ("disconnected");
					//TODO/BUG: Need to remove this, but can't remove the key (uncomment and see bug on disconnect)
//					if (this._parent._connectedDevices.ContainsKey (gatt.Device))
//						this._parent._connectedDevices.Remove (gatt.Device);
					this._parent.DeviceDisconnected (this, new DeviceConnectionEventArgs () { Device = gatt.Device });
					break;
				// connecting
				case ProfileState.Connecting:
					Console.WriteLine ("Connecting");
					break;
				// connected
				case ProfileState.Connected:
					Console.WriteLine ("Connected");
					//TODO/BUGBUG: need to remove this when disconnected
					this._parent._connectedDevices.Add (gatt.Device, gatt);
					this._parent.DeviceConnected (this, new DeviceConnectionEventArgs () { Device = gatt.Device });
					break;
				// disconnecting
				case ProfileState.Disconnecting:
					Console.WriteLine ("Disconnecting");
					break;
				}
			}

			public override void OnServicesDiscovered (BluetoothGatt gatt, GattStatus status)
			{
				base.OnServicesDiscovered (gatt, status);

				Console.WriteLine ("OnServicesDiscovered: " + status.ToString ());

				//TODO: somehow, we need to tie this directly to the device, rather than for all
				// google's API deisgners are children.

				//TODO: not sure if this gets called after all services have been enumerated or not
				if(!this._parent._services.ContainsKey(gatt.Device))
					this._parent.Services.Add(gatt.Device, this._parent._connectedDevices [gatt.Device].Services);
				else
					this._parent._services[gatt.Device] = this._parent._connectedDevices [gatt.Device].Services;

				this._parent.ServiceDiscovered (this, new ServiceDiscoveredEventArgs () {
					Gatt = gatt
				});
			}

		}
	}
}

