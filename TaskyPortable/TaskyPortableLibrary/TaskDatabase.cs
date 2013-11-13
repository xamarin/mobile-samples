using System;
using System.Linq;
using Tasky.BL;
using System.Collections.Generic;
using Tasky.DL.SQLiteBase;

namespace Tasky.DL
{
	/// <summary>
	/// TaskDatabase builds on SQLite.Net and represents a specific database, in our case, the Task DB.
	/// It contains methods for retrieval and persistance as well as db creation, all based on the 
	/// underlying ORM.
	/// </summary>
	public class TaskDatabase 
	{
		static object locker = new object ();

        SQLiteConnection database;

		/// <summary>
		/// Initializes a new instance of the <see cref="Tasky.DL.TaskDatabase"/> TaskDatabase. 
		/// if the database doesn't exist, it will create the database and all the tables.
		/// </summary>
		/// <param name='path'>
		/// Path.
		/// </param>
        public TaskDatabase(SQLiteConnection conn, string path)
		{
            database = conn;
			// create the tables
            database.CreateTable<Task>();
		}
		
		public IEnumerable<T> GetItems<T> () where T : BL.Contracts.IBusinessEntity, new ()
		{
            lock (locker) {
                return (from i in database.Table<T>() select i).ToList();
            }
		}

		public T GetItem<T> (int id) where T : BL.Contracts.IBusinessEntity, new ()
		{
            lock (locker) {
                return database.Table<T>().FirstOrDefault(x => x.ID == id);
                // Following throws NotSupportedException - thanks aliegeni
                //return (from i in Table<T> ()
                //        where i.ID == id
                //        select i).FirstOrDefault ();
            }
		}

		public int SaveItem<T> (T item) where T : BL.Contracts.IBusinessEntity
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
		
		public int DeleteItem<T>(int id) where T : BL.Contracts.IBusinessEntity, new ()
		{
            lock (locker) {
                return database.Delete<T>(new T() { ID = id });
            }
		}
	}
}