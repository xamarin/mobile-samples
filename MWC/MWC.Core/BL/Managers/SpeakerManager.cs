using System;
using System.Collections.Generic;
using System.Linq;

namespace MWC.BL.Managers
{
	public static class SpeakerManager
	{
		static SpeakerManager ()
		{
		}

		internal static void UpdateSpeakerData(IList<Speaker> speakers)
		{
			DAL.DataManager.DeleteSpeakers ();
			DAL.DataManager.SaveSpeakers (speakers); //SAL.MwcSiteParser.GetSpeakers ());			
		}
	
		public static IList<Speaker> GetSpeakers ()
		{
            var ispeakers = DAL.DataManager.GetSpeakers();
            var speakers = ispeakers.ToList(); // new List<Speaker>(ispeakers); //TODO: figure out Exception in Android
			speakers.Sort( (s1, s2) => s1.Name.CompareTo (s2.Name));
			return speakers;
		}
	
		public static Speaker GetSpeaker (int speakerID)
		{
			return DAL.DataManager.GetSpeaker ( speakerID );
		}

        public static Speaker GetSpeakerWithKey (string speakerKey)
        {
            return DAL.DataManager.GetSpeakerWithKey (speakerKey);
        }
	}
}

