using System;
using CoreBluetooth;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BluetoothLEExplorer.iOS
{
	/// <summary>
	/// Manager class for Bluetooth Low Energy connectivity. Adds functionality to the 
	/// CoreBluetooth Manager to track discovered devices, scanning state, and automatically
	/// stops scanning after a timeout period.
	/// </summary>
	public class BluetoothLEManager// : CBCentralManagerDelegate
	{
		// event declarations
		public event EventHandler<CBDiscoveredPeripheralEventArgs> DeviceDiscovered = delegate {};
		public event EventHandler<CBPeripheralEventArgs> DeviceConnected = delegate {};
		public event EventHandler<CBPeripheralErrorEventArgs> DeviceDisconnected = delegate {};
		public event EventHandler ScanTimeoutElapsed = delegate {};


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
		public List<CBPeripheral> DiscoveredDevices
		{
			get { return this._discoveredDevices; }
		}
		List<CBPeripheral> _discoveredDevices = new List<CBPeripheral>();

		/// <summary>
		/// Gets the connected peripherals.
		/// </summary>
		/// <value>The discovered peripherals.</value>
		public List<CBPeripheral> ConnectedDevices
		{
			get { return this._connectedDevices; }
		}
		List<CBPeripheral> _connectedDevices = new List<CBPeripheral>();

		public CBCentralManager CentralBleManager
		{
			get { return this._central; }
		}
		CBCentralManager _central;

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
			_central = new CBCentralManager (MonoTouch.CoreFoundation.DispatchQueue.CurrentQueue);
			_central.DiscoveredPeripheral += (object sender, CBDiscoveredPeripheralEventArgs e) => {
				Console.WriteLine ("DiscoveredPeripheral: " + e.Peripheral.Name);
				this._discoveredDevices.Add (e.Peripheral);
				this.DeviceDiscovered(this, e);
			};

			_central.UpdatedState += (object sender, EventArgs e) => {
				Console.WriteLine ("UpdatedState: " + _central.State);
			};


			_central.ConnectedPeripheral += (object sender, CBPeripheralEventArgs e) => {
				Console.WriteLine ("ConnectedPeripheral: " + e.Peripheral.Name);

				// when a peripheral gets connected, add that peripheral to our running list of connected peripherals
				if(!this._connectedDevices.Contains(e.Peripheral) ) {
					this._connectedDevices.Add (e.Peripheral );
				}			

				// raise our connected event
				this.DeviceConnected ( sender, e);
			
			};

			_central.DisconnectedPeripheral += (object sender, CBPeripheralErrorEventArgs e) => {
				Console.WriteLine ("DisconnectedPeripheral: " + e.Peripheral.Name);

				// when a peripheral disconnects, remove it from our running list.
				if ( this._connectedDevices.Contains (e.Peripheral) ) {
					this._connectedDevices.Remove ( e.Peripheral);
				}

				// raise our disconnected event
				this.DeviceDisconnected (sender, e);

			};
		}


		/// <summary>
		/// Begins the scanning for bluetooth LE devices. Automatically called after 10 seconds
		/// to prevent battery drain.
		/// </summary>
		/// <returns>The scanning for devices.</returns>
		public async Task BeginScanningForDevices()
		{
			Console.WriteLine ("BluetoothLEManager: Starting a scan for devices.");

			// clear out the list
			this._discoveredDevices = new List<CBPeripheral> ();

			// start scanning
			this._isScanning = true;
			_central.ScanForPeripherals (serviceUuids:null);

			// in 10 seconds, stop the scan
			await Task.Delay (10000);

			// if we're still scanning
			if (this._isScanning) {
				Console.WriteLine ("BluetoothLEManager: Scan timeout has elapsed.");
				this._central.StopScan ();
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
			_central.StopScan ();
		}

		//TODO: rename to DisconnectDevice
		public void DisconnectPeripheral (CBPeripheral peripheral)
		{
			_central.CancelPeripheralConnection (peripheral);
		}

	}
}

