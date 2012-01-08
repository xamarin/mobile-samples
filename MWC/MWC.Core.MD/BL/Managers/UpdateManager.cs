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
				Console.WriteLine ("Updating all data from cloud");
				_isUpdating = true;
				var siteParser = new MWC.SAL.MWCSiteParser(Constants.ConferenceDataUrl);
				siteParser.GetConference ( 
					delegate {
						var c = siteParser.ConferenceData;

						if (c == null)
						{
							Console.WriteLine ("No conference data downloaded, using HACK");
							//HACK: load 'test' data (hardcoded)
							LoadHardcodedData();
						}
						else
						{
							Console.WriteLine ("SAVING new conference data to sqlite");
							DAL.DataManager.DeleteSpeakers ();
							DAL.DataManager.SaveSpeakers (c.Speakers);
							DAL.DataManager.DeleteExhibitors ();
							DAL.DataManager.SaveExhibitors (c.Exhibitors);	
							DAL.DataManager.DeleteSessions ();
							DAL.DataManager.SaveSessions (c.Sessions);
						}
						UpdateFinished (null, EventArgs.Empty);
						_isUpdating = false;
					}
				);
			
				UpdateFinished (null, EventArgs.Empty);
				
				_isUpdating = false;

				//HACK: get test file to use for testing :)
//				Console.WriteLine ("Creating test serialized XML file.");
//				var conf = new Conference();
//				conf.Exhibitors = new List<MWC.BL.Exhibitor>(ExhibitorManager.GetExhibitors ());
//				conf.Sessions = new List<Session>(SessionManager.GetSessions ());
//				conf.Speakers = new List<Speaker>(SpeakerManager.GetSpeakers ());
//				XmlSerializer serializerXml = new XmlSerializer(typeof(Conference));
//	            System.IO.TextWriter writer = new System.IO.StreamWriter(
//					Environment.GetFolderPath (Environment.SpecialFolder.Personal) + "/mwc11.xml");
//	            serializerXml.Serialize(writer, conf);
//	            writer.Close();
//				Console.WriteLine("Finished creating test file.");
			}
		}

		//HACK: load test data
		static void LoadHardcodedData()
		{
			// simulate request time
			System.Threading.Thread.Sleep ( 2500 );
			
			UpdateStarted (null, EventArgs.Empty);
			
			Console.WriteLine ("Updating Exhibitor Data.");
			ExhibitorManager.UpdateExhibitorData(SAL.MWCSiteParser.GetExhibitors ());
			
			Console.WriteLine ("Updating Session Data.");
			SessionManager.UpdateSessionData(SAL.MWCSiteParser.GetSessions ());
			
			Console.WriteLine ("Updating Speaker Data.");
			SpeakerManager.UpdateSpeakerData(SAL.MWCSiteParser.GetSpeakers ());
		}
	}
}
