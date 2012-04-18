using System;
using System.Collections.Generic;

namespace Tasky.Core {
	/// <summary>
	/// Manager classes are an abstraction on the data access layers
	/// </summary>
	public static class TaskManager {
		static TaskManager ()
		{
		}
		
		public static Task GetTask(int id)
		{
			return TaskRepository.GetTask(id);
		}
		
		public static IList<Task> GetTasks ()
		{
			return new List<Task>(TaskRepository.GetTasks());
		}
		
		public static int SaveTask (Task item)
		{
			return TaskRepository.SaveTask(item);
		}
		
		public static int DeleteTask(int id)
		{
			return TaskRepository.DeleteTask(id);
		}
	}
}