using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;

using Tasky.BL;
using Tasky.BL.Managers;
using Tasky.Droid.Adapters;

namespace Tasky.Droid.Screens
{
    [Activity(Label = "Tasky Pro", MainLauncher = true)]
    public class HomeScreen : Activity
    {
        protected TaskListAdapter taskList;
        protected ListView taskListView = null;
        protected IList<Task> tasks;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Enable the ActionBar
            RequestWindowFeature(WindowFeatures.ActionBar);

            // set our layout to be the home screen
            SetContentView(Resource.Layout.HomeScreen);

            //Find our controls
            taskListView = FindViewById<ListView>(Resource.Id.lstTasks);

            // wire up task click handler
            if (taskListView != null)
            {
                taskListView.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) =>{
                                              Intent taskDetails = new Intent(this, typeof(TaskDetailsScreen));
                                              taskDetails.PutExtra("TaskID", tasks[e.Position].ID);
                                              StartActivity(taskDetails);
                                          };
            }
        }

        protected override void OnResume()
        {
            base.OnResume();

            tasks = TaskManager.GetTasks();

            // create our adapter
            taskList = new TaskListAdapter(this, tasks);

            //Hook up our adapter to our ListView
            taskListView.Adapter = taskList;
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            // Create the actions in the ActionBar.
            MenuInflater.Inflate(Resource.Menu.menu_homescreen, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.menu_add_task:
                    // The user has tapped the add task button
                    StartActivity(typeof(TaskDetailsScreen));
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }
    }
}
