using System;

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
		
	}
}

