using System;
using System.Collections.Generic;
using CoreBluetooth;
using Foundation;
using UIKit;
using System.Text;

namespace BluetoothLEExplorer.iOS
{
	[Register("ServiceDetailsScreen")]
	public partial class ServiceDetailsScreen : UIViewController
	{
		readonly List<CBCharacteristic> characteristics = new List<CBCharacteristic>();
		readonly CharacteristicTableSource tableSource = new CharacteristicTableSource ();

		CBPeripheral connectedPeripheral;

		public ServiceDetailsScreen (IntPtr handle)
			: base(handle)
		{
			Initialize();
		}

		public ServiceDetailsScreen ()
		{
			Initialize ();
		}

		protected void Initialize()
		{
			tableSource.Characteristics = characteristics;

			// when the characteristic is selected in the table, make a request to disover the descriptors for it.
			tableSource.CharacteristicSelected += (sender, e) => {
				connectedPeripheral.DiscoverDescriptors(e.Characteristic);
			};
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			CharacteristicsTable.Source = tableSource;
		}

		public void SetPeripheralAndService (CBPeripheral peripheral, CBService service)
		{
			connectedPeripheral = peripheral;

			connectedPeripheral.DiscoveredCharacteristic += (sender, e) => {
				HandleDiscoveredCharacteristic((CBPeripheral)sender, service);
			};

			// when a descriptor is dicovered, reload the table.
			connectedPeripheral.DiscoveredDescriptor += HandleDiscoveredDescriptor;

			// discover the charactersistics
			connectedPeripheral.DiscoverCharacteristics(service);
		}

		void HandleDiscoveredCharacteristic (CBPeripheral peripheral, CBService service)
		{
			Console.WriteLine ("Discovered Characteristic.");
			foreach (CBService srv in peripheral.Services) {
				// if the service has characteristics yet
				if (srv.Characteristics == null)
					continue;

				foreach (var characteristic in service.Characteristics) {
					Console.WriteLine("Characteristic: {0}", characteristic.Description);
					characteristics.Add (characteristic);
				}
				CharacteristicsTable.ReloadData();
			}
		}

		void HandleDiscoveredDescriptor (object sender, CBCharacteristicEventArgs e)
		{
			foreach (var descriptor in e.Characteristic.Descriptors)
				Console.WriteLine ("Characteristic: {0} Discovered Descriptor: {1}", e.Characteristic.Value, descriptor);

			CharacteristicsTable.ReloadData();
		}
	}
}