using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Tasky.BL;
using Android.Graphics;
using Android.Views;

namespace TaskyAndroid.Screens {
	[Activity (Label = "TaskyPro", MainLauncher = true, Icon="@drawable/ic_launcher", Theme = "@style/AppTheme")]			
	public class HomeScreen : Activity {
		protected Adapters.TaskListAdapter taskList;
		protected IList<Task> tasks;
		protected Button addTaskButton = null;
		protected ListView taskListView = null;
		
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


			// set our layout to be the home screen
			SetContentView(Resource.Layout.HomeScreen);

			//Find our controls
			taskListView = FindViewById<ListView> (Resource.Id.lstTasks);
			addTaskButton = FindViewById<Button> (Resource.Id.btnAddTask);

			// wire up add task button handler
			if(addTaskButton != null) {
				addTaskButton.Click += (sender, e) => {
					StartActivity(typeof(TaskDetailsScreen));
				};
			}
			
			// wire up task click handler
			if(taskListView != null) {
				taskListView.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) => {
					var taskDetails = new Intent (this, typeof (TaskDetailsScreen));
					taskDetails.PutExtra ("TaskID", tasks[e.Position].ID);
					StartActivity (taskDetails);
				};
			}
		}
		
		protected override void OnResume ()
		{
			base.OnResume ();

			tasks = TaskyApp.Current.TaskMgr.GetTasks();
			
			// create our adapter
			taskList = new Adapters.TaskListAdapter(this, tasks);

			//Hook up our adapter to our ListView
			taskListView.Adapter = taskList;
		}
	}
}