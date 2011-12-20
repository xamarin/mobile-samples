using System;
using MonoTouch.UIKit;
using System.Collections.Generic;
using MWC.BL;
using MWC.iOS;

namespace MWC.AL
{
	// TODO: this should group by timeslot and just return the next two (now and up-next)
	public class HomeTableSource : UITableViewSource
	{	
		public event EventHandler<SessionClickedEventArgs> SessionClicked;
		public event EventHandler<SessionClickedEventArgs> SessionDeleted;

		protected string _cellId = "SessionCell";
		
		public IList<Session> Session
		{
			get { return this._tasks; }
			set { this._tasks = value; }
		}
		protected IList<Session> _tasks = new List<Session>();
		
		public HomeTableSource (IList<Session> tasks) : base ()
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
			
			cell.TextLabel.Text = this._tasks[indexPath.Row].Title;
			cell.DetailTextLabel.Text = this._tasks[indexPath.Row].Start.ToString ("H:mm");
			
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
			this.RaiseSessionClicked(indexPath.Row);
			tableView.DeselectRow(indexPath,true);
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
				this.RaiseSessionDeleted(indexPath.Row);
			}
		}
	
		protected void RaiseSessionClicked(int row)
		{
			// raise the event in a thread-safe way
			var handler = this.SessionClicked;
			if(handler != null)
			{
				handler(this, new SessionClickedEventArgs(this._tasks[row]));
			}	
		}
		
		protected void RaiseSessionDeleted(int row)
		{
			// raise the event in a thread-safe way
			var handler = this.SessionDeleted;
			if(handler != null)
			{
				handler(this, new SessionClickedEventArgs(this._tasks[row]));
			}	
		}
	}
}

