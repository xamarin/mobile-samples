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
			
			cell.DetailTextLabel.Text = this.days[indexPath.Row].ToString("d MMMM yyyy");
			cell.DetailTextLabel.Font = UIFont.FromName("Helvetica-Light", 12f);
			
			if (AppDelegate.IsPhone) {
				cell.TextLabel.TextColor = UIColor.White;
				cell.DetailTextLabel.TextColor = UIColor.LightGray;
				cell.BackgroundColor = UIColor.Black;
			} else { // IsPad
				cell.TextLabel.Font = UIFont.FromName("Helvetica-Light", AppDelegate.Font16pt);
				cell.TextLabel.TextColor = UIColor.Black;
				cell.DetailTextLabel.Font = UIFont.FromName("Helvetica-LightOblique", AppDelegate.Font9pt);
				cell.DetailTextLabel.TextColor = UIColor.DarkGray;
				cell.BackgroundColor = UIColor.White;
			}
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
		 
		public override UIView GetViewForHeader (UITableView tableView, int section)
		{
			if (AppDelegate.IsPhone) return null;
			return BuildSectionHeaderView("Full Schedule");
		}
		
		/// <summary>
		/// Sharing this with all three tables on the HomeScreen
		/// </summary>
		public static UIView BuildSectionHeaderView(string caption) {
           UIView view = new UIView(new System.Drawing.RectangleF(0,0,320,20));
           UILabel label = new UILabel();
           label.BackgroundColor = UIColor.Clear;
           label.Opaque = false;
           label.TextColor = UIColor.White;
           label.Font = UIFont.FromName("Helvetica", 16f);
           label.Frame = new System.Drawing.RectangleF(15,0,290,20);
           label.Text = caption;
           view.AddSubview(label);
           return view;
		}
	}
}