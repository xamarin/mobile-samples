using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.Dialog;
using MWC.BL;

namespace MWC.iOS.Screens.Common.Session
{
	public partial class SessionDayScheduleScreen : DialogViewController
	{
		protected SessionDetailsScreen _sessionDetailsScreen;
		protected IList<BL.Session> _sessions;
		string _dayName;
		//int _day;
		
		public SessionDayScheduleScreen ( string dayName, int day) : base (UITableViewStyle.Grouped, null)
		{
			this._sessions = BL.Managers.SessionManager.GetSessions ( day );
			this._dayName = dayName;
			//this._day = day;
			this.Title = this._dayName;
			
			Section section;
			MWC.iOS.UI.CustomElements.SessionElement sessionElement;
			
			Root = new RootElement (this._dayName); 
	
			//TODO: group by time. this can all be built out using a fancy LINQ statement
			foreach ( var session in this._sessions )
			{
				var currentSession = session; // cloj
				section = new Section() { Caption = session.Start.ToShortTimeString() };
				sessionElement = new MWC.iOS.UI.CustomElements.SessionElement ( currentSession );
				section.Add ( sessionElement );
				Root.Add ( section );
			}
			
		}
	}
}
