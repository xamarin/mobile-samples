using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.Dialog;
using System.Drawing;
using MWC.BL;

namespace MWC.iOS.UI.CustomElements
{
	// Renders a session
	public class SessionElement : Element {
		static NSString key = new NSString ("sessionElement");
	
		Session session;
		string subtitle;
		
		public SessionElement (Session session) : base (session.Title)
		{
			this.session = session;
			if(String.IsNullOrEmpty(session.Room))
				subtitle = String.Format ("{0}", session.SpeakerNames);
			else if (String.IsNullOrEmpty(session.SpeakerNames))
				subtitle = String.Format("{0} room", session.Room);
			else
				subtitle = String.Format ("{0} room; {1}", session.Room, session.SpeakerNames);

		}
		
		static int count;
		public override UITableViewCell GetCell (UITableView tv)
		{
			var cell = tv.DequeueReusableCell (key);
			count++;
			if (cell == null)
			{
				cell = new SessionCell (UITableViewCellStyle.Subtitle, key, session, Caption, subtitle);
			}
			else
				((SessionCell)cell).UpdateCell (session, Caption, subtitle);
			
			return cell;
		}

		public override void Selected (DialogViewController dvc, UITableView tableView, MonoTouch.Foundation.NSIndexPath path)
		{
			var sds = new MWC.iOS.Screens.Common.Session.SessionDetailsScreen (session.ID);
			dvc.ActivateController (sds);
		}
	}
}