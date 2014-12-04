using System;
using CoreBluetooth;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoreFoundation;

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

		const int _scanTimeout = 10000;

		/// <summary>
		/// Whether or not we're currently scanning for peripheral devices
		/// </summary>
		/// <value><c>true</c> if this instance is scanning; otherwise, <c>false</c>.</value>
		public bool IsScanning { get; private set; } 

		/// <summary>
		/// Gets the discovered peripherals.
		/// </summary>
		/// <value>The discovered peripherals.</value>
		public List<CBPeripheral> DiscoveredDevices
		{
			get { return _discoveredDevices; }
		}
		List<CBPeripheral> _discoveredDevices = new List<CBPeripheral>();

		/// <summary>
		/// Gets the connected peripherals.
		/// </summary>
		/// <value>The discovered peripherals.</value>
		public List<CBPeripheral> ConnectedDevices
		{
			get { return _connectedDevices; }
		}
		List<CBPeripheral> _connectedDevices = new List<CBPeripheral>();

		public CBCentralManager CentralBleManager
		{
			get { return _central; }
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
			_central = new CBCentralManager (DispatchQueue.CurrentQueue);
			_central.DiscoveredPeripheral += (object sender, CBDiscoveredPeripheralEventArgs e) => {
				Console.WriteLine ("DiscoveredPeripheral: {0}", e.Peripheral.Name);
				_discoveredDevices.Add (e.Peripheral);
				DeviceDiscovered(this, e);
			};

			_central.UpdatedState += (object sender, EventArgs e) => {
				Console.WriteLine ("UpdatedState: {0}", _central.State);
			};


			_central.ConnectedPeripheral += (object sender, CBPeripheralEventArgs e) => {
				Console.WriteLine ("ConnectedPeripheral: {0}", e.Peripheral.Name);

				// when a peripheral gets connected, add that peripheral to our running list of connected peripherals
				if(!_connectedDevices.Contains(e.Peripheral) ) {
					_connectedDevices.Add (e.Peripheral );
				}			

				// raise our connected event
				DeviceConnected ( sender, e);
			
			};

			_central.DisconnectedPeripheral += (object sender, CBPeripheralErrorEventArgs e) => {
				Console.WriteLine ("DisconnectedPeripheral: " + e.Peripheral.Name);

				// when a peripheral disconnects, remove it from our running list.
				if (_connectedDevices.Contains (e.Peripheral) ) {
					_connectedDevices.Remove ( e.Peripheral);
				}

				// raise our disconnected event
				DeviceDisconnected (sender, e);
			};
		}

		/// <summary>
		/// Begins the scanning for bluetooth LE devices. Automatically called after 10 seconds
		/// to prevent battery drain.
		/// </summary>
		/// <returns>The scanning for devices.</returns>
		public async void BeginScanningForDevices()
		{
			Console.WriteLine ("BluetoothLEManager: Starting a scan for devices.");

			// clear out the list
			_discoveredDevices = new List<CBPeripheral> ();

			// start scanning
			IsScanning = true;
			_central.ScanForPeripherals ((CBUUID[])null);

			// in 10 seconds, stop the scan
			await Task.Delay (10000);

			// if we're still scanning
			if (IsScanning) {
				Console.WriteLine ("BluetoothLEManager: Scan timeout has elapsed.");
				_central.StopScan ();
				ScanTimeoutElapsed (this, new EventArgs ());
			}
		}

		/// <summary>
		/// Stops the Central Bluetooth Manager from scanning for more devices. Automatically
		/// called after 10 seconds to prevent battery drain. 
		/// </summary>
		public void StopScanningForDevices()
		{
			Console.WriteLine ("BluetoothLEManager: Stopping the scan for devices.");
			IsScanning = false;
			_central.StopScan ();
		}

		//TODO: rename to DisconnectDevice
		public void DisconnectPeripheral (CBPeripheral peripheral)
		{
			_central.CancelPeripheralConnection (peripheral);
		}
	}
}