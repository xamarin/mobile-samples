using MonoTouch.UIKit;
using System.Drawing;
using System;
using MonoTouch.Foundation;
using Tasky.BL;

namespace Tasky.Screens.iPhone.TaskDetails {
	public partial class Screen : UIViewController {
		protected Task task;
		
		public Screen (Task task) : base ("Screen", null)
		{
			this.task = task;
		}
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			Title = "Details";
			
			// set the cancel delete based on whether or not it's an existing task
			btnCancelDelete.SetTitle((task.ID == 0 ? "Cancel" : "Delete"), UIControlState.Normal);
			
			txtName.Text = task.Name;
			txtNotes.Text = task.Notes;
			
			btnCancelDelete.TouchUpInside += (sender, e) => { CancelDelete(); };
			btnSave.TouchUpInside += (sender, e) => { Save(); };
			
			txtName.ReturnKeyType = UIReturnKeyType.Next;
			txtName.ShouldReturn += (t) => { txtNotes.BecomeFirstResponder(); return true; };
			
			txtNotes.ReturnKeyType = UIReturnKeyType.Done;
			txtNotes.ShouldReturn += (t) => { txtNotes.ResignFirstResponder(); return true; };
		}
				
		protected void Save()
		{
			task.Name = txtName.Text;
			task.Notes = txtNotes.Text;
			BL.Managers.TaskManager.SaveTask(task);
			NavigationController.PopViewControllerAnimated(true);
		}
		
		protected void CancelDelete()
		{
			if(task.ID != 0) {
				BL.Managers.TaskManager.DeleteTask(task.ID);
			}
			
			NavigationController.PopViewControllerAnimated(true);
		}
	}
}