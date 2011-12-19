using System;
using System.Collections.Generic;
using System.Linq;
using MWC.BL;
using MWC.DL.SQLite;
using System.IO;

namespace MWC.DL
{
	/// <summary>
	/// TaskDatabase builds on SQLite.Net and represents a specific database, in our case, the MWC DB.
	/// It contains methods for retreival and persistance as well as db creation, all based on the 
	/// underlying ORM.
	/// </summary>
	public class MwcDatabase : SQLiteConnection
	{
		protected static MwcDatabase _me = null;
		protected static string _dbLocation;		
		
		/// <summary>
		/// Initializes a new instance of the <see cref="MWC.DL.MwcDatabase"/> MwcDatabase. 
		/// if the database doesn't exist, it will create the database and all the tables.
		/// </summary>
		/// <param name='path'>
		/// Path.
		/// </param>
		protected MwcDatabase (string path) : base (path)
		{
			// create the tables
			CreateTable<Exhibitor> ();
			CreateTable<Session> ();			
			CreateTable<Speaker> ();
		}

		static MwcDatabase ()
		{
			// set the db location
			_dbLocation = Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Personal), "MwcDB.db3");
			
			// instantiate a new db
			_me = new MwcDatabase(_dbLocation);
		}
		
		public static IEnumerable<T> GetItems<T> () where T : BL.Contracts.IBusinessEntity, new ()
		{
			return (from i in _me.Table<T> () select i);
		}
		
		public static T GetItem<T> (int id) where T : BL.Contracts.IBusinessEntity, new ()
		{
			return (from i in _me.Table<T> ()
				where i.ID == id
				select i).FirstOrDefault ();
		}
		
		public static int SaveItem<T> (T item) where T : BL.Contracts.IBusinessEntity
		{
			if(item.ID != 0)
			{
				_me.Update(item);
				return item.ID;
			}
			else
			{
				return _me.Insert (item);
			}
		}
		
		//TODO: Transact this so it's not ass-slow
		public static void SaveItems<T> (IEnumerable<T> items) where T : BL.Contracts.IBusinessEntity
		{
			foreach(T item in items)
			{
				SaveItem<T>(item);
			}
		}
		
		public static int DeleteItem<T>(int id) where T : BL.Contracts.IBusinessEntity, new ()
		{
			return _me.Delete<T>(new T() { ID = id });
		}
		
		
	}
}

