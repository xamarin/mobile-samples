using System;
using MonoTouch.UIKit;
using System.Collections.Generic;
using Tasky.BL;

namespace Tasky.AL {
	public class TaskTableSource : UITableViewSource {
		public event EventHandler<TaskClickedEventArgs> TaskClicked;
		public event EventHandler<TaskClickedEventArgs> TaskDeleted;
			
		protected string cellId = "TaskCell";
		
		
		public IList<Task> Tasks
		{
			get { return tasks; }
			set { tasks = value; }
		}
		protected IList<Task> tasks = new List<Task>();
		
		public TaskTableSource (IList<Task> tasks) : base ()
		{
			this.tasks = tasks;
		}
		
		public override UITableViewCell GetCell (UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
		{
			UITableViewCell cell = tableView.DequeueReusableCell(cellId);
			
			if(cell == null) {
				cell = new UITableViewCell(UITableViewCellStyle.Value1, cellId);
			}
			
			cell.TextLabel.Text = tasks[indexPath.Row].Name;
			cell.DetailTextLabel.Text = tasks[indexPath.Row].Notes;
			
			return cell;
		}
		
		public override int RowsInSection (UITableView tableview, int section)
		{
			return tasks.Count;
		}
		
		public override int NumberOfSections (UITableView tableView)
		{
			return 1;
		}
		
		public override void RowSelected (UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
		{
			RaiseTaskClicked(indexPath.Row);
		}
		
		public override bool CanEditRow (UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
		{
			 return true;
		}
	
		public override UITableViewCellEditingStyle EditingStyleForRow (UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
		{
			return UITableViewCellEditingStyle.Delete;
		}

		public override void CommitEditingStyle (UITableView tableView, UITableViewCellEditingStyle editingStyle, MonoTouch.Foundation.NSIndexPath indexPath)
		{
			if(editingStyle == UITableViewCellEditingStyle.Delete) {
				RaiseTaskDeleted(indexPath.Row);
			}
		}
	
		protected void RaiseTaskClicked(int row)
		{
			// raise the event in a thread-safe way
			var handler = TaskClicked;
			if(handler != null) {
				handler(this, new TaskClickedEventArgs(tasks[row]));
			}	
		}
		
		protected void RaiseTaskDeleted(int row)
		{
			// raise the event in a thread-safe way
			var handler = TaskDeleted;
			if(handler != null) {
				handler(this, new TaskClickedEventArgs(tasks[row]));
			}	
		}
	}
}

