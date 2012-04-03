using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.UIKit;
using MonoTouch.Dialog;
using Tasky.AL;
using Tasky.BL;

namespace Tasky.Screens.iPhone.Home {
	public class controller_iPhone : DialogViewController {
		List<Task> tasks;
		TaskDetails.Screen detailsScreen = null;
		
		public controller_iPhone () : base (UITableViewStyle.Plain, null)
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
			tasks = BL.Managers.TaskManager.GetTasks().ToList ();
			Root = new RootElement("Tasky") {
				new Section() {
					from t in tasks
					select (Element) new StringElement(t.Name, t.Notes)
				}
			}; 
		}
		public override void Selected (MonoTouch.Foundation.NSIndexPath indexPath)
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