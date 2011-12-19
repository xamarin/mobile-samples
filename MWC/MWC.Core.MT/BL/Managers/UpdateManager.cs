using System;

namespace MWC.BL.Managers
{
	public static class UpdateManager
	{
		
		static UpdateManager ()
		{
			
		}
		
		public static void UpdateAll()
		{
			ExhibitorManager.UpdateExhibitorData();
			SessionManager.UpdateSessionData();
			SpeakerManager.UpdateSpeakerData();
		}
		
		
	}
}

