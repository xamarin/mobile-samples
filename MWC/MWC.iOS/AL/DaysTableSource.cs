using System;
using System.Collections.Generic;
using MonoTouch.UIKit;

namespace MWC.iOS.AL
{
	public class DaysTableSource : UITableViewSource
	{
		public event EventHandler<DayClickedEventArgs> DayClicked = delegate { };
		static string _cellId = "DayCell";
		
		IList<DateTime> _days;
		
		public DaysTableSource () : base ()
		{
			_days = DaysManager.GetDays ();
		}
		
		public override UITableViewCell GetCell (UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
		{
			UITableViewCell cell = tableView.DequeueReusableCell(_cellId);
			
			if(cell == null)
				cell = new UITableViewCell(UITableViewCellStyle.Value1, _cellId);
			
			cell.TextLabel.Text = this._days[indexPath.Row].ToString ("dddd");
			cell.TextLabel.Font = UIFont.FromName("Helvetica-Bold", 18f);
			cell.TextLabel.TextColor = UIColor.White;

			cell.DetailTextLabel.Text = this._days[indexPath.Row].ToString("d MMMM yyyy");
			cell.DetailTextLabel.TextColor = UIColor.LightGray;
			cell.DetailTextLabel.Font = UIFont.FromName("Helvetica-Light", 12f);
			
			cell.BackgroundColor = UIColor.Black;
			
			return cell;
		}
		
		public override int RowsInSection (UITableView tableview, int section)
		{
			return this._days.Count;
		}
		
		public override int NumberOfSections (UITableView tableView)
		{
			return 1;
		}
		
		public override void RowSelected (UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
		{
			this.DayClicked ( this, new DayClickedEventArgs ( this._days [indexPath.Row].ToString ("dddd"), indexPath.Row + 1 ) );
			tableView.DeselectRow ( indexPath, true );
		}
	}
}