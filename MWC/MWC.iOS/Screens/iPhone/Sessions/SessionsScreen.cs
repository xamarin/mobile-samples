using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.Dialog;
using MWC.BL;
using MWC.iOS.Screens.Common.Session;

namespace MWC.iOS.Screens.iPhone.Sessions
{
	/// <summary>
	/// Speakers screen. Derives from MonoTouch.Dialog's DialogViewController to do 
	/// the heavy lifting for table population.
	/// </summary>
	public partial class SessionsScreen : UpdateManagerLoadingDialogViewController
	{
		protected IList<BL.Session> _sessions;
		
		public SessionsScreen () : base ()
		{
		}

		/// <summary>
		/// Populates the page with sessions, grouped by time slot
		/// </summary>
		protected override void PopulateTable()
		{
			// get the sessions from the database
			_sessions = BL.Managers.SessionManager.GetSessions ();
			
			Root = 	new RootElement ("Sessions") {
					from session in _sessions
						group session by session.Start.Ticks into timeslot
						orderby timeslot.Key
						select new Section (new DateTime (timeslot.Key).ToString("dddd HH:mm") ) {
						from eachSession in timeslot
						   select (Element) new MWC.iOS.UI.CustomElements.SessionElement (eachSession)
			}};
		}	
	
		public override DialogViewController.Source CreateSizingSource (bool unevenRows)
		{
			return new SessionsTableSource(this);
		}
	}

	/// <summary>
	/// Implement custom row height
	/// </summary>
	public class SessionsTableSource : DialogViewController.SizingSource
	{
		public SessionsTableSource (DialogViewController dvc) : base(dvc)
		{}

		public override float GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
		{
			return 60f;
		}
	}
}