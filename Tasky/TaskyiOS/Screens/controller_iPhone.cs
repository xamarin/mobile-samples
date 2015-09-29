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
		List<TaskItem> taskItems;
		
		public controller_iPhone () : base (UITableViewStyle.Plain, null)
		{
			Initialize ();
		}
		
		protected void Initialize()
		{
			Root = new RootElement ("Tasky");
			NavigationItem.SetRightBarButtonItem (new UIBarButtonItem (UIBarButtonSystemItem.Add), false);
			NavigationItem.RightBarButtonItem.Clicked += (sender, e) => { ShowTaskDetails(new TaskItem()); };
		}
		

		// MonoTouch.Dialog individual TaskDetails view (uses /AL/TaskDialog.cs wrapper class)
		LocalizableBindingContext context;
		TaskDialog taskItemDialog;
		TaskItem currentTaskItem;
		DialogViewController detailsScreen;
		protected void ShowTaskDetails (TaskItem taskItem)
		{
			currentTaskItem = taskItem;
			taskItemDialog = new TaskDialog (taskItem);
			
			var title = Foundation.NSBundle.MainBundle.LocalizedString ("Task Details", "Task Details");
			context = new LocalizableBindingContext (this, taskItemDialog, title);
			detailsScreen = new DialogViewController (context.Root, true);
			ActivateController(detailsScreen);
		}
		public void SaveTask()
		{
			context.Fetch (); // re-populates with updated values
			currentTaskItem.Name = taskItemDialog.Name;
			currentTaskItem.Notes = taskItemDialog.Notes;
			currentTaskItem.Done = taskItemDialog.Done;
			BL.Managers.TaskItemManager.SaveTask(currentTaskItem);
			NavigationController.PopViewController (true);
			//context.Dispose (); // per documentation
		}
		public void DeleteTask ()
		{
			if (currentTaskItem.ID >= 0)
				BL.Managers.TaskItemManager.DeleteTask (currentTaskItem.ID);
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
			taskItems = BL.Managers.TaskItemManager.GetTasks ().ToList ();
			var newTask = NSBundle.MainBundle.LocalizedString ("<new task>", "<new task>");
				
			Root.Clear ();
			Root.Add (new Section() {
				from t in taskItems
				select (Element) new CheckboxElement((t.Name == "" ? newTask : t.Name), t.Done)
			});
		}
		public override void Selected (NSIndexPath indexPath)
		{
			var task = taskItems[indexPath.Row];
			ShowTaskDetails(task);
		}
		public override Source CreateSizingSource (bool unevenRows)
		{
			return new EditingSource (this);
		}
		public void DeleteTaskRow(int rowId)
		{
			BL.Managers.TaskItemManager.DeleteTask(taskItems[rowId].ID);
		}
	}
}