using System;
using System.Collections.Generic;
using System.IO;
using Tasky.BL;
using Tasky.DL.SQLiteBase;

namespace Tasky.DAL {
	public class TaskRepository {
		DL.TaskDatabase db = null;
		protected static string dbLocation;		
		//protected static TaskRepository me;

        public TaskRepository(SQLiteConnection conn, string dbLocation)
		{
			db = new Tasky.DL.TaskDatabase(conn, dbLocation);
		}

		public Task GetTask(int id)
		{
            return db.GetItem<Task>(id);
		}
		
		public IEnumerable<Task> GetTasks ()
		{
			return db.GetItems<Task>();
		}
		
		public int SaveTask (Task item)
		{
			return db.SaveItem<Task>(item);
		}

		public int DeleteTask(int id)
		{
			return db.DeleteItem<Task>(id);
		}
	}
}

