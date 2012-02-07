using System;
using MWC.BL;
using System.Collections.Generic;
using System.Linq;

namespace MWC.BL.Managers {
	public static class SessionManager {
		static SessionManager ()
		{
		}
		
		internal static void GenerateKeys (IList<Session> sessions)
		{
			foreach (var s in sessions) {
				s.Key = s.Start.ToString("yyMMddhhmm") + s.Title;
			}
		}

		internal static void UpdateSessionData(IList<Session> sessions)
		{
			DAL.DataManager.DeleteSessions ();
            DAL.DataManager.SaveSessions(sessions); //SAL.MwcSiteParser.GetSessions () ); //TODO: figure out Exception in Android
		}

		public static IList<Session> GetSessions ()
		{
            var isessions = DAL.DataManager.GetSessions();
            return isessions.ToList(); // new List<Session>(DAL.DataManager.GetSessions()); //TODO: figure out Exception in Android
		}
		
		public static IList<Session> GetSessions ( int day )
		{
            var isessions = DAL.DataManager.GetSessions(day);
            return isessions.ToList(); // new List<Session> ( DAL.DataManager.GetSessions ( day ) );
		}

        public static IList<SessionTimeslot> GetSessionTimeslots()
        {
            var sessions = GetSessions();
            var timeslotHeaderFormat = "dddd H:mm";
            return new List<SessionTimeslot>(GroupSessionsByTimeslot(sessions, timeslotHeaderFormat));
        }

        public static IList<SessionTimeslot> GetSessionTimeslots(int day)
        {
            var sessions = GetSessions(day);
            var timeslotHeaderFormat = "H:mm";
            return new List<SessionTimeslot>(GroupSessionsByTimeslot(sessions, timeslotHeaderFormat));
        }
        /// <summary>
        /// Split sessions up into timeslot groups
        /// </summary>
        static IEnumerable<SessionTimeslot> GroupSessionsByTimeslot (IList<Session> sessions, string headerFormat)
        {
             return from session in sessions
                    group session by session.Start.Ticks into timeslot
                    orderby timeslot.Key
                    select new SessionTimeslot(
                        new DateTime(timeslot.Key).ToString(headerFormat)
                    ,
                        from eachSession in timeslot
                        select eachSession
                    );
        }
		
		public static Session GetSession (int id)
		{
			return DAL.DataManager.GetSession (id);
		}

        public static Session GetSessionWithKey (string key)
        {
            return DAL.DataManager.GetSessionWithKey (key);
        }
	}
}