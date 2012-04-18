using System;
using System.Collections.Generic;
using System.IO;

namespace Tasky.Core {
	/// <summary>
	/// The repository is responsible for providing an abstraction to actual data storage mechanism
	/// whether it be SQLite, XML or some other method
	/// </summary>
	public class TaskRepository {
		TaskDatabase db = null;
		protected static string dbLocation;		
		protected static TaskRepository me;		
		
		static TaskRepository ()
		{
			me = new TaskRepository();
		}
		
		protected TaskRepository()
		{
			// set the db location
			dbLocation = DatabaseFilePath;
			
			// instantiate the database	
			db = new TaskDatabase(dbLocation);
		}
		
		public static string DatabaseFilePath {
			get { 
				var sqliteFilename = "TaskDB.db3";
#if SILVERLIGHT
				// Windows Phone expects a local path, not absolute
	            var path = sqliteFilename;
#else

#if __ANDROID__
				// Just use whatever directory SpecialFolder.Personal returns
	            string libraryPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); ;
#else
				// we need to put in /Library/ on iOS5.1 to meet Apple's iCloud terms
				// (they don't want non-user-generated data in Documents)
				string documentsPath = Environment.GetFolderPath (Environment.SpecialFolder.Personal); // Documents folder
				string libraryPath = Path.Combine (documentsPath, "../Library/"); // Library folder
#endif
				var path = Path.Combine (libraryPath, sqliteFilename);
#endif		
				return path;	
			}
		}

		public static Task GetTask(int id)
		{
			return me.db.GetItem<Task>(id);
		}
		
		public static IEnumerable<Task> GetTasks ()
		{
			return me.db.GetItems<Task>();
		}
		
		public static int SaveTask (Task item)
		{
			return me.db.SaveItem<Task>(item);
		}

		public static int DeleteTask(int id)
		{
			return me.db.DeleteItem<Task>(id);
		}
	}
}