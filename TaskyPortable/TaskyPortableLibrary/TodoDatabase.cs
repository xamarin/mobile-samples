using System;
using System.Linq;
using System.Collections.Generic;
using SQLite;

namespace Tasky.PortableLibrary
{
	/// <summary>
	/// TaskDatabase uses ADO.NET to create the [Items] table and create,read,update,delete data
	/// </summary>
	public class TodoDatabase 
	{
		static object locker = new object ();

		public SQLiteConnection database;

		public string path;

		/// <summary>
		/// Initializes a new instance of the <see cref="Tasky.DL.TaskDatabase"/> TaskDatabase. 
		/// if the database doesn't exist, it will create the database and all the tables.
		/// </summary>
		public TodoDatabase (SQLiteConnection conn) 
		{
			database = conn;
			// create the tables
			database.CreateTable<TodoItem>();
		}

		public IEnumerable<TodoItem> GetItems ()
		{
			lock (locker) {
				return (from i in database.Table<TodoItem>() select i).ToList();
			}
		}

		public TodoItem GetItem (int id) 
		{
			lock (locker) {
				return database.Table<TodoItem>().FirstOrDefault(x => x.ID == id);
				// Following throws NotSupportedException - thanks aliegeni
				//return (from i in Table<T> ()
				//        where i.ID == id
				//        select i).FirstOrDefault ();
			}
		}

		public int SaveItem (TodoItem item) 
		{
			lock (locker) {
				if (item.ID != 0) {
					database.Update(item);
					return item.ID;
				} else {
					return database.Insert(item);
				}
			}
		}

		public int DeleteItem(int id) 
		{
			lock (locker) {
				return database.Delete<TodoItem>(id);
			}
		}
	}
}