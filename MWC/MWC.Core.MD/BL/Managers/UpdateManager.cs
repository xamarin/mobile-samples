using System;

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
		/// </summary
		public static void UpdateAll()
		{
			// make this a critical section to ensure that access is serial
			lock(_locker)
			{
				_isUpdating = true;
				
				// simulate request time
				System.Threading.Thread.Sleep ( 2500 );
				
				Console.WriteLine ("Updating all data from cloud");
				UpdateStarted (null, EventArgs.Empty);
				
				Console.WriteLine ("Updating Exhibitor Data.");
				ExhibitorManager.UpdateExhibitorData();
				
				Console.WriteLine ("Updating Session Data.");
				SessionManager.UpdateSessionData();
				
				Console.WriteLine ("Updating Speaker Data.");
				SpeakerManager.UpdateSpeakerData();
				
				UpdateFinished (null, EventArgs.Empty);
				_isUpdating = false;
			}
		}
	}
}

