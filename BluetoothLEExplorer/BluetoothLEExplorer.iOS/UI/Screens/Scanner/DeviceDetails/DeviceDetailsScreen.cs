using System;
using System.Collections.Generic;
using CoreBluetooth;
using Foundation;
using UIKit;

namespace BluetoothLEExplorer.iOS
{
	[Register("DeviceDetailsScreen")]
	public partial class DeviceDetailsScreen : UIViewController
	{
		protected List<CBService> _services = new List<CBService>();
		protected Dictionary<CBService, CBCharacteristic> _serviceCharacteristics = new Dictionary<CBService, CBCharacteristic>();
		protected ServiceDetailsScreen _serviceDetailsScreen;

		ServiceTableSource _tableSource;

		public CBPeripheral ConnectedPeripheral
		{
			get { return _connectedPeripheral; }
			set {
				_connectedPeripheral = value;
				InitializePeripheral ();
			}
		}
		protected CBPeripheral _connectedPeripheral;

		public DeviceDetailsScreen (IntPtr handle) : base(handle)
		{
			Initialize();
		}

		public DeviceDetailsScreen ()
		{
			Initialize ();
		}

		protected void Initialize()
		{
			_tableSource = new ServiceTableSource ();

			_tableSource.ServiceSelected += (object sender, ServiceTableSource.ServiceSelectedEventArgs e) => {
				_serviceDetailsScreen = Storyboard.InstantiateViewController("ServiceDetailsScreen") as ServiceDetailsScreen;
				_serviceDetailsScreen.SetPeripheralAndService ( _connectedPeripheral, e.Service );
				NavigationController.PushViewController(_serviceDetailsScreen, true);
			};
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			_tableSource.Services = _services;
			ServicesTableView.Source = _tableSource;

			NavigationItem.SetLeftBarButtonItem( new UIBarButtonItem ("Disconnect", UIBarButtonItemStyle.Plain, (s,e) => {
				BluetoothLEManager.Current.DisconnectPeripheral (_connectedPeripheral);
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

			Title = _connectedPeripheral.Name;

			// when a device disconnects, show an alert and unload this screen
			BluetoothLEManager.Current.DeviceDisconnected += (object sender, CBPeripheralErrorEventArgs e) => {
				var alert = new UIAlertView("Peripheral Disconnected", e.Peripheral.Name + " disconnected", 
					null, "ok", null);
				alert.Clicked += (object s, UIButtonEventArgs e2) => {
					Console.WriteLine ("Alert.Clicked");
					NavigationController.PopToRootViewController(true);//.PopViewControllerAnimated(true);
				};
				alert.Show();
			};

			_connectedPeripheral.DiscoverServices ();
			_connectedPeripheral.DiscoveredService += (object sender, NSErrorEventArgs err) => {
				foreach (CBService service in ((CBPeripheral)sender).Services) {
					Console.WriteLine ("Discovered Service: " + service.Description);

					if(!_services.Contains(service)) {
						_services.Add (service);
						ServicesTableView.ReloadData();
					}
				}
			};
		}

		protected class ServiceTableSource : UITableViewSource
		{
			public event EventHandler<ServiceSelectedEventArgs> ServiceSelected = delegate {};

			const string cellID = "BleServiceCell";

			public List<CBService> Services { get; set; }

			public ServiceTableSource()
			{
				Services = new List<CBService> ();
			}

			public override nint NumberOfSections (UITableView tableView)
			{
				return 1;
			}

			public override nint RowsInSection (UITableView tableview, nint section)
			{
				return Services.Count;
			}

			public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
			{
				UITableViewCell cell = tableView.DequeueReusableCell (cellID);
				if (cell == null) {
					cell = new UITableViewCell (UITableViewCellStyle.Default, cellID);
				}

				CBService service = Services [indexPath.Row];
				cell.TextLabel.Text = service.Description;

				return cell;
			}

			public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
			{
				CBService service = Services [indexPath.Row];

				ServiceSelected (this, new ServiceSelectedEventArgs (service));

				tableView.DeselectRow (indexPath, true);
			}

			public class ServiceSelectedEventArgs : EventArgs
			{
				public CBService Service { get; set; }

				public ServiceSelectedEventArgs (CBService service)
				{
					Service = service;
				}
			}
		}
	}
}

