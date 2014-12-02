using System;
using System.Collections.Generic;
using System.Linq;
using UIKit;
using MonoTouch.Dialog;
using Foundation;
using Tasky.AL;
using Tasky.BL;

namespace Tasky.Screens {
	public class controller_iPhone : DialogViewController {
		List<Task> tasks;
		
		public controller_iPhone () : base (UITableViewStyle.Plain, null)
		{
			Initialize ();
		}
		
		protected void Initialize()
		{
			NavigationItem.SetRightBarButtonItem (new UIBarButtonItem (UIBarButtonSystemItem.Add), false);
			NavigationItem.RightBarButtonItem.Clicked += (sender, e) => { ShowTaskDetails(new Task()); };
		}
		

		// MonoTouch.Dialog individual TaskDetails view (uses /AL/TaskDialog.cs wrapper class)
		LocalizableBindingContext context;
		TaskDialog taskDialog;
		Task currentTask;
		DialogViewController detailsScreen;
		protected void ShowTaskDetails (Task task)
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
			BL.Managers.TaskManager.SaveTask(currentTask);
			NavigationController.PopViewController (true);
			//context.Dispose (); // per documentation
		}
		public void DeleteTask ()
		{
			if (currentTask.ID >= 0)
				BL.Managers.TaskManager.DeleteTask (currentTask.ID);
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
			tasks = BL.Managers.TaskManager.GetTasks ().ToList ();
			var newTask = NSBundle.MainBundle.LocalizedString ("<new task>", "<new task>");
			Root = new RootElement ("Tasky") {
				new Section() {
					from t in tasks
					select (Element) new CheckboxElement((t.Name == "" ? newTask : t.Name), t.Done)
				}
			}; 
		}
		public override void Selected (NSIndexPath indexPath)
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
			BL.Managers.TaskManager.DeleteTask(tasks[rowId].ID);
		}
	}
}