using System;

namespace MWC.BL.Managers
{
	public static class SpeakerManager
	{
		static SpeakerManager ()
		{
		}

		public static void UpdateSpeakerData()
		{
			DAL.DataManager.SaveSpeakers (SAL.MwcSiteParser.GetSpeakers ());			
		}
	
	}
}

