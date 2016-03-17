using System;
using System.Collections.Generic;

namespace Tasky.Shared 
{
	/// <summary>
	/// Manager classes are an abstraction on the data access layers
	/// </summary>
	public static class TodoItemManager 
	{
		static TodoItemManager ()
		{
		}
		
		public static TodoItem GetTask(int id)
		{
			return TodoItemRepositoryADO.GetTask(id);
		}
		
		public static IList<TodoItem> GetTasks ()
		{
			return new List<TodoItem>(TodoItemRepositoryADO.GetTasks());
		}
		
		public static int SaveTask (TodoItem item)
		{
			return TodoItemRepositoryADO.SaveTask(item);
		}
		
		public static int DeleteTask(int id)
		{
			return TodoItemRepositoryADO.DeleteTask(id);
		}
	}
}