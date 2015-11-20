using System;
using System.Collections.Generic;
using SQLite;

namespace Tasky.PortableLibrary 
{
	/// <summary>
	/// Manager classes are an abstraction on the data access layers
	/// </summary>
	public class TodoItemManager 
	{
		TodoItemRepository repository; 

		public TodoItemManager (SQLiteConnection conn)
		{
			repository = new TodoItemRepository (conn);
		}
		
		public TodoItem GetTask(int id)
		{
			return repository.GetTask(id);
		}
		
		public IList<TodoItem> GetTasks ()
		{
			return new List<TodoItem>(repository.GetTasks());
		}
		
		public int SaveTask (TodoItem item)
		{
			return repository.SaveTask(item);
		}
		
		public int DeleteTask(int id)
		{
			return repository.DeleteTask(id);
		}
	}
}