using System;
using MonoTouch.UIKit;
using Tasky.AL;
using Tasky.BL;

namespace Tasky.Screens.iPhone.Home
{
	public class controller_iPhone : UITableViewController
	{
		AL.TaskTableSource _tableSource = null;
		TaskDetails.Screen _detailsScreen = null;
		
		public controller_iPhone () : base ()
		{
			this.Initialize ();
			this.Title = "Tasky";
		}
		
		protected void Initialize()
		{
			this.NavigationItem.SetRightBarButtonItem (new UIBarButtonItem (UIBarButtonSystemItem.Add), false);
			this.NavigationItem.RightBarButtonItem.Clicked += (sender, e) => { this.ShowTaskDetails(new Task()); };
		}
		
		protected void ShowTaskDetails(Task task)
		{
			this._detailsScreen = new Tasky.Screens.iPhone.TaskDetails.Screen(task);
			this.NavigationController.PushViewController(this._detailsScreen, true);
		}
		
		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			
			// reload/refresh
			this.PopulateTable();			
		}
		
		protected void PopulateTable()
		{
			this._tableSource = new Tasky.AL.TaskTableSource(BL.Managers.TaskManager.GetTasks());
			this._tableSource.TaskDeleted += (object sender, TaskClickedEventArgs e) => { this.DeleteTaskRow(e.Task.ID); };
			this._tableSource.TaskClicked += (object sender, TaskClickedEventArgs e) => { this.ShowTaskDetails(e.Task); };
			this.TableView.Source = this._tableSource;					
		}
		
		protected void DeleteTaskRow(int id)
		{
			BL.Managers.TaskManager.DeleteTask(id);
			this.PopulateTable();
		}
		
	}
}

