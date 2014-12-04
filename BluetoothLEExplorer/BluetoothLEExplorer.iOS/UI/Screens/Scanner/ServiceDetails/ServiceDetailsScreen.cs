using System;
using System.Collections.Generic;
using CoreBluetooth;
using Foundation;
using UIKit;
using System.Text;

namespace BluetoothLEExplorer.iOS.UI.Screens.Scanner.ServiceDetails
{
	[Register("ServiceDetailsScreen")]
	public partial class ServiceDetailsScreen : UIViewController
	{
		CharacteristicTableSource _tableSource;
		protected List<CBCharacteristic> _characteristics = new List<CBCharacteristic>();

		protected CBPeripheral _connectedPeripheral;
		protected CBService _currentService;

		public ServiceDetailsScreen (IntPtr handle) : base(handle)
		{
			this.Initialize();
		}

		public ServiceDetailsScreen ()
		{
			this.Initialize ();
		}

		protected void Initialize()
		{
			this._tableSource = new CharacteristicTableSource ();
			this._tableSource.Characteristics = this._characteristics;

			// when the characteristic is selected in the table, make a request to disover the descriptors for it.
			this._tableSource.CharacteristicSelected += (object sender, CharacteristicTableSource.CharacteristicSelectedEventArgs e) => {
				this._connectedPeripheral.DiscoverDescriptors(e.Characteristic);
			};
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			this.CharacteristicsTable.Source = this._tableSource;

		}

		public void SetPeripheralAndService (CBPeripheral peripheral, CBService service)
		{
			_connectedPeripheral = peripheral;
			_currentService = service;

			_connectedPeripheral.DiscoveredCharacteristic += (object sender, CBServiceEventArgs e) => {
				Console.WriteLine ("Discovered Characteristic.");
				foreach (CBService srv in ((CBPeripheral)sender).Services) {

					// if the service has characteristics yet
					if(srv.Characteristics != null) {
						foreach (var characteristic in service.Characteristics) {
							Console.WriteLine("Characteristic: {0}", characteristic.Description);

							_characteristics.Add (characteristic);
							CharacteristicsTable.ReloadData();
						}
					}
				}
			};

			// when a descriptor is dicovered, reload the table.
			_connectedPeripheral.DiscoveredDescriptor += (object sender, CBCharacteristicEventArgs e) => {
				foreach (var descriptor in e.Characteristic.Descriptors)
					Console.WriteLine ("Characteristic: {0} Discovered Descriptor: {1}", e.Characteristic.Value, descriptor);

				// reload the table
				CharacteristicsTable.ReloadData();
			};

			// discover the charactersistics
			_connectedPeripheral.DiscoverCharacteristics(service);
		}

		protected class DisconnectAlertViewDelegate : UIAlertViewDelegate
		{
			protected UIViewController _parent;

			public DisconnectAlertViewDelegate(UIViewController parent)
			{
				this._parent = parent;
			}

			public override void Clicked (UIAlertView alertview, nint buttonIndex)
			{
				_parent.NavigationController.PopViewController(true);
			}

			public override void Canceled (UIAlertView alertView)
			{
				_parent.NavigationController.PopViewController(true);
			}
		}

		protected class CharacteristicTableSource : UITableViewSource
		{
			protected const string cellID = "BleCharacteristicCell";
			public event EventHandler<CharacteristicSelectedEventArgs> CharacteristicSelected = delegate {};

			public List<CBCharacteristic> Characteristics
			{
				get { return this._characteristics; }
				set { this._characteristics = value; }
			}
			protected List<CBCharacteristic> _characteristics = new List<CBCharacteristic>();

			public override nint NumberOfSections (UITableView tableView)
			{
				return 1;
			}

			public override nint RowsInSection (UITableView tableview, nint section)
			{
				return this._characteristics.Count;
			}

			public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
			{
				UITableViewCell cell = tableView.DequeueReusableCell (cellID);
				if (cell == null) {
					cell = new UITableViewCell (UITableViewCellStyle.Subtitle, cellID);
				}

				CBCharacteristic characteristic = this._characteristics [indexPath.Row];
				cell.TextLabel.Text = "Characteristic: " + characteristic.Description;
				StringBuilder descriptors = new StringBuilder ();
				if (characteristic.Descriptors != null) {
					foreach (var descriptor in characteristic.Descriptors) {
						descriptors.Append (descriptor.Description + " ");
					}
					cell.DetailTextLabel.Text = descriptors.ToString ();
				} else {
					cell.DetailTextLabel.Text = "Select to discover characteristic descriptors.";
				}

				return cell;
			}


			public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
			{
				CBCharacteristic characteristic = this._characteristics [indexPath.Row];
				Console.WriteLine ("Selected: " + characteristic.Description);

				this.CharacteristicSelected (this, new CharacteristicSelectedEventArgs (characteristic));

				tableView.DeselectRow (indexPath, true);
			}

			public class CharacteristicSelectedEventArgs : EventArgs
			{
				public CBCharacteristic Characteristic
				{
					get { return this._characteristic; }
					set { this._characteristic = value; }
				}
				protected CBCharacteristic _characteristic;

				public CharacteristicSelectedEventArgs (CBCharacteristic characteristic)
				{
					this._characteristic = characteristic;
				}
			}
		}
	}
}

