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
using System.IO;

namespace RazorNativeTodo
{
	[Activity (Label = "HybridNativeTodo", MainLauncher = true)]			
	public class NativeListActivity : Activity
	{
		List<TodoItem> todoItems;
		TaskListAdapter adapter;

		Button addButton, speakButton;
		ListView todoList;
		Speech speech;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView(Resource.Layout.NativeList);

			addButton = FindViewById<Button> (Resource.Id.AddButton);
			addButton.Click += (sender, e) => {
				StartActivity(typeof(RazorActivity));
			};

			speakButton = FindViewById<Button> (Resource.Id.SpeakButton);
			speakButton.Click += (sender, e) => {
				var todos = App.Database.GetItemsNotDone ();
				var tospeak = "";
				foreach (var t in todos)
					tospeak += t.Name + " ";
				if (tospeak == "")
					tospeak = "there are no tasks to do";
				if (speech == null)
					speech = new Speech ();
				speech.Speak (this, tospeak);
			};

			todoList = FindViewById<ListView> (Resource.Id.TodoList);
			todoList.ItemClick += (sender, e) => {
				var taskDetails = new Intent (this, typeof (RazorActivity));
				taskDetails.PutExtra ("todoid", todoItems[e.Position].ID);
				StartActivity (taskDetails);
			};

			#region Database setup
			var sqliteFilename = "TodoSQLite.db3";
			string documentsPath = System.Environment.GetFolderPath (System.Environment.SpecialFolder.Personal); // Documents folder
			var path = Path.Combine(documentsPath, sqliteFilename);

			// This is where we copy in the prepopulated database
			Console.WriteLine (path);
			if (!File.Exists(path))
			{
				var s = Resources.OpenRawResource(RazorNativeTodo.Resource.Raw.TodoSQLite);  // RESOURCE NAME ###

				// create a write stream
				FileStream writeStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
				// write to the stream
				ReadWriteStream(s, writeStream);
			}

			var plat = new SQLite.Net.Platform.XamarinAndroid.SQLitePlatformAndroid();
			var conn = new SQLite.Net.SQLiteConnection(plat, path);

			// Set the database connection string
			App.SetDatabaseConnection (conn);
			#endregion
		}

		protected override void OnResume ()
		{
			base.OnResume ();

			todoItems = App.Database.GetItems().ToList();

			// create our adapter
			adapter = new TaskListAdapter(this, todoItems);

			//Hook up our adapter to our ListView
			todoList.Adapter = adapter;
		}

		/// <summary>
		/// helper method to get the database out of /raw/ and into the user filesystem
		/// </summary>
		void ReadWriteStream(Stream readStream, Stream writeStream)
		{
			int Length = 256;
			Byte[] buffer = new Byte[Length];
			int bytesRead = readStream.Read(buffer, 0, Length);
			// write the required bytes
			while (bytesRead > 0)
			{
				writeStream.Write(buffer, 0, bytesRead);
				bytesRead = readStream.Read(buffer, 0, Length);
			}
			readStream.Close();
			writeStream.Close();
		}
	}

	public class TaskListAdapter : BaseAdapter<TodoItem> {
		protected Activity context = null;
		protected IList<TodoItem> todoItems = new List<TodoItem>();

		public TaskListAdapter (Activity context, IList<TodoItem> tasks) : base ()
		{
			this.context = context;
			this.todoItems = tasks;
		}

		public override TodoItem this[int position]
		{
			get { return todoItems[position]; }
		}

		public override long GetItemId (int position)
		{
			return position;
		}

		public override int Count
		{
			get { return todoItems.Count; }
		}

		public override global::Android.Views.View GetView (int position, global::Android.Views.View convertView, global::Android.Views.ViewGroup parent)
		{
			// Get our object for position
			var task = todoItems[position];			

			//Try to reuse convertView if it's not  null, otherwise inflate it from our item layout
			// gives us some performance gains by not always inflating a new view
			// will sound familiar to MonoTouch developers with UITableViewCell.DequeueReusableCell()
			var view = (convertView ?? 
				context.LayoutInflater.Inflate(
					global::Android.Resource.Layout.SimpleListItemChecked,
					parent, 
					false)) as CheckedTextView;

			view.SetText (task.Name, TextView.BufferType.Normal);
			view.Checked = task.Done;

			//Finally return the view
			return view;
		}
	}
}

