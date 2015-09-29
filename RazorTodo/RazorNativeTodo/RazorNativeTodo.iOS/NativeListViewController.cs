using System;
using System.Linq;
using UIKit;
using System.Collections.Generic;
using Foundation;

namespace RazorNativeTodo
{
	public class NativeListViewController: UITableViewController
	{
		List<TodoItem> model;
		RazorViewController webViewController;

		public NativeListViewController () : base (UITableViewStyle.Plain)
		{
			Title = "HybridNativeTodo";
			webViewController = new RazorViewController ();

			NavigationItem.SetRightBarButtonItem (new UIBarButtonItem (UIBarButtonSystemItem.Add), false);
			NavigationItem.RightBarButtonItem.Clicked += (sender, e) => { 
				var template = new TodoView () { Model = new TodoItem() };
				var page = template.GenerateString ();
				webViewController.webView.LoadHtmlString (page, NSBundle.MainBundle.BundleUrl);
				NavigationController.PushViewController(webViewController, true);
			};

			NavigationItem.SetLeftBarButtonItem (new UIBarButtonItem (UIBarButtonSystemItem.Play), false);
			NavigationItem.LeftBarButtonItem.Clicked += (sender, e) => { 
				var todos = App.Database.GetItemsNotDone ();
				var tospeak = "";
				foreach (var t in todos)
					tospeak += t.Name + " ";
				if (tospeak == "")
					tospeak = "there are no tasks to do";
				Speech.Speak (tospeak);
			};
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			model = App.Database.GetItems ().ToList ();
			TableView.Source = new TaskDataSource (model, webViewController, this);
		}
	}

	public class TaskDataSource : UITableViewSource
	{
		IList<TodoItem> taskList;
		RazorViewController webViewController;
		UIViewController viewController;
		public TaskDataSource (IList<TodoItem> tasks, RazorViewController webViewController, UIViewController viewController) {
			taskList = tasks;
			this.webViewController = webViewController;
			this.viewController = viewController;
		}

		public override nint RowsInSection (UITableView tableview, nint section) {
			return taskList.Count;
		}

		public override UITableViewCell GetCell (UITableView tableView, Foundation.NSIndexPath indexPath)
		{
			var cell = tableView.DequeueReusableCell ("todocell");
			if (cell == null)
				cell = new UITableViewCell (UITableViewCellStyle.Default, "todocell");

			cell.TextLabel.Text = taskList [indexPath.Row].Name;

			if (taskList [indexPath.Row].Done)
				cell.Accessory = UITableViewCellAccessory.Checkmark;
			else
				cell.Accessory = UITableViewCellAccessory.None;

			return cell;
		}

		public TodoItem GetItem(int id) {
			return taskList[id];
		}

		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			var id = taskList [indexPath.Row].ID;
			var model = App.Database.GetItem (Convert.ToInt32 (id));
			var template = new TodoView () { Model = model };
			var page = template.GenerateString ();
			webViewController.webView.LoadHtmlString (page, NSBundle.MainBundle.BundleUrl);
			viewController.NavigationController.PushViewController(webViewController, true);
		}
	}
}

