using System;
using System.Collections.Generic;
using System.Text;

namespace MWC.BL
{
    public class SessionTimeslot
    {
        public SessionTimeslot(string timeslot, IEnumerable<Session> sessions)
        {
            Timeslot = timeslot;
            Sessions = new List<Session>(sessions);
        }
        public string Timeslot { get; set; }
        public IList<Session> Sessions { get; set; }
    }
}
