using System;
using MWC.BL;
using System.Collections.Generic;

namespace MWC.BL.Managers
{
	public static class SessionManager
	{
		static SessionManager ()
		{
		}
		
		public static void UpdateSessionData()
		{
			DAL.DataManager.SaveSessions (SAL.MwcSiteParser.GetSessions ());
		}

		public static IList<Session> GetSessions ()
		{
			return new List<Session>(DAL.DataManager.GetSessions());
		}
		
	}
}

