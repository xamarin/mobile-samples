using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MWC.BL
{
	public static class ConferenceManager
	{
		public static Conference GetConference()
		{
			Conference conf = new Conference();

			var sessions = SessionManager.GetSessionList();
			conf.Sessions = sessions;

			foreach(var session in sessions){
				conf.Speakers.AddRange(session.SpeakerList);
			}
			
			conf.Exhibitors = ExhibitorManager.GetExhibitorList();

			return conf;
		}
	}
}
