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
			CreateTable<Favorite> ();
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
		
		public static void SaveItems<T> (IEnumerable<T> items) where T : BL.Contracts.IBusinessEntity
		{
			_me.BeginTransaction();
			
			foreach(T item in items)
			{
				SaveItem<T>(item);
			}
			
			_me.Commit();
		}
		
		public static int DeleteItem<T>(int id) where T : BL.Contracts.IBusinessEntity, new ()
		{
			return _me.Delete<T>(new T() { ID = id });
		}
		
		//TODO: extend sqlite.net to do a "delete * from T"
		public static void ClearTable<T>() where T : BL.Contracts.IBusinessEntity, new ()
		{
			IEnumerable<T> items = GetItems<T>();
			
			_me.BeginTransaction();
			
			foreach ( var i in items)
				DeleteItem<T>( i.ID );
			
			_me.Commit();
		}
		
//		public static IEnumerable<T> Query<T>(string query)
//		{
//			return _me.Query.
//		}
		
		public static IEnumerable<Session> GetSessionsByStartDate(DateTime dateMin, DateTime dateMax)
		{
			return (from i in _me.Table<Session> ()
				where i.Start >= dateMin && i.Start <= dateMax
				select i);
		}


        /*
         * the following two queries are currently required because the Generic versions throw
         * an exception on this line in SQLite.cs (Android ONLY)
         * 1565:  throw new NotSupportedException ("Cannot compile: " + expr.NodeType.ToString ());
         */
        public static Session GetSession(int id)
        {
            //return DL.MwcDatabase.GetItem<Session> (id);
            return (from s in _me.Table<Session> ()
                    where s.ID == id
                    select s).FirstOrDefault();
        }
        public static Speaker GetSpeaker(int id)
        {
            //return DL.MwcDatabase.GetItem<Session> (id);
            return (from s in _me.Table<Speaker>()
                    where s.ID == id
                    select s).FirstOrDefault();
        }
        public static Exhibitor GetExhibitor(int id)
        {
            //return DL.MwcDatabase.GetItem<Exhibitor> (id);
            return (from s in _me.Table<Exhibitor>()
                    where s.ID == id
                    select s).FirstOrDefault();
        }
	}
}

