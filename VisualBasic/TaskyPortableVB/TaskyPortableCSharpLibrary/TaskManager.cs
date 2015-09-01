using System;
using System.Collections.Generic;

namespace Tasky.Portable {
	/// <summary>
	/// Manager classes are an abstraction on the data access layers
	/// </summary>
	public class TaskManager {
		TaskRepository repository;

		public TaskManager (string filename, IXmlStorage storage) 
		{
			repository = new TaskRepository(filename, storage);
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