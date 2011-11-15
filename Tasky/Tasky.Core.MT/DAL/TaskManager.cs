using System;
using System.IO;
using MonoTouch.Foundation;
using Tasky.BL;
using System.Collections.Generic;

namespace Tasky.DAL
{
	public class TaskManager
	{
		DL.TaskDatabase _db = null;
		protected static string _dbLocation;		
		protected static TaskManager _me;		
		
		static TaskManager ()
		{
			_me = new TaskManager();
		}
		
		protected TaskManager()
		{
			// set the db location
			//_dbLocation = Path.Combine (NSBundle.MainBundle.BundlePath, "Library/TaskDB.db3");
			_dbLocation = Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Personal), "TaskDB.db3");
			// instantiate the database	
			this._db = new Tasky.DL.TaskDatabase(_dbLocation);
		}
		
		public static Task GetTask(int id)
		{
			return _me._db.GetTask(id);
		}
		
		public static IEnumerable<Task> GetTasks ()
		{
			return _me._db.GetTasks();
		}
		
		public static int SaveTask (Task item)
		{
			return _me._db.SaveTask(item);
		}

		public static int DeleteTask(int id)
		{
			return _me._db.DeleteTask(id);
		}

		
	}
}

