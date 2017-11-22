using System;
using System.Collections.Generic;

using UIKit;
using CoreBluetooth;
using Foundation;
using System.Text;

namespace BluetoothLEExplorer.iOS
{
	public class CharacteristicTableSource : UITableViewSource
	{
		public event EventHandler<CharacteristicSelectedEventArgs> CharacteristicSelected = delegate {};

		static readonly NSString cellID = (NSString)"BleCharacteristicCell";

		public List<CBCharacteristic> Characteristics { get; set; }

		public CharacteristicTableSource()
		{
			Characteristics = new List<CBCharacteristic>();
		}

		public override nint NumberOfSections (UITableView tableView)
		{
			return 1;
		}

		public override nint RowsInSection (UITableView tableview, nint section)
		{
			return Characteristics.Count;
		}

		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			UITableViewCell cell = tableView.DequeueReusableCell (cellID);
			cell = cell ?? new UITableViewCell (UITableViewCellStyle.Subtitle, cellID);

			CBCharacteristic characteristic = Characteristics [indexPath.Row];
			cell.TextLabel.Text = string.Format ("Characteristic: {0}", characteristic.Description);
			cell.DetailTextLabel.Text = FetchDetailedText (characteristic);

			return cell;
		}

		string FetchDetailedText(CBCharacteristic characteristic)
		{
			if (characteristic.Descriptors != null)
				return GetDescription (characteristic.Descriptors);
			else
				return "Select to discover characteristic descriptors.";
		}

		string GetDescription(IEnumerable<CBDescriptor> descriptors)
		{
			var sb = new StringBuilder ();

			foreach (var descriptor in descriptors)
				sb.Append (descriptor.Description + " ");

			return sb.ToString ();
		}

		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			CBCharacteristic characteristic = Characteristics [indexPath.Row];
			Console.WriteLine ("Selected: {0}", characteristic.Description);

			CharacteristicSelected (this, new CharacteristicSelectedEventArgs (characteristic));

			tableView.DeselectRow (indexPath, true);
		}
	}
}