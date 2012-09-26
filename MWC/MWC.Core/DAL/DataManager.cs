using System;
using System.Collections.Generic;
using System.Linq;
using MWC.BL;

namespace MWC.DAL {
	/// <summary>
	/// [abstracts fromt the underlying data source(s)]
	/// [if multiple data sources, can agreggate/etc without BL knowing]
	/// [superflous if only one data source]
	/// </summary>
	public static class DataManager {
		#region Session
		
		public static IEnumerable<Session> GetSessions ()
		{
			return DL.MwcDatabase.GetItems<Session> ();
		}
		
		public static Session GetSession (int id)
		{
			//return DL.MwcDatabase.GetItem<Session> (id);
            return DL.MwcDatabase.GetSession(id);
		}

        public static Session GetSessionWithKey (string key)
        {
            return DL.MwcDatabase.GetSessionWithKey (key);
        }
		
		public static int SaveSession (Session item)
		{
			return DL.MwcDatabase.SaveItem<Session> (item);
		}
		
		public static void SaveSessions (IEnumerable<Session> items)
		{
			DL.MwcDatabase.SaveItems<Session> (items);
		}
		
		public static void SaveSessionSpeakers (IEnumerable<SessionSpeaker> items)
		{
			DL.MwcDatabase.SaveItems<SessionSpeaker> (items);
		}

		public static int DeleteSession(int id)
		{
			return DL.MwcDatabase.DeleteItem<Session> (id);
		}
		
		public static void DeleteSessions()
		{
			DL.MwcDatabase.ClearTable<Session>();
		}

		public static void DeleteSessionSpeakers()
		{
			DL.MwcDatabase.ClearTable<SessionSpeaker>();
		}
		
		/// <summary>
		/// Gets the sessions for a given day (day 1 - 4).
		/// </summary>
		/// <param name='day'>
		/// [1 - 4] number of the day in the conference. 
		/// Converted to zero-based in the method
		/// </param>
		public static IEnumerable<Session> GetSessions (int day)
		{
			DateTime dayMin = Constants.StartDateMin;
			
			// increment for days
			dayMin = dayMin.AddDays (day - 1);
			DateTime dayMax = dayMin.AddHours (24);
			
			return DL.MwcDatabase.GetSessionsByStartDate (dayMin, dayMax);
		}
		
		#endregion

		#region Speaker
		
		public static IEnumerable<Speaker> GetSpeakers ()
		{
			return DL.MwcDatabase.GetItems<Speaker> ();
		}
		
		public static Speaker GetSpeaker (int id)
		{
			//return DL.MwcDatabase.GetItem<Speaker> (id);
            return DL.MwcDatabase.GetSpeaker(id);
		}

        public static Speaker GetSpeakerWithKey (string key)
        {
            return DL.MwcDatabase.GetSpeakerWithKey (key);
        }
		
		public static int SaveSpeaker (Speaker item)
		{
			return DL.MwcDatabase.SaveItem<Speaker> (item);
		}
		
		public static void SaveSpeakers (IEnumerable<Speaker> items)
		{
			DL.MwcDatabase.SaveItems<Speaker> (items);
		}
		
		public static int DeleteSpeaker(int id)
		{
			return DL.MwcDatabase.DeleteItem<Speaker> (id);
		}
		
		public static void DeleteSpeakers()
		{
			DL.MwcDatabase.ClearTable<Speaker>();
		}
		
		#endregion

		#region Exhibitor
		
		public static IEnumerable<Exhibitor> GetExhibitors ()
		{
			return DL.MwcDatabase.GetItems<Exhibitor> ().OrderBy (exhibitor => exhibitor.Name);
		}
		
		public static Exhibitor GetExhibitor (int id)
		{
			//return DL.MwcDatabase.GetItem<Exhibitor> (id);
            return DL.MwcDatabase.GetExhibitor(id);
		}

        public static Exhibitor GetExhibitorWithName (string name)
        {
            return DL.MwcDatabase.GetExhibitorWithName (name);
        }
		
		public static int SaveExhibitor (Exhibitor item)
		{
			return DL.MwcDatabase.SaveItem<Exhibitor> (item);
		}
		
		public static void SaveExhibitors (IEnumerable<Exhibitor> items)
		{
			DL.MwcDatabase.SaveItems<Exhibitor> (items);
		}
		
		public static int DeleteExhibitor(int id)
		{
			return DL.MwcDatabase.DeleteItem<Exhibitor> (id);
		}
		
		public static void DeleteExhibitors()
		{
			DL.MwcDatabase.ClearTable<Exhibitor>();
		}
		
		#endregion

		#region Favorites
		
		// API for managing favorites is via SessionName
		// since Session.ID might not survive data updates
		// (to be confirmed, adapt if required)

		public static int SaveFavorite (Favorite favorite)
		{
            //var fav = new Favorite { SessionKey = sessionKey };
            return DL.MwcDatabase.SaveItem<Favorite>(favorite);
		}

		public static IEnumerable<Favorite> GetFavorites ()
		{
			return DL.MwcDatabase.GetItems<Favorite> ();
		}
		
		public static bool GetIsFavorite (string sessionKey)
		{
			var fav = (from f in GetFavorites()
                       where f.SessionKey == sessionKey
					  select f).FirstOrDefault();

			return fav != null;
		}

        static int GetFavoriteID(string sessionKey)
		{
			var fav = (from f in GetFavorites()
                       where f.SessionKey == sessionKey
					  select f).FirstOrDefault();

			return fav != null?fav.ID:-1;
		}

        public static int DeleteFavorite(string sessionKey)
		{
            int id = GetFavoriteID(sessionKey);
			if (id >= 0)
				return DL.MwcDatabase.DeleteFavorite (id); // HACK: was DeleteItem<Favorite>
			else 
				return -1;
		}
		#endregion

		#region Tweets
		public static void SaveTweets (IEnumerable<Tweet> items)
		{
			DL.MwcDatabase.SaveItems<Tweet> (items);
		}
        public static Tweet GetTweet(int id)
        {
            //return DL.MwcDatabase.GetItem<Tweet> (id);
            return DL.MwcDatabase.GetTweet(id);
        }
		public static IEnumerable<Tweet> GetTweets ()
		{
			return DL.MwcDatabase.GetItems<Tweet> ();
		}
		public static void DeleteTweets()
		{
			DL.MwcDatabase.ClearTable<Tweet>();
		}
		#endregion

		#region News
		public static void SaveNews (IEnumerable<RSSEntry> items)
		{
			DL.MwcDatabase.SaveItems<RSSEntry> (items);
		}
        public static RSSEntry GetNews(int id)
        {
            //return DL.MwcDatabase.GetItem<RSSEntry> (id);
            return DL.MwcDatabase.GetNews(id);
        }
		public static IEnumerable<RSSEntry> GetNews ()
		{
			return DL.MwcDatabase.GetItems<RSSEntry> ().OrderByDescending (item => item.Published);
		}
		public static void DeleteNews()
		{
			DL.MwcDatabase.ClearTable<RSSEntry>();
		}
		#endregion
	}
}