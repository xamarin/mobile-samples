using System;
using System.Collections.Generic;
using System.Linq;
using UIKit;
using MonoTouch.Dialog;
using Foundation;
using Tasky.AL;
using Tasky.BL;

namespace Tasky.Screens.iPhone {
	public class HomeScreen : DialogViewController {
		List<Task> tasks;
		
		public HomeScreen () : base (UITableViewStyle.Plain, null)
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
			
			var title = NSBundle.MainBundle.LocalizedString ("Task Details", "Task Details");
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
		//	context.Dispose (); // documentation suggests this is required, but appears to cause a crash sometimes
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
			var newTaskDefaultName = NSBundle.MainBundle.LocalizedString ("<new task>", "<new task>");
			// make into a list of MT.D elements to display
			List<Element> le = new List<Element>();
			foreach (var t in tasks) {
				le.Add (new CheckboxElement((t.Name == "" ? newTaskDefaultName : t.Name), t.Done));
			}
			// add to section
			var s = new Section ();
			s.AddAll (le);
			// add as root
			Root = new RootElement ("Tasky") { s }; 
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
			BL.Managers.TaskManager.DeleteTask(tasks[rowId].ID);
		}
	}
}