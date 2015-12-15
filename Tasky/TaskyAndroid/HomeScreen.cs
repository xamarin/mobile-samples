using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Tasky.Shared;
using TaskyAndroid;
using TaskyAndroid.ApplicationLayer;
using Android.Content.PM;

namespace TaskyAndroid.Screens 
{
	/// <summary>
	/// Main ListView screen displays a list of tasks, plus an [Add] button
	/// </summary>
	[Activity (Label = "Tasky",  
		Icon="@drawable/icon",
		MainLauncher = true,
		ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,
		ScreenOrientation = ScreenOrientation.Portrait)]
	public class HomeScreen : Activity 
	{
		TodoItemListAdapter taskList;
		IList<TodoItem> tasks;
		Button addTaskButton;
		ListView taskListView;
		
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// set our layout to be the home screen
			SetContentView(Resource.Layout.HomeScreen);

			//Find our controls
			taskListView = FindViewById<ListView> (Resource.Id.TaskList);
			addTaskButton = FindViewById<Button> (Resource.Id.AddButton);

			// wire up add task button handler
			if(addTaskButton != null) {
				addTaskButton.Click += (sender, e) => {
					StartActivity(typeof(TodoItemScreen));
				};
			}
			
			// wire up task click handler
			if(taskListView != null) {
				taskListView.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) => {
					var taskDetails = new Intent (this, typeof (TodoItemScreen));
					taskDetails.PutExtra ("TaskID", tasks[e.Position].ID);
					StartActivity (taskDetails);
				};
			}
		}
		
		protected override void OnResume ()
		{
			base.OnResume ();

			tasks = TodoItemManager.GetTasks();
			
			// create our adapter
			taskList = new TodoItemListAdapter(this, tasks);

			//Hook up our adapter to our ListView
			taskListView.Adapter = taskList;
		}
	}
}