using System;
using MonoTouch.UIKit;
using System.Collections.Generic;
using Tasky.BL;

namespace Tasky.AL
{
	public class TaskTableSource : UITableViewSource
	{
		public event EventHandler<TaskClickedEventArgs> TaskClicked;
		public event EventHandler<TaskClickedEventArgs> TaskDeleted;
			
		protected string _cellId = "TaskCell";
		
		
		public IList<Task> Tasks
		{
			get { return this._tasks; }
			set { this._tasks = value; }
		}
		protected IList<Task> _tasks = new List<Task>();
		
		public TaskTableSource (IList<Task> tasks) : base ()
		{
			this._tasks = tasks;
		}
		
		public override UITableViewCell GetCell (UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
		{
			UITableViewCell cell = tableView.DequeueReusableCell(this._cellId);
			
			if(cell == null)
			{
				cell = new UITableViewCell(UITableViewCellStyle.Value1, this._cellId);
			}
			
			cell.TextLabel.Text = this._tasks[indexPath.Row].Name;
			cell.DetailTextLabel.Text = this._tasks[indexPath.Row].Notes;
			
			return cell;
		}
		
		public override int RowsInSection (UITableView tableview, int section)
		{
			return this._tasks.Count;
		}
		
		public override int NumberOfSections (UITableView tableView)
		{
			return 1;
		}
		
		public override void RowSelected (UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
		{
			this.RaiseTaskClicked(indexPath.Row);
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
			if(editingStyle == UITableViewCellEditingStyle.Delete)
			{
				this.RaiseTaskDeleted(indexPath.Row);
			}
		}
	
		protected void RaiseTaskClicked(int row)
		{
			// raise the event in a thread-safe way
			var handler = this.TaskClicked;
			if(handler != null)
			{
				handler(this, new TaskClickedEventArgs(this._tasks[row]));
			}	
		}
		
		protected void RaiseTaskDeleted(int row)
		{
			// raise the event in a thread-safe way
			var handler = this.TaskDeleted;
			if(handler != null)
			{
				handler(this, new TaskClickedEventArgs(this._tasks[row]));
			}	
		}
	}
}

