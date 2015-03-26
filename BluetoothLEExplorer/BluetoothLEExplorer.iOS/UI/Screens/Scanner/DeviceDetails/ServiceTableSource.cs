using System;
using System.Collections.Generic;

using UIKit;
using CoreBluetooth;
using Foundation;

namespace BluetoothLEExplorer.iOS
{
	class ServiceTableSource : UITableViewSource
	{
		public event EventHandler<ServiceSelectedEventArgs> ServiceSelected = delegate {};

		static readonly NSString cellID = (NSString)"BleServiceCell";

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
			cell = cell ?? new UITableViewCell (UITableViewCellStyle.Default, cellID);

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
	}
}