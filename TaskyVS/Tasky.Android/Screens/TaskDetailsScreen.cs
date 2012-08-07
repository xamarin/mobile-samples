using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using Tasky.BL;

namespace TaskyAndroid.Screens
{
	//TODO: implement proper lifecycle methods
	[Activity (Label = "TaskDetailsScreen")]			
	public class TaskDetailsScreen : Activity
	{
		protected Task _task = new Task();
		protected Button _cancelDeleteButton = null;
		protected EditText _notesTextEdit = null;
		protected EditText _nameTextEdit = null;
		protected Button _saveButton = null;
		
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			
			int taskID = Intent.GetIntExtra("TaskID", 0);
			if(taskID > 0)
			{
				this._task = Tasky.BL.Managers.TaskManager.GetTask(taskID);
			}
			
			// set our layout to be the home screen
			this.SetContentView(Resource.Layout.TaskDetails);
			this._nameTextEdit = this.FindViewById<EditText>(Resource.Id.txtName);
			this._notesTextEdit = this.FindViewById<EditText>(Resource.Id.txtNotes);
			this._saveButton = this.FindViewById<Button>(Resource.Id.btnSave);
			
			// find all our controls
			this._cancelDeleteButton = this.FindViewById<Button>(Resource.Id.btnCancelDelete);
			
			
			// set the cancel delete based on whether or not it's an existing task
			if(this._cancelDeleteButton != null)
			{ this._cancelDeleteButton.Text = (this._task.ID == 0 ? "Cancel" : "Delete"); }
			
			// name
			if(this._nameTextEdit != null) { this._nameTextEdit.Text = this._task.Name; }
			
			// notes
			if(this._notesTextEdit != null) { this._notesTextEdit.Text = this._task.Notes; }
			
			// button clicks 
			this._cancelDeleteButton.Click += (sender, e) => { this.CancelDelete(); };
			this._saveButton.Click += (sender, e) => { this.Save(); };
		}

		protected void Save()
		{
			this._task.Name = this._nameTextEdit.Text;
			this._task.Notes = this._notesTextEdit.Text;
			Tasky.BL.Managers.TaskManager.SaveTask(this._task);
			this.Finish();
		}
		
		protected void CancelDelete()
		{
			if(this._task.ID != 0)
			{
				Tasky.BL.Managers.TaskManager.DeleteTask(this._task.ID);
			}
			this.Finish();
		}
	
	}
}

