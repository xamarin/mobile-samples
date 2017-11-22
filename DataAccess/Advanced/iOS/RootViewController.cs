using System;
using System.Collections.Generic;
using Foundation;
using UIKit;

namespace DataAccess
{
	public partial class RootViewController : UITableViewController
	{
		public RootViewController (IntPtr handle) : base (handle)
		{
		}

		IEnumerable<Stock> stocks;

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			// Wire up the Add [+] button
			AddButton.Clicked += (sender, e) => {
				EditStock ();
			};
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			stocks = AppDelegate.Database.GetStocks ();
			TableView.Source = new RootTableSource(stocks);
			TableView.ReloadData ();
		}

		/// <summary>
		/// Prepares for segue.
		/// </summary>
		/// <remarks>
		/// The prepareForSegue method is invoked whenever a segue is about to take place. 
		/// The new view controller has been loaded from the storyboard at this point but 
		/// it’s not visible yet, and we can use this opportunity to send data to it.
		/// </remarks>
		public override void PrepareForSegue (UIStoryboardSegue segue, NSObject sender)
		{
			if (segue.Identifier == "StockSegue") { // set in Storyboard
				var navctlr = segue.DestinationViewController as StockDetailViewController;
				if (navctlr != null) {
					var source = TableView.Source as RootTableSource;
					var rowPath = TableView.IndexPathForSelectedRow;
					var item = source.GetItem(rowPath.Row);
					navctlr.SetStock(this, item);
				}
			}
		}
		
		public void EditStock () {
			// then open the detail view to edit it
			var detail = Storyboard.InstantiateViewController("detail") as StockDetailViewController;
			detail.Delegate = this;
			NavigationController.PushViewController (detail, true);
			
			// Could to this instead of the above, but need to create 'new Stock()' in PrepareForSegue()
			//this.PerformSegue ("StockSegue", this);
		}
		public void SaveStock (Stock stock) {
			Console.WriteLine("Save "+stock.Name);
			AppDelegate.Database.SaveStock (stock);
			NavigationController.PopViewController(true);
		}
		public void DeleteStock (Stock stock) {
			Console.WriteLine("Delete "+stock.Name);
			AppDelegate.Database.DeleteStock (stock);
			NavigationController.PopViewController(true);
		}
	}
}