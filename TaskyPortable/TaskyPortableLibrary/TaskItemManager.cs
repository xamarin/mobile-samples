using System;
using System.Collections.Generic;
using Tasky.BL;
using SQLite;

namespace Tasky.BL.Managers
{
	public class TaskItemManager
	{
        DAL.TaskItemRepository repository;

		public TaskItemManager (SQLiteConnection conn) 
        {
            repository = new DAL.TaskItemRepository(conn, "");
        }

		public TaskItem GetTask(int id)
		{
            return repository.GetTask(id);
		}
		
		public IList<TaskItem> GetTasks ()
		{
            return new List<TaskItem>(repository.GetTasks());
		}
		
		public int SaveTask (TaskItem item)
		{
            return repository.SaveTask(item);
		}
		
		public int DeleteTask(int id)
		{
            return repository.DeleteTask(id);
		}
		
	}
}