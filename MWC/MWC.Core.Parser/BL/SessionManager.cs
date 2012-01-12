using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MWC.BL
{
	public static class SessionManager
	{
		public static List<Session> GetSessionList(bool doPartial)
		{
			return SAL.SessionManager.GetSessionList(doPartial);
		}
	}
}
