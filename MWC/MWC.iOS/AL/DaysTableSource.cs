using System;
using System.Collections.Generic;
using UIKit;
using Foundation;
using MWC.iOS.UI.CustomElements;

namespace MWC.iOS.AL {
	public class DaysTableSource : UITableViewSource {
		public event EventHandler<DayClickedEventArgs> DayClicked = delegate 
		{
		};
		static NSString cellId = new NSString("DayCell");
		
		IList<DateTime> days;
		
		public DaysTableSource () : base ()
		{
			days = DaysManager.GetDays ();
		}
		
		public override UITableViewCell GetCell (UITableView tableView, Foundation.NSIndexPath indexPath)
		{
			DayCell cell = tableView.DequeueReusableCell(cellId) as DayCell;
			
			if(cell == null)
				cell = new DayCell(days[indexPath.Row].ToString ("dddd"), days[indexPath.Row], cellId);
			else
				cell.UpdateCell (days[indexPath.Row].ToString ("dddd"), days[indexPath.Row]);

			cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
			return cell;
		}
		
		public override nint RowsInSection (UITableView tableview, nint section)
		{
			return this.days.Count;
		}
		
		public override nint NumberOfSections (UITableView tableView)
		{
			return 1;
		}
		
		public override nfloat GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
		{
			if (AppDelegate.IsPad)
				return 70f;
			else
				return 45f;
		}

		public override void RowSelected (UITableView tableView, Foundation.NSIndexPath indexPath)
		{
			this.DayClicked (this, new DayClickedEventArgs (days [indexPath.Row].ToString ("dddd"), indexPath.Row + 1 ) );
			tableView.DeselectRow ( indexPath, true );
		}
		 
		public override nfloat GetHeightForHeader (UITableView tableView, nint section)
		{
			return 30f;
		}
		public override string TitleForHeader (UITableView tableView, nint section)
		{
			return "Full Schedule";
//			if (AppDelegate.IsPad) return "Full Schedule";
//			return null; // don't want a section title on the Phone
		}
		public override UIView GetViewForHeader (UITableView tableView, nint section)
		{
//			if (AppDelegate.IsPhone) return null;
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
           label.TextColor = AppDelegate.ColorHeadingHome; //UIColor.FromRGB (150, 210, 254);
           label.Font = UIFont.FromName("Helvetica-Bold", 16f);
           label.Frame = new System.Drawing.RectangleF(15,0,290,20);
           label.Text = caption;
           view.AddSubview(label);
           return view;
		}
	}
}