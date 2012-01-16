using System;
using System.Collections.Generic;
using System.Linq;
using MWC.BL;

namespace MWC.DAL
{
	/// <summary>
	/// [abstracts fromt the underlying data source(s)]
	/// [if multiple data sources, can agreggate/etc without BL knowing]
	/// [superflous if only one data source]
	/// </summary>
	public static class DataManager
	{
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
		
		public static int SaveSession (Session item)
		{
			return DL.MwcDatabase.SaveItem<Session> (item);
		}
		
		public static void SaveSessions (IEnumerable<Session> items)
		{
			DL.MwcDatabase.SaveItems<Session> (items);
		}
		
		public static int DeleteSession(int id)
		{
			return DL.MwcDatabase.DeleteItem<Session> (id);
		}
		
		public static void DeleteSessions()
		{
			DL.MwcDatabase.ClearTable<Session>();
		}
		
		/// <summary>
		/// Gets the sessions for a given day (day 1 - 4).
		/// </summary>
		/// <param name='day'>
		/// Day.
		/// </param>
		public static IEnumerable<Session> GetSessions ( int day )
		{
			DateTime dayMin = Constants.StartDateMin; //new DateTime ( 2012, 02, 27, 0, 0, 0 );
			DateTime dayMax = Constants.StartDateMax; //new DateTime ( 2012, 02, 27, 23, 59, 59 );

			// increment for days
			dayMin = dayMin.AddDays ( day - 1 );
			dayMax = dayMax.AddDays ( day - 1 );

			return DL.MwcDatabase.GetSessionsByStartDate ( dayMin, dayMax );
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
			return DL.MwcDatabase.GetItems<Exhibitor> ();
		}
		
		public static Exhibitor GetExhibitor (int id)
		{
			//return DL.MwcDatabase.GetItem<Exhibitor> (id);
            return DL.MwcDatabase.GetExhibitor(id);
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
		// (to be confirmed, adapted if required)

		public static int SaveFavorite (Favorite favorite)
		{
			//var fav = new Favorite { SessionName = sessionName };
            return DL.MwcDatabase.SaveItem<Favorite>(favorite);
		}

		public static IEnumerable<Favorite> GetFavorites ()
		{
			return DL.MwcDatabase.GetItems<Favorite> ();
		}
		
		public static bool GetIsFavorite (string sessionName)
		{
			var fav = (from f in GetFavorites()
					  where f.SessionName == sessionName
					  select f).FirstOrDefault();

			return fav != null;
		}
		
		static int GetFavoriteID (string sessionName)
		{
			var fav = (from f in GetFavorites()
					  where f.SessionName == sessionName
					  select f).FirstOrDefault();

			return fav != null?fav.ID:-1;
		}

		public static int DeleteFavorite(string sessionName)
		{
			int id = GetFavoriteID (sessionName);
			if (id >= 0)
				return DL.MwcDatabase.DeleteItem<Favorite> (id);
			else 
				return -1;
		}
		#endregion

		#region Tweets
		public static void SaveTweets (IEnumerable<Tweet> items)
		{
			DL.MwcDatabase.SaveItems<Tweet> (items);
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
		public static IEnumerable<RSSEntry> GetNews ()
		{
			return DL.MwcDatabase.GetItems<RSSEntry> ();
		}
		public static void DeleteNews()
		{
			DL.MwcDatabase.ClearTable<RSSEntry>();
		}
		#endregion
	}
}

