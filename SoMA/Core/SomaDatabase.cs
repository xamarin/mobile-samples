using System;
using SQLite;
using System.Collections.Generic;
using System.Linq;

namespace Core
{
	/// <summary>
	/// SomaDatabase builds on SQLite.Net and represents a specific database, in our case, the Task DB.
	/// It contains methods for retrieval and persistance as well as db creation, all based on the 
	/// underlying ORM.
	/// </summary>
	public class SomaDatabase : SQLiteConnection
	{
		static object locker = new object ();

		/// <summary>
		/// Initializes a new instance of the SomaDatabase. 
		/// if the database doesn't exist, it will create the database and all the tables.
		/// </summary>
		/// <param name='path'>
		/// Path.
		/// </param>
		public SomaDatabase (string path) : base (path)
		{
			// create the tables
			CreateTable<ShareItem> ();
		}

		public List<ShareItem> GetItems () {
			lock (locker) {
				return (from i in Table<ShareItem> () select i).ToList ();
			}
		}

		public ShareItem GetItem (int id) 
		{
			lock (locker) {
				return Table<ShareItem>().FirstOrDefault(x => x.Id == id);
				// Following throws NotSupportedException - thanks aliegeni
				//return (from i in Table<T> ()
				//        where i.ID == id
				//        select i).FirstOrDefault ();
			}
		}

		public int SaveItem (ShareItem item)
		{
			lock (locker) {
				if (item.Id != 0) {
					Update (item);
					return item.Id;
				} else {
					return Insert (item);
				}
			}
		}

		public int DeleteItem (int id)
		{
			lock (locker) {
				return Delete<ShareItem> (new ShareItem () { Id = id });
			}
		}
	}
}

