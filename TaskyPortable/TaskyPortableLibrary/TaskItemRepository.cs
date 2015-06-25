using System;
using System.Collections.Generic;
using System.IO;
using Tasky.BL;
using SQLite;

namespace Tasky.DAL {
	public class TaskItemRepository {
		DL.TaskItemDatabase db = null;
		protected static string dbLocation;		
		//protected static TaskRepository me;

        public TaskItemRepository(SQLiteConnection conn, string dbLocation)
		{
			db = new Tasky.DL.TaskItemDatabase(conn, dbLocation);
		}

		public TaskItem GetTask(int id)
		{
            return db.GetItem<TaskItem>(id);
		}
		
		public IEnumerable<TaskItem> GetTasks ()
		{
			return db.GetItems<TaskItem>();
		}
		
		public int SaveTask (TaskItem item)
		{
			return db.SaveItem<TaskItem>(item);
		}

		public int DeleteTask(int id)
		{
			return db.DeleteItem<TaskItem>(id);
		}
	}
}

