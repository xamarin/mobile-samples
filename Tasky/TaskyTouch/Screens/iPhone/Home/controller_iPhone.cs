using System;
using MonoTouch.UIKit;
using Tasky.AL;
using Tasky.BL;

namespace Tasky.Screens.iPhone.Home {
	public class controller_iPhone : UITableViewController {
		AL.TaskTableSource tableSource = null;
		TaskDetails.Screen detailsScreen = null;
		
		public controller_iPhone () : base ()
		{
			Initialize ();
			Title = "Tasky";
		}
		
		protected void Initialize()
		{
			NavigationItem.SetRightBarButtonItem (new UIBarButtonItem (UIBarButtonSystemItem.Add), false);
			NavigationItem.RightBarButtonItem.Clicked += (sender, e) => { ShowTaskDetails(new Task()); };
		}
		
		protected void ShowTaskDetails(Task task)
		{
			detailsScreen = new Tasky.Screens.iPhone.TaskDetails.Screen(task);
			NavigationController.PushViewController(detailsScreen, true);
		}
		
		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			
			// reload/refresh
			PopulateTable();			
		}
		
		protected void PopulateTable()
		{
			tableSource = new Tasky.AL.TaskTableSource(BL.Managers.TaskManager.GetTasks());
			tableSource.TaskDeleted += (object sender, TaskClickedEventArgs e) => { DeleteTaskRow(e.Task.ID); };
			tableSource.TaskClicked += (object sender, TaskClickedEventArgs e) => { ShowTaskDetails(e.Task); };
			TableView.Source = tableSource;					
		}
		
		protected void DeleteTaskRow(int id)
		{
			BL.Managers.TaskManager.DeleteTask(id);
			PopulateTable();
		}
	}
}