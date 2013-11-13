using System;
using System.Collections.Generic;
using Tasky.BL;
using Tasky.DL.SQLiteBase;

namespace Tasky.BL.Managers
{
	public class TaskManager
	{
        DAL.TaskRepository repository;

		public TaskManager (SQLiteConnection conn) 
        {
            repository = new DAL.TaskRepository(conn, "");
        }

		public Task GetTask(int id)
		{
            return repository.GetTask(id);
		}
		
		public IList<Task> GetTasks ()
		{
            return new List<Task>(repository.GetTasks());
		}
		
		public int SaveTask (Task item)
		{
            return repository.SaveTask(item);
		}
		
		public int DeleteTask(int id)
		{
            return repository.DeleteTask(id);
		}
		
	}
}