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
		
		public static Session GetItem (int id)
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
		
		#endregion

	}
}

