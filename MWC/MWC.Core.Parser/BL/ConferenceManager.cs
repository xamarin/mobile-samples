using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MWC.BL
{
	public static class ConferenceManager
	{
		public static Conference GetConference(bool doPartial)
		{
			Conference conf = new Conference();

			var sessions = SessionManager.GetSessionList(doPartial);
			conf.Sessions = sessions;

			foreach(var session in sessions)
			{
				conf.Speakers.AddRange(session.SpeakerList);
			}
			
			//conf.Exhibitors = ExhibitorManager.GetExhibitorList(doPartial);

			return conf;
		}
	}
}
