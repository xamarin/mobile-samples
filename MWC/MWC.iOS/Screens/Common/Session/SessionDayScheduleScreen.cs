using System;
using System.Collections.Generic;
using System.Linq; // required for linq
using MonoTouch.Dialog;
using MonoTouch.UIKit;
using MWC.iOS.Screens.iPad.Sessions;

namespace MWC.iOS.Screens.Common.Session {
	public partial class SessionDayScheduleScreen : DialogViewController {
		protected IList<BL.Session> sessions;
		string dayName;
		SessionSplitView splitView;
		
			/// <summary>
		/// Display sessions for the day, grouped by time slot
		/// </summary>
		public SessionDayScheduleScreen (string dayName, int day, SessionSplitView sessionSplitView) 
		: base (UITableViewStyle.Plain, null)
		{
			splitView = sessionSplitView;

			this.sessions = BL.Managers.SessionManager.GetSessions ( day );
			this.dayName = dayName;
			this.Title = this.dayName;

			Root = 	new RootElement (this.dayName) {
					from s in this.sessions
						group s by s.Start.Ticks into g
						orderby g.Key
						select new Section (new DateTime (g.Key).ToString("dddd HH:mm") ) {
						from hs in g
						   select (Element) new MWC.iOS.UI.CustomElements.SessionElement (hs, splitView)
			}};

		}

		public override DialogViewController.Source CreateSizingSource (bool unevenRows)
		{
			return new MWC.iOS.Screens.iPhone.Sessions.SessionsTableSource(this);
		}
	}
}