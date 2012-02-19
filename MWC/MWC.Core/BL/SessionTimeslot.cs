using System;
using System.Collections.Generic;
using System.Text;

namespace MWC.BL {
    public class SessionTimeslot {
		/// <summary>
		/// Used to group sessions (inc favorites) by time, esp in a linq expression for MT.D
		/// </summary>
        public SessionTimeslot(string timeslot, IEnumerable<Session> sessions)
        {
            Timeslot = timeslot;
            Sessions = new List<Session>(sessions);
        }
        public string Timeslot { get; set; }
        public IList<Session> Sessions { get; set; }
    }
}
