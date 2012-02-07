using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MWC.BL.Managers {
	/// <summary>
	/// Central point for triggering data update from server
	/// to our local SQLite db
	/// </summary>
	public static class UpdateManager {
		private static object locker = new object();
		
		public static event EventHandler UpdateStarted = delegate {};
		public static event EventHandler UpdateFinished = delegate {};
		
		public static event EventHandler UpdateExhibitorsStarted = delegate {};
		public static event EventHandler UpdateExhibitorsFinished = delegate {};

		/// <summary>
		/// Gets or sets a value indicating whether the data is updating.
		/// </summary>
		/// <value>
		/// <c>true</c> if the data is updating; otherwise, <c>false</c>.
		/// </value>
		public static bool IsUpdating
		{
			get { return isUpdating; }
			set { isUpdating = value; }
		}
		public static bool IsUpdatingExhibitors
		{
			get { return isUpdatingExhibitors; }
			set { isUpdatingExhibitors = value; }
		}
		private static bool isUpdatingExhibitors = false;
		private static bool isUpdating = false;
		
		static UpdateManager ()
		{
		}
		
		public static void UpdateFromFile(string xmlString)
		{
			Console.WriteLine ("### Updating all data from local file");

			// make this a critical section to ensure that access is serial
			lock (locker) {
				isUpdating = true;
				UpdateStarted (null, EventArgs.Empty);
				var ea = new UpdateFinishedEventArgs (UpdateType.SeedData, false);
				
				var c = MWC.SAL.MWCSiteParser.DeserializeConference (xmlString);
				if (c != null) {
					if (SaveToDatabase (c)) {
						ea.Success = true;
					}
				}
				UpdateFinished (null, ea);
				isUpdating = false;
			}
		}
		/// <summary>
		/// Updates all conference data from the cloud. Sets UpdateManager.IsUpdating
		/// to true while updating. serialized, thread-safe access.
		/// (although, the webclient request is async...)
		/// </summary
		public static void UpdateConference()
		{
			Console.WriteLine ("### Updating all data from cloud; _isUpdating = true");
				
			// make this a critical section to ensure that access is serial
			lock (locker) {
				isUpdating = true;
				UpdateStarted (null, EventArgs.Empty);
				var ea = new UpdateFinishedEventArgs (UpdateType.Conference, false);

				var siteParser = new MWC.SAL.MWCSiteParser();
				siteParser.GetConference (Constants.ConferenceDataUrl,  
					delegate  {
						var c = siteParser.ConferenceData;

						if (c == null) {
							Console.WriteLine ("xxx No conference data downloaded, skipping");
						} else {
							if (SaveToDatabase (c)) {
								ea.Success = true;
							}
						}
						UpdateFinished (null, ea);
						isUpdating = false;
					}
				);
			}
		}

		public static void UpdateExhibitors()
		{
			Console.WriteLine ("### Updating exhibitors data from cloud; _isUpdating = true");
				
			// make this a critical section to ensure that access is serial
			lock (locker) {
				isUpdatingExhibitors = true;
				UpdateExhibitorsStarted (null, EventArgs.Empty);
				var ea = new UpdateFinishedEventArgs (UpdateType.Exhibitors, false);

				var siteParser = new MWC.SAL.MWCSiteParser();
				siteParser.GetExhibitors (Constants.ExhibitorDataUrl,  
					delegate {
						var c = siteParser.Exhibitors;

						if (c == null) {
							Console.WriteLine ("xxx No conference data downloaded, skipping");
						} else {
							if (SaveToDatabase (c)) {
								ea.Success = true;
							}
						}
						UpdateExhibitorsFinished (null, ea);
						isUpdatingExhibitors = false;
					}
				);
			}
		}

		static bool SaveToDatabase(Conference c)
		{
			bool success = false;
			try  {
				Console.WriteLine ("yyy SAVING new conference data to sqlite");
			
				if (c.Speakers.Count > 0) {
					DAL.DataManager.DeleteSpeakers ();
					DAL.DataManager.SaveSpeakers (c.Speakers);
				}
				if (c.Sessions.Count > 0) {
					DAL.DataManager.DeleteSessions ();
					DAL.DataManager.DeleteSessionSpeakers ();
					var speakers = SessionManager.GenerateKeysAndSpeakers (c.Sessions);
					DAL.DataManager.SaveSessions (c.Sessions);
					DAL.DataManager.SaveSessionSpeakers (speakers);
				}
				if (c.Exhibitors.Count > 0) {
					DAL.DataManager.DeleteExhibitors ();
					DAL.DataManager.SaveExhibitors (c.Exhibitors);	
				}
				success = true;
			} catch (Exception) {
				Console.WriteLine ("xxx SAVING conference to sqlite failed");
			}
			return success;
		}
		static bool SaveToDatabase(List<Exhibitor> exhibitors)
		{
			bool success = false;
			try  {
				Console.WriteLine ("yyy SAVING new exhibitors data to sqlite");
				if (exhibitors.Count > 0) {
					DAL.DataManager.DeleteExhibitors ();
					DAL.DataManager.SaveExhibitors (exhibitors);	
				}
				success = true;
			} catch (Exception) {
				Console.WriteLine ("xxx SAVING exhibitors to sqlite failed");
			}
			return success;
		}
	}
}