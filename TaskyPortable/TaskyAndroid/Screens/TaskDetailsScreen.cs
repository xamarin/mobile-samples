using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using Android.Graphics;
using Android.Views;

using Tasky.BL;
using Android.Speech.Tts;

namespace TaskyAndroid.Screens {
	//TODO: implement proper lifecycle methods
	[Activity (Label = "Task Details", Icon="@drawable/ic_launcher", Theme = "@style/AppTheme")]			
	public class TaskDetailsScreen : Activity, TextToSpeech.IOnInitListener {
		protected Task task = new Task();
		protected Button cancelDeleteButton;
		protected EditText notesTextEdit;
		protected EditText nameTextEdit;
		protected Button saveButton;
		protected Button speakButton;
		CheckBox doneCheckbox;
		
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			
			View titleView = Window.FindViewById(Android.Resource.Id.Title);
			if (titleView != null) {
			  IViewParent parent = titleView.Parent;
			  if (parent != null && (parent is View)) {
			    View parentView = (View)parent;
			    parentView.SetBackgroundColor(Color.Rgb(0x26, 0x75 ,0xFF)); //38, 117 ,255
			  }
			}

			int taskID = Intent.GetIntExtra("TaskID", 0);
			if(taskID > 0) {
                task = TaskyApp.Current.TaskMgr.GetTask(taskID);
			}
			
			// set our layout to be the home screen
			SetContentView(Resource.Layout.TaskDetails);
			nameTextEdit = FindViewById<EditText>(Resource.Id.txtName);
			notesTextEdit = FindViewById<EditText>(Resource.Id.txtNotes);
			saveButton = FindViewById<Button>(Resource.Id.btnSave);
			doneCheckbox = FindViewById<CheckBox>(Resource.Id.chkDone);
			cancelDeleteButton = FindViewById<Button>(Resource.Id.btnCancelDelete);
			speakButton = FindViewById<Button> (Resource.Id.btnSpeak);

			// set the cancel delete based on whether or not it's an existing task
			cancelDeleteButton.Text = (task.ID == 0 ? "Cancel" : "Delete");
			// name
			nameTextEdit.Text = task.Name; 
			// notes
			notesTextEdit.Text = task.Notes; 
			// done
			doneCheckbox.Checked = task.Done; 

			// button clicks 
			cancelDeleteButton.Click += (sender, e) => { CancelDelete(); };
			saveButton.Click += (sender, e) => { Save(); };
			speakButton.Click += (sender, e) => {
				Speak(nameTextEdit.Text + ". " + notesTextEdit.Text);
			};
			speaker = new TextToSpeech (this, this);
		}

		void Speak(string text)
		{
			var p = new Dictionary<string,string> ();
			speaker.Speak (text, QueueMode.Flush, p);
		}

		#region IOnInitListener implementation
		TextToSpeech speaker;
		public void OnInit (OperationResult status)
		{
			if (status.Equals (OperationResult.Success))
				Console.WriteLine ("spoke");
			else
				Console.WriteLine ("was quiet");
		}
		#endregion

		protected void Save()
		{
			task.Name = nameTextEdit.Text;
			task.Notes = notesTextEdit.Text;
			task.Done = doneCheckbox.Checked;
            TaskyApp.Current.TaskMgr.SaveTask(task);
			Finish();
		}
		
		protected void CancelDelete()
		{
			if(task.ID != 0) {
                TaskyApp.Current.TaskMgr.DeleteTask(task.ID);
			}
			Finish();
		}
	}
}