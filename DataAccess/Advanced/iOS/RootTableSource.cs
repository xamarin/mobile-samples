using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Foundation;
using UIKit;

namespace DataAccess {
	public class RootTableSource : UITableViewSource {

		IList<Stock> tableItems;
	    string cellIdentifier = "stockcell"; // in Storyboard
	 
		public RootTableSource (IEnumerable<Stock> items)
		{
			tableItems = items.ToList(); 
		}
	    
	    public override nint RowsInSection (UITableView tableview, nint section)
	    {
	        return tableItems.Count;
	    }
	    public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
	    {
			Console.WriteLine ("   Id:" + tableItems[indexPath.Row].Id + " " + tableItems[indexPath.Row].Name);
			// in a Storyboard, Dequeue will ALWAYS return a cell, 
	        UITableViewCell cell = tableView.DequeueReusableCell (cellIdentifier);
   			cell.TextLabel.Text = tableItems[indexPath.Row].Name;
			cell.DetailTextLabel.Text = tableItems[indexPath.Row].Symbol;
		    return cell;
	    }

		public Stock GetItem(int id) {
			return tableItems[id];
		}
	}
}