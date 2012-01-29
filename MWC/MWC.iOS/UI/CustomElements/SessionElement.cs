using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.Dialog;
using System.Drawing;
using MWC.BL;

namespace MWC.iOS.UI.CustomElements
{
	/// <summary>
	/// Session element, used on both iPhone (full screen)
	/// and iPad (in a popover). 
	/// </summary>
	public class SessionElement : Element {
		static NSString key = new NSString ("sessionElement");
	
		Session _session;
		string _subtitle;
		/// <summary>If this is null, on iPhone; otherwise on iPad</summary>
		MWC.iOS.Screens.iPad.Sessions.SessionSplitView _splitView;
		
		/// <summary>for iPhone</summary>
		public SessionElement (Session session) : base (session.Title)
		{
			this._session = session;
			if(String.IsNullOrEmpty(session.Room))
				_subtitle = String.Format ("{0}", session.SpeakerNames);
			else if (String.IsNullOrEmpty(session.SpeakerNames))
				_subtitle = String.Format("{0} room", session.Room);
			else
				_subtitle = String.Format ("{0} room; {1}", session.Room, session.SpeakerNames);

		}
		/// <summary>for iPad (SplitViewController)</summary>
		public SessionElement (Session session, MWC.iOS.Screens.iPad.Sessions.SessionSplitView splitView) : this (session)
		{
			this._splitView = splitView;
		}

		static int count;
		public override UITableViewCell GetCell (UITableView tv)
		{
			var cell = tv.DequeueReusableCell (key);
			count++;
			if (cell == null)
			{
				cell = new SessionCell (UITableViewCellStyle.Subtitle, key, _session, Caption, _subtitle);
			}
			else
				((SessionCell)cell).UpdateCell (_session, Caption, _subtitle);
			
			return cell;
		}
		
		/// <summary>
		/// Behaves differently depending on iPhone or iPad
		/// </summary>
		public override void Selected (DialogViewController dvc, UITableView tableView, MonoTouch.Foundation.NSIndexPath path)
		{
			if (_splitView != null)
			{
				_splitView.ShowSession(_session.ID);
			}
			else
			{
				var sds = new MWC.iOS.Screens.Common.Session.SessionDetailsScreen (_session.ID);
				dvc.ActivateController (sds);
			}
		}
	}
}