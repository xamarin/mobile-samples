
using Android.App;
using Android.OS;
using Android.Widget;
using Android.Graphics;
using Android.Views;

using Tasky.BL;

namespace TaskyAndroid.Screens {

	[Activity (Label = "Task Details")]			
	public class TaskDetailsScreen : Activity {
		protected TaskItem taskItem = new TaskItem();
		protected Button cancelDeleteButton = null;
		protected EditText notesTextEdit = null;
		protected EditText nameTextEdit = null;
		protected Button saveButton = null;
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
				taskItem = Tasky.BL.Managers.TaskItemManager.GetTask(taskID);
			}
			
			// set our layout to be the home screen
			SetContentView(Resource.Layout.TaskDetails);
			nameTextEdit = FindViewById<EditText>(Resource.Id.txtName);
			notesTextEdit = FindViewById<EditText>(Resource.Id.txtNotes);
			saveButton = FindViewById<Button>(Resource.Id.btnSave);
			doneCheckbox = FindViewById<CheckBox>(Resource.Id.chkDone);
			
			// find all our controls
			cancelDeleteButton = FindViewById<Button>(Resource.Id.btnCancelDelete);
			
			
			// set the cancel delete based on whether or not it's an existing task
			cancelDeleteButton.Text = (taskItem.ID == 0 ? "Cancel" : "Delete");
			
			// name
			nameTextEdit.Text = taskItem.Name;
			
			// notes
			notesTextEdit.Text = taskItem.Notes;

			// done
			doneCheckbox.Checked = taskItem.Done;

			// button clicks 
			cancelDeleteButton.Click += (sender, e) => { CancelDelete(); };
			saveButton.Click += (sender, e) => { Save(); };
		}

		protected void Save()
		{
			taskItem.Name = nameTextEdit.Text;
			taskItem.Notes = notesTextEdit.Text;
			taskItem.Done = doneCheckbox.Checked;
			Tasky.BL.Managers.TaskItemManager.SaveTask(taskItem);
			Finish();
		}
		
		protected void CancelDelete()
		{
			if(taskItem.ID != 0) {
				Tasky.BL.Managers.TaskItemManager.DeleteTask(taskItem.ID);
			}
			Finish();
		}
	}
}