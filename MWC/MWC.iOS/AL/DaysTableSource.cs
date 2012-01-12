using System;
using System.Collections.Generic;
using MonoTouch.UIKit;

namespace MWC.iOS.AL
{
	public class DaysTableSource : UITableViewSource
	{
		public event EventHandler<DayClickedEventArgs> DayClicked = delegate { };
		private static string _cellId = "DayCell";
		
		public IList<string> Days
		{
			get { return this._days; }
			set { this._days = value; }
		}
		protected IList<String> _days; // = new List<String>() { "Monday", "Tuesday", "Wednesday", "Thursday" };
		
		public DaysTableSource () : base ()
		{
			_days = Constants.DayNames;
		}
		
		public override UITableViewCell GetCell (UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
		{
			UITableViewCell cell = tableView.DequeueReusableCell(_cellId);
			
			if(cell == null)
				cell = new UITableViewCell(UITableViewCellStyle.Default, _cellId);
			
			cell.TextLabel.Text = this._days[indexPath.Row];
			cell.BackgroundColor = UIColor.Black;
			cell.TextLabel.TextColor = UIColor.White;

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
			this.DayClicked ( this, new DayClickedEventArgs ( this._days [indexPath.Row], indexPath.Row + 1 ) );
			tableView.DeselectRow ( indexPath, true );
		}
	}
}