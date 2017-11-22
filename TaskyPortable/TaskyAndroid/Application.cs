using System;
using System.IO;
using SQLite;
using Android.App;
using Tasky.PortableLibrary;

namespace TaskyAndroid
{
	[Application]
	public class TaskyApp : Application {
		public static TaskyApp Current { get; private set; }

		public TodoItemManager TodoManager { get; set; }
		SQLiteConnection conn;

		public TaskyApp(IntPtr handle, global::Android.Runtime.JniHandleOwnership transfer)
			: base(handle, transfer) {
			Current = this;
		}

		public override void OnCreate()
		{
			base.OnCreate();

			var sqliteFilename = "TodoItemDB.db3";
			string libraryPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			var path = Path.Combine(libraryPath, sqliteFilename);
			conn = new SQLiteConnection(path);

			TodoManager = new TodoItemManager(conn);
		}
	}
}

