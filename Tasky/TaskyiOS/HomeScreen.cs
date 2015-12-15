using System;
using System.Collections.Generic;
using System.Linq;
using UIKit;
using MonoTouch.Dialog;
using Tasky.Shared;
using Tasky.ApplicationLayer;

namespace Tasky.Screens {

	/// <summary>
	/// A UITableViewController that uses MonoTouch.Dialog - displays the list of Tasks
	/// </summary>
	public class HomeScreen : DialogViewController {
		// 
		List<TodoItem> tasks;
		
		// MonoTouch.Dialog individual TaskDetails view (uses /ApplicationLayer/TaskDialog.cs wrapper class)
		BindingContext context;
		TodoItemDialog taskDialog;
		TodoItem currentItem;
		DialogViewController detailsScreen;

		public HomeScreen () : base (UITableViewStyle.Plain, null)
		{
			Initialize ();
			UITableView.Appearance.TintColor = UIColor.FromRGB (0x6F, 0xA2, 0x2E);
		}
		
		protected void Initialize()
		{
			NavigationItem.SetRightBarButtonItem (new UIBarButtonItem (UIBarButtonSystemItem.Add), false);
			NavigationItem.RightBarButtonItem.Clicked += (sender, e) => { ShowTaskDetails(new TodoItem()); };
		}
		
		protected void ShowTaskDetails(TodoItem item)
		{
			currentItem = item;
			taskDialog = new TodoItemDialog (currentItem);
			context = new BindingContext (this, taskDialog, "Task Details");
			detailsScreen = new DialogViewController (context.Root, true);
			ActivateController(detailsScreen);
		}
		public void SaveTask()
		{
			context.Fetch (); // re-populates with updated values
			currentItem.Name = taskDialog.Name;
			currentItem.Notes = taskDialog.Notes;
			// TODO: show the completion status in the UI
			currentItem.Done = taskDialog.Done;
			TodoItemManager.SaveTask(currentItem);
			NavigationController.PopViewController (true);
		}
		public void DeleteTask ()
		{
			if (currentItem.ID >= 0)
				TodoItemManager.DeleteTask (currentItem.ID);
			NavigationController.PopViewController (true);
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			
			// reload/refresh
			PopulateTable();			
		}
		
		protected void PopulateTable()
		{
			tasks = TodoItemManager.GetTasks().ToList ();
//			var rows = from t in tasks
//				select (Element)new StringElement ((t.Name == "" ? "<new task>" : t.Name), t.Notes);
			// TODO: use this element, which displays a 'tick' when item is completed
			var rows = from t in tasks
				select (Element)new CheckboxElement ((t.Name == "" ? "<new task>" : t.Name), t.Done);
			var s = new Section ();
			s.AddAll(rows);
			Root = new RootElement("Tasky") {s}; 
		}
		public override void Selected (Foundation.NSIndexPath indexPath)
		{
			var todoItem = tasks[indexPath.Row];
			ShowTaskDetails(todoItem);
		}
		public override Source CreateSizingSource (bool unevenRows)
		{
			return new EditingSource (this);
		}
		public void DeleteTaskRow(int rowId)
		{
			TodoItemManager.DeleteTask(tasks[rowId].ID);
		}
	}
}