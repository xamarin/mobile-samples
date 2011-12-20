using System;
using MWC.BL;

namespace MWC.iOS
{
	public class SessionClickedEventArgs : EventArgs
	{
		public Session Session { get; set; }
		
		public SessionClickedEventArgs (Session session) : base ()
		{
			this.Session = session;
		}
	}
}

