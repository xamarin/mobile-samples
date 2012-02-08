using System;
using System.Collections.Generic;
using System.Linq;
using MWC.BL;
using MWC.DL.SQLite;
using System.IO;

namespace MWC.DL {
	/// <summary>
	/// TaskDatabase builds on SQLite.Net and represents a specific database, in our case, the MWC DB.
	/// It contains methods for retreival and persistance as well as db creation, all based on the 
	/// underlying ORM.
	/// </summary>
	public class MwcDatabase : SQLiteConnection {
		protected static MwcDatabase me = null;
		protected static string dbLocation;

        static object locker = new object ();
		
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
			
			// FK
			CreateTable<SessionSpeaker> ();

			// these are really for caches
			CreateTable<Tweet> ();
			CreateTable<RSSEntry> ();
		}

		static MwcDatabase ()
		{
			// set the db location
#if SILVERLIGHT
            dbLocation = "MwcDB.db3";
#else
			dbLocation = Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Personal), "MwcDB.db3");
#endif
			
			// instantiate a new db
			me = new MwcDatabase(dbLocation);
		}
		
		public static IEnumerable<T> GetItems<T> () where T : BL.Contracts.IBusinessEntity, new ()
		{
            lock (locker) {
                return (from i in me.Table<T> () select i).ToList ();
            }
		}
		
		public static T GetItem<T> (int id) where T : BL.Contracts.IBusinessEntity, new ()
		{
            lock (locker) {
                return (from i in me.Table<T> ()
                        where i.ID == id
                        select i).FirstOrDefault ();
            }
		}
		
		public static int SaveItem<T> (T item) where T : BL.Contracts.IBusinessEntity
		{
            lock (locker) {
                if (item.ID != 0) {
                    me.Update (item);
                    return item.ID;
                } else {
                    return me.Insert (item);
                }
            }
		}
		
		public static void SaveItems<T> (IEnumerable<T> items) where T : BL.Contracts.IBusinessEntity
		{
            lock (locker) {
                me.BeginTransaction ();

                foreach (T item in items) {
                    SaveItem<T> (item);
                }

                me.Commit ();
            }
		}
		
		public static int DeleteItem<T>(int id) where T : BL.Contracts.IBusinessEntity, new ()
		{
            lock (locker) {
                return me.Delete<T> (new T () { ID = id });
            }
		}
		
		public static void ClearTable<T>() where T : BL.Contracts.IBusinessEntity, new ()
		{
            lock (locker) {
                me.Execute (string.Format ("delete from \"{0}\"", typeof (T).Name));
            }
		}
		
//		public static IEnumerable<T> Query<T>(string query)
//		{
//			return _me.Query.
//		}
		
		public static IEnumerable<Session> GetSessionsByStartDate(DateTime dateMin, DateTime dateMax)
		{
            lock (locker) {
                return (from i in me.Table<Session> ()
                        where i.Start >= dateMin && i.Start <= dateMax
                        select i).ToList ();
            }
		}


        /*
         * the following two queries are currently required because the Generic versions throw
         * an exception on this line in SQLite.cs (Android ONLY)
         * 1565:  throw new NotSupportedException ("Cannot compile: " + expr.NodeType.ToString ());
         */
        public static Session GetSession (int id)
        {
            lock (locker) {
                //return DL.MwcDatabase.GetItem<Session> (id);
				Session session = (from s in me.Table<Session> ()
                        where s.ID == id
                        select s).FirstOrDefault ();
				try{ // bug occurs in simulator...???
				session.SpeakerKeys = (from ss in me.Table<SessionSpeaker> ()
									where ss.SessionKey == session.Key
									select ss.SpeakerKey).ToList();
				var speakers = GetItems<Speaker>();

				var speakerInSession = (from sp in speakers
								where session.SpeakerKeys.Contains (sp.Key)
								select sp).ToList ();

				session.Speakers = speakerInSession;
					} catch {}
				return session;
            }
        }
        public static Session GetSessionWithKey (string key)
        {
            lock (locker) {
//                return (from s in me.Table<Session> ()
//                        where s.Key == key
//                        select s).FirstOrDefault ();
				Session session = (from s in me.Table<Session> ()
                        where s.Key == key
                        select s).FirstOrDefault ();

				session.SpeakerKeys = (from ss in me.Table<SessionSpeaker> ()
									where ss.SessionKey == session.Key
									select ss.SpeakerKey).ToList();
				var speakers = GetItems<Speaker>();

				var speakerInSession = (from sp in speakers
								where session.SpeakerKeys.Contains (sp.Key)
								select sp).ToList ();

				session.Speakers = speakerInSession;

				return session;
            }
        }
        public static Speaker GetSpeaker(int id)
        {
            lock (locker) {
                //return DL.MwcDatabase.GetItem<Session> (id);
                return (from s in me.Table<Speaker> ()
                        where s.ID == id
                        select s).FirstOrDefault ();
            }
        }
        public static Speaker GetSpeakerWithKey (string key)
        {
            lock (locker) {
                return (from s in me.Table<Speaker> ()
                        where s.Key == key
                        select s).FirstOrDefault ();
            }
        }
        public static Exhibitor GetExhibitor(int id)
        {
            lock (locker) {
                //return DL.MwcDatabase.GetItem<Exhibitor> (id);
                return (from s in me.Table<Exhibitor> ()
                        where s.ID == id
                        select s).FirstOrDefault ();
            }
        }
        public static Exhibitor GetExhibitorWithName (string name)
        {
            lock (locker) {
                return (from s in me.Table<Exhibitor> ()
                        where s.Name == name
                        select s).FirstOrDefault ();
            }
        }
        public static Tweet GetTweet(int id)
        {
            lock (locker) {
                //return DL.MwcDatabase.GetItem<Tweet> (id);
                return (from s in me.Table<Tweet> ()
                        where s.ID == id
                        select s).FirstOrDefault ();
            }
        }
        public static RSSEntry GetNews(int id)
        {
            lock (locker) {
                //return DL.MwcDatabase.GetItem<RSSEntry> (id);
                return (from s in me.Table<RSSEntry> ()
                        where s.ID == id
                        select s).FirstOrDefault ();
            }
        }
	}
}