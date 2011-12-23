using System;
using System.Collections.Generic;
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
			return DL.MwcDatabase.GetItem<Session> (id);
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
			DateTime startMin = startMin = new DateTime ( 2012, 02, 27, 0, 0, 0 );
			DateTime startMax = startMax = new DateTime ( 2012, 02, 27, 23, 59, 59 );

			// increment for days
			startMin = startMin.AddDays ( day - 1 );
			startMax = startMax.AddDays ( day - 1 );

			return DL.MwcDatabase.GetSessionsByStartDate ( startMin, startMax );
		}
		
		#endregion

		#region Speaker
		
		public static IEnumerable<Speaker> GetSpeakers ()
		{
			return DL.MwcDatabase.GetItems<Speaker> ();
		}
		
		public static Speaker GetSpeaker (int id)
		{
			return DL.MwcDatabase.GetItem<Speaker> (id);
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
			return DL.MwcDatabase.GetItem<Exhibitor> (id);
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

	}
}

