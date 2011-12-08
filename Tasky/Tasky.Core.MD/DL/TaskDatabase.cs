using System;
using System.Linq;
using Tasky.BL;
using System.Collections.Generic;
using Tasky.DL.SQLite;

namespace Tasky.DL
{
	/// <summary>
	/// TaskDatabase builds on SQLite.Net and represents a specific database, in our case, the Task DB.
	/// It contains methods for retreival and persistance as well as db creation, all based on the 
	/// underlying ORM.
	/// </summary>
	public class TaskDatabase : SQLiteConnection
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Tasky.DL.TaskDatabase"/> TaskDatabase. 
		/// if the database doesn't exist, it will create the database and all the tables.
		/// </summary>
		/// <param name='path'>
		/// Path.
		/// </param>
		public TaskDatabase (string path) : base (path)
		{
			// create the tables
			CreateTable<Task> ();
		}
		
		//TODO: make these methods generic, Add<T>(item), etc.
		
		public IEnumerable<Task> GetTasks ()
		{
			return (from i in this.Table<Task> () select i);
		}
		
		public Task GetTask (int id)
		{
			return (from i in Table<Task> ()
				where i.ID == id
				select i).FirstOrDefault ();
		}
		
		public int SaveTask (Task item)
		{
			if(item.ID != 0)
			{
				base.Update(item);
				return item.ID;
			}
			else
			{
				return base.Insert (item);
			}
		}
		
		public int DeleteTask(int id)
		{
			return base.Delete<Task>(new Task() { ID = id });
		}
	}
}

