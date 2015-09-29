using System;
using System.Collections.Generic;
using System.IO;
using Tasky.BL;

namespace Tasky.DAL {
	public class TaskItemRepository {
		DL.TaskItemDatabase db = null;
		protected static string dbLocation;		
		protected static TaskItemRepository me;		
		
		static TaskItemRepository ()
		{
			me = new TaskItemRepository();
		}
		
		protected TaskItemRepository()
		{
			// set the db location
			dbLocation = DatabaseFilePath;
			
			// instantiate the database	
			db = new Tasky.DL.TaskItemDatabase(dbLocation);
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
				string libraryPath = Path.Combine (documentsPath, "..", "Library"); // Library folder
#endif










				var path = Path.Combine (libraryPath, sqliteFilename);
#endif		






				return path;	
			}
		}

		public static TaskItem GetTask(int id)
		{
            return me.db.GetItem<TaskItem>(id);
		}
		
		public static IEnumerable<TaskItem> GetTasks ()
		{
			return me.db.GetItems<TaskItem>();
		}
		
		public static int SaveTask (TaskItem item)
		{
			return me.db.SaveItem<TaskItem>(item);
		}

		public static int DeleteTask(int id)
		{
			return me.db.DeleteItem<TaskItem>(id);
		}
	}
}

