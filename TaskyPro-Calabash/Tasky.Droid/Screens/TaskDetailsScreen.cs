using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;

using Tasky.BL;
using Tasky.BL.Managers;

namespace Tasky.Droid.Screens
{
    //TODO: implement proper lifecycle methods
    [Activity(Label = "Task Details")]
    public class TaskDetailsScreen : Activity
    {
        CheckBox doneCheckbox;
        protected EditText nameTextEdit;
        protected EditText notesTextEdit;
        protected Task task = new Task();

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            RequestWindowFeature(WindowFeatures.ActionBar);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetHomeButtonEnabled(true);

            int taskID = Intent.GetIntExtra("TaskID", 0);
            if (taskID > 0)
            {
                task = TaskManager.GetTask(taskID);
            }

            // set our layout to be the home screen
            SetContentView(Resource.Layout.TaskDetails);
            nameTextEdit = FindViewById<EditText>(Resource.Id.txtName);
            notesTextEdit = FindViewById<EditText>(Resource.Id.txtNotes);
            doneCheckbox = FindViewById<CheckBox>(Resource.Id.chkDone);

            // name
            if (nameTextEdit != null)
            {
                nameTextEdit.Text = task.Name;
            }

            // notes
            if (notesTextEdit != null)
            {
                notesTextEdit.Text = task.Notes;
            }

            if (doneCheckbox != null)
            {
                doneCheckbox.Checked = task.Done;
            }
        }

        protected void Save()
        {
            task.Name = nameTextEdit.Text;
            task.Notes = notesTextEdit.Text;
            task.Done = doneCheckbox.Checked;
            TaskManager.SaveTask(task);
            Finish();
        }

        protected void CancelDelete()
        {
            if (task.ID != 0)
            {
                TaskManager.DeleteTask(task.ID);
            }
            Finish();
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_detailsscreen, menu);

            IMenuItem menuItem = menu.FindItem(Resource.Id.menu_delete_task);
            menuItem.SetTitle(task.ID == 0 ? "Cancel" : "Delete");

            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.menu_save_task:
                    Save();
                    return true;

                case Resource.Id.menu_delete_task:
                    CancelDelete();
                    return true;

                default:
                    Finish();
                    return base.OnOptionsItemSelected(item);
            }
        }
    }
}
