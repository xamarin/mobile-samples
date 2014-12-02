using System;
using System.Collections.Generic;
using CoreBluetooth;
using Foundation;
using UIKit;

namespace BluetoothLEExplorer.iOS.UI.Screens.Scanner.DeviceDetails
{
	[Register("DeviceDetailsScreen")]
	public partial class DeviceDetailsScreen : UIViewController
	{
		protected List<CBService> _services = new List<CBService>();
		protected Dictionary<CBService, CBCharacteristic> _serviceCharacteristics = new Dictionary<CBService, CBCharacteristic>();
		protected ServiceDetails.ServiceDetailsScreen _serviceDetailsScreen;

		ServiceTableSource _tableSource;

		public CBPeripheral ConnectedPeripheral
		{
			get { return this._connectedPeripheral; }
			set {
				this._connectedPeripheral = value;
				this.InitializePeripheral ();
			}
		}
		protected CBPeripheral _connectedPeripheral;

		public DeviceDetailsScreen (IntPtr handle) : base(handle)
		{
			this.Initialize();
		}

		public DeviceDetailsScreen ()
		{
			this.Initialize ();
		}

		protected void Initialize()
		{
			this._tableSource = new ServiceTableSource ();

			this._tableSource.ServiceSelected += (object sender, ServiceTableSource.ServiceSelectedEventArgs e) => {
				this._serviceDetailsScreen = this.Storyboard.InstantiateViewController("ServiceDetailsScreen") as ServiceDetails.ServiceDetailsScreen;
				this._serviceDetailsScreen.SetPeripheralAndService ( this._connectedPeripheral, e.Service );
				this.NavigationController.PushViewController(this._serviceDetailsScreen, true);
			};
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			this._tableSource.Services = this._services;
			this.ServicesTableView.Source = this._tableSource;

			this.NavigationItem.SetLeftBarButtonItem( new UIBarButtonItem ("Disconnect", UIBarButtonItemStyle.Plain, (s,e) => {
				BluetoothLEManager.Current.DisconnectPeripheral (this._connectedPeripheral);
			}), false );
		}

		protected void InitializePeripheral()
		{
			// update all our shit

			// > peripheral
			//   > service[s]
			//		> characteristic
			//			> value
			//			> descriptor[s]

			this.Title = this._connectedPeripheral.Name;

			// when a device disconnects, show an alert and unload this screen
			BluetoothLEManager.Current.DeviceDisconnected += (object sender, CBPeripheralErrorEventArgs e) => {
				var alert = new UIAlertView("Peripheral Disconnected", e.Peripheral.Name + " disconnected", 
					null, "ok", null);
				alert.Clicked += (object s, UIButtonEventArgs e2) => {
					Console.WriteLine ("Alert.Clicked");
					this.NavigationController.PopToRootViewController(true);//.PopViewControllerAnimated(true);
				};
				alert.Show();
			};

			//
			this._connectedPeripheral.DiscoverServices ();
			this._connectedPeripheral.DiscoveredService += (object sender, NSErrorEventArgs err) => {
				foreach (CBService service in ((CBPeripheral)sender).Services) {
					Console.WriteLine ("Discovered Service: " + service.Description);

					if(!this._services.Contains(service)) {
						this._services.Add (service);

						this.ServicesTableView.ReloadData();
					}

				}
			};

		}

		protected class ServiceTableSource : UITableViewSource
		{
			protected const string cellID = "BleServiceCell";
			public event EventHandler<ServiceSelectedEventArgs> ServiceSelected = delegate {};

			public List<CBService> Services
			{
				get { return this._services; }
				set { this._services = value; }
			}
			protected List<CBService> _services = new List<CBService>();

			public override nint NumberOfSections (UITableView tableView)
			{
				return 1;
			}

			public override nint RowsInSection (UITableView tableview, nint section)
			{
				return this._services.Count;
			}

			public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
			{
				UITableViewCell cell = tableView.DequeueReusableCell (cellID);
				if (cell == null) {
					cell = new UITableViewCell (UITableViewCellStyle.Default, cellID);
				}

				CBService service = this._services [indexPath.Row];
				cell.TextLabel.Text = service.Description;

				return cell;
			}

			public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
			{
				CBService service = this._services [indexPath.Row];

				this.ServiceSelected (this, new ServiceSelectedEventArgs (service));

				tableView.DeselectRow (indexPath, true);
			}

			public class ServiceSelectedEventArgs : EventArgs
			{
				public CBService Service
				{
					get { return this._service; }
					set { this._service = value; }
				}
				protected CBService _service;

				public ServiceSelectedEventArgs (CBService service)
				{
					this._service = service;
				}
			}
		}
	}
}

