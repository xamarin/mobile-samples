using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MWC.BL.Managers
{
	/// <summary>
	/// Update manager.
	/// </summary>
	public static class UpdateManager
	{
		private static object _locker = new object();
		
		public static event EventHandler UpdateStarted = delegate {};
		public static event EventHandler UpdateFinished = delegate {};
		
		/// <summary>
		/// Gets or sets a value indicating whether the data is updating.
		/// </summary>
		/// <value>
		/// <c>true</c> if the data is updating; otherwise, <c>false</c>.
		/// </value>
		public static bool IsUpdating
		{
			get { return _isUpdating; }
			set { _isUpdating = value; }
		}
		private static bool _isUpdating = false;
		
		static UpdateManager ()
		{
		}
		
		public static void UpdateFromFile(string xmlString)
		{
			lock(_locker)
			{
				Console.WriteLine ("### Updating all data from local file");
				_isUpdating = true;
				UpdateStarted (null, EventArgs.Empty);
				
				
				var c = MWC.SAL.MWCSiteParser.DeserializeConference (xmlString);
				SaveToDatabase(c);

				UpdateFinished (null, EventArgs.Empty);
				_isUpdating = false;
			}
		}
		/// <summary>
		/// Updates all conference data from the cloud. Sets UpdateManager.IsUpdating
		/// to true while updating. serialized, thread-safe access.
		/// (although, the webclient request is async...)
		/// </summary
		public static void UpdateAll()
		{
			// make this a critical section to ensure that access is serial
			lock(_locker)
			{
				Console.WriteLine ("### Updating all data from cloud; _isUpdating = true");
				_isUpdating = true;
				UpdateStarted (null, EventArgs.Empty);
				

				var siteParser = new MWC.SAL.MWCSiteParser(Constants.ConferenceDataUrl);
				siteParser.GetConference ( 
					delegate {
						var c = siteParser.ConferenceData;

						if (c == null)
						{
							Console.WriteLine ("xxx No conference data downloaded, skipping");
							//HACK: load 'test' data (hardcoded)
							//LoadHardcodedData();
						}
						else
						{
							SaveToDatabase(c);
						}
						UpdateFinished (null, EventArgs.Empty);
						_isUpdating = false;
					}
				);
			
				UpdateFinished (null, EventArgs.Empty);
				_isUpdating = false;
				Console.WriteLine ("### _isUpdating = false");
			}
		}

		static void SaveToDatabase(Conference c)
		{
			Console.WriteLine ("yyy SAVING new conference data to sqlite");
			DAL.DataManager.DeleteSpeakers ();
			DAL.DataManager.SaveSpeakers (c.Speakers);
			DAL.DataManager.DeleteExhibitors ();
			DAL.DataManager.SaveExhibitors (c.Exhibitors);	
			DAL.DataManager.DeleteSessions ();
			DAL.DataManager.SaveSessions (c.Sessions);
		}
	}
}