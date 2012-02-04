using System;
using System.Collections.Generic;
using MonoTouch.UIKit;

namespace MWC.iOS.AL {
	public class DaysTableSource : UITableViewSource {
		public event EventHandler<DayClickedEventArgs> DayClicked = delegate 
		{
		};
		static string cellId = "DayCell";
		
		IList<DateTime> days;
		
		public DaysTableSource () : base ()
		{
			days = DaysManager.GetDays ();
		}
		
		public override UITableViewCell GetCell (UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
		{
			UITableViewCell cell = tableView.DequeueReusableCell(cellId);
			
			if(cell == null)
				cell = new UITableViewCell(UITableViewCellStyle.Value1, cellId);
			
			cell.TextLabel.Text = this.days[indexPath.Row].ToString ("dddd");
			cell.TextLabel.Font = UIFont.FromName("Helvetica-Bold", 18f);
			cell.TextLabel.TextColor = UIColor.White;

			cell.DetailTextLabel.Text = this.days[indexPath.Row].ToString("d MMMM yyyy");
			cell.DetailTextLabel.TextColor = UIColor.LightGray;
			cell.DetailTextLabel.Font = UIFont.FromName("Helvetica-Light", 12f);
			
			cell.BackgroundColor = UIColor.Black;
			
			return cell;
		}
		
		public override int RowsInSection (UITableView tableview, int section)
		{
			return this.days.Count;
		}
		
		public override int NumberOfSections (UITableView tableView)
		{
			return 1;
		}
		
		public override string TitleForHeader (UITableView tableView, int section)
		{
			if (AppDelegate.IsPad) return "Full Schedule";
			return null; // don't want a section title on the Phone
		}

		public override void RowSelected (UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
		{
			this.DayClicked ( this, new DayClickedEventArgs ( this.days [indexPath.Row].ToString ("dddd"), indexPath.Row + 1 ) );
			tableView.DeselectRow ( indexPath, true );
		}
	}
}