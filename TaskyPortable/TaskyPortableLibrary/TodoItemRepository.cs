using System;
using System.Collections.Generic;
using SQLite;

namespace Tasky.PortableLibrary 
{
	public class TodoItemRepository
	{
		TodoDatabase db = null;

		public TodoItemRepository (SQLiteConnection conn)
		{
			db = new TodoDatabase(conn);
		}

		public TodoItem GetTask(int id)
		{
			return db.GetItem(id);
		}

		public IEnumerable<TodoItem> GetTasks ()
		{
			return db.GetItems();
		}

		public int SaveTask (TodoItem item)
		{
			return db.SaveItem(item);
		}

		public int DeleteTask(int id)
		{
			return db.DeleteItem(id);
		}
	}
}

