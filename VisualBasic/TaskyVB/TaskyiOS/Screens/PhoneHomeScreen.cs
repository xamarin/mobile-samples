using System;
using System.Collections.Generic;
using System.Linq;
using UIKit;
using MonoTouch.Dialog;
using Tasky.AL;
using TaskyVBNetStandard;

namespace Tasky.Screens.iPhone.Home {
	public class PhoneHomeScreen : DialogViewController {
		List<TodoItem> tasks;
		
		public PhoneHomeScreen () : base (UITableViewStyle.Plain, null)
		{
			Initialize ();
		}
		
		protected void Initialize()
		{
			NavigationItem.SetRightBarButtonItem (new UIBarButtonItem (UIBarButtonSystemItem.Add), false);
			NavigationItem.RightBarButtonItem.Clicked += (sender, e) => { ShowTaskDetails(new TodoItem()); };
		}
		

		// MonoTouch.Dialog individual TaskDetails view (uses /AL/TaskDialog.cs wrapper class)
		LocalizableBindingContext context;
		TaskDialog taskDialog;
		TodoItem currentTask;
		DialogViewController detailsScreen;
		protected void ShowTaskDetails (TodoItem task)
		{
			currentTask = task;
			taskDialog = new TaskDialog (task);
			
			var title = Foundation.NSBundle.MainBundle.LocalizedString ("Task Details", "Task Details");
			context = new LocalizableBindingContext (this, taskDialog, title);
			detailsScreen = new DialogViewController (context.Root, true);
			ActivateController(detailsScreen);
		}
		public void SaveTask()
		{
			context.Fetch (); // re-populates with updated values
			currentTask.Name = taskDialog.Name;
			currentTask.Notes = taskDialog.Notes;
			currentTask.Done = taskDialog.Done;
			AppDelegate.Current.TaskMgr.SaveTask(currentTask);
			NavigationController.PopViewController (true);
			//context.Dispose (); // per documentation
		}
		public void DeleteTask ()
		{
			if (currentTask.ID >= 0)
				AppDelegate.Current.TaskMgr.DeleteTask (currentTask.ID);
			NavigationController.PopViewController (true);
		}



		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			
			// reload/refresh
			PopulateTable();			
		}
		
		protected void PopulateTable ()
		{
			tasks = AppDelegate.Current.TaskMgr.GetTasks ().ToList ();
			var newTask = Foundation.NSBundle.MainBundle.LocalizedString ("<new task>", "<new task>");
			Root = new RootElement ("Tasky") {
				new Section() {
					from t in tasks
					select (Element) new CheckboxElement((t.Name==""?newTask:t.Name), t.Done)
				}
			}; 
		}
		public override void Selected (Foundation.NSIndexPath indexPath)
		{
			var task = tasks[indexPath.Row];
			ShowTaskDetails(task);
		}
		public override Source CreateSizingSource (bool unevenRows)
		{
			return new EditingSource (this);
		}
		public void DeleteTaskRow(int rowId)
		{
			AppDelegate.Current.TaskMgr.DeleteTask(tasks[rowId].ID);
		}
	}
}