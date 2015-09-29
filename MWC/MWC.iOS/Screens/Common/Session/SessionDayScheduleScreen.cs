using System;
using System.Collections.Generic;
using System.Linq; // required for linq
using MonoTouch.Dialog;
using UIKit;
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
		: base (UITableViewStyle.Plain, null, true)
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
		
		/// <summary>
		/// Keep favorite-stars in sync with changes made on other screens
		/// </summary>
		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			
			// sync the favorite stars if they change in other views (also SessionsScreen)
			foreach (var sv in TableView.Subviews) {
				var cell = sv as MWC.iOS.UI.CustomElements.SessionCell;
				if (cell != null) {
					cell.UpdateFavorite();
				}	
			}
		}
	}
}