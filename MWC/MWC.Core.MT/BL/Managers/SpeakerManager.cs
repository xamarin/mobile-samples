using System;
using System.Collections.Generic;

namespace MWC.BL.Managers
{
	public static class SpeakerManager
	{
		static SpeakerManager ()
		{
		}

		public static void UpdateSpeakerData()
		{
			DAL.DataManager.DeleteSpeakers ();
			DAL.DataManager.SaveSpeakers (SAL.MwcSiteParser.GetSpeakers ());			
		}
	
		public static IList<Speaker> GetSpeakers ()
		{
			return new List<Speaker> ( DAL.DataManager.GetSpeakers () );
		}
	}
}

