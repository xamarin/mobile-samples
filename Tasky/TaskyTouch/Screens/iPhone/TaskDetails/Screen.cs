using MonoTouch.UIKit;
using System.Drawing;
using System;
using MonoTouch.Foundation;
using Tasky.BL;

namespace Tasky.Screens.iPhone.TaskDetails
{
	public partial class Screen : UIViewController
	{
		protected Task _task;
		
		public Screen (Task task) : base ("Screen", null)
		{
			this._task = task;
		}
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			this.Title = "Details";
			
			// set the cancel delete based on whether or not it's an existing task
			this.btnCancelDelete.SetTitle((this._task.ID == 0 ? "Cancel" : "Delete"), UIControlState.Normal);
			
			this.txtName.Text = this._task.Name;
			this.txtNotes.Text = this._task.Notes;
			
			this.btnCancelDelete.TouchUpInside += (sender, e) => { this.CancelDelete(); };
			this.btnSave.TouchUpInside += (sender, e) => { this.Save(); };
			
			this.txtName.ReturnKeyType = UIReturnKeyType.Next;
			this.txtName.ShouldReturn += (t) => { this.txtNotes.BecomeFirstResponder(); return true; };
			
			this.txtNotes.ReturnKeyType = UIReturnKeyType.Done;
			this.txtNotes.ShouldReturn += (t) => { this.txtNotes.ResignFirstResponder(); return true; };
		}
				
		protected void Save()
		{
			this._task.Name = this.txtName.Text;
			this._task.Notes = this.txtNotes.Text;
			BL.Managers.TaskManager.SaveTask(this._task);
			this.NavigationController.PopViewControllerAnimated(true);
		}
		
		protected void CancelDelete()
		{
			if(this._task.ID != 0)
			{
				BL.Managers.TaskManager.DeleteTask(this._task.ID);
			}
			
			this.NavigationController.PopViewControllerAnimated(true);
		}
	}
}

