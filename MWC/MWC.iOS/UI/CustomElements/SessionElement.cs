using System;
using Foundation;
using UIKit;
using MonoTouch.Dialog;
using System.Drawing;
using MWC.BL;
using CoreGraphics;

namespace MWC.iOS.UI.CustomElements
{
	/// <summary>
	/// Session element, used on both iPhone (full screen)
	/// and iPad (in a popover). 
	/// </summary>
	public class SessionElement : Element, IElementSizing {
		static NSString cellId = new NSString ("sessionElement");
	
		Session session;
		string subtitle;
		/// <summary>If this is null, on iPhone; otherwise on iPad</summary>
		MWC.iOS.Screens.iPad.Sessions.SessionSplitView splitView;
		
		/// <summary>for iPhone</summary>
		public SessionElement (Session showSession) : base (showSession.Title)
		{
			this.session = showSession;
			if (String.IsNullOrEmpty(session.Room))
				subtitle = String.Format ("{0}", session.SpeakerNames);
			else if (String.IsNullOrEmpty(session.SpeakerNames))
				subtitle = String.Format ("{0} room", session.Room);
			else
				subtitle = String.Format ("{0} room; {1}", session.Room, session.SpeakerNames);

		}
		/// <summary>for iPad (SplitViewController)</summary>
		public SessionElement (Session session, MWC.iOS.Screens.iPad.Sessions.SessionSplitView sessionSplitView) : this (session)
		{
			splitView = sessionSplitView;
		}

		static int count;
		public override UITableViewCell GetCell (UITableView tv)
		{
			var cell = tv.DequeueReusableCell (cellId);
			count++;
			if (cell == null)
				cell = new SessionCell (UITableViewCellStyle.Subtitle, cellId, session, Caption, subtitle);
			else
				((SessionCell)cell).UpdateCell (session, Caption, subtitle);
			
			return cell;
		}
		
		public nfloat GetHeight (UITableView tableView, NSIndexPath indexPath)
		{
			float height = 60f;
			CGSize maxSize = new SizeF (262, float.MaxValue); //273
			
			// test if we need two lines to display more of the Session.Title
			CGSize size = tableView.StringSize (session.Title
						, UIFont.FromName ("Helvetica-Light", AppDelegate.Font16pt)
						, maxSize);
			if (size.Height > 27) {
				height += 27;
			}
			
			return height;
		}
		
		/// <summary>Implement MT.D search on title</summary>
		public override bool Matches (string text)
		{
			return (session.Title).ToLower ().IndexOf (text.ToLower ()) >= 0;
		}

		/// <summary>
		/// Behaves differently depending on iPhone or iPad
		/// </summary>
		public override void Selected (DialogViewController dvc, UITableView tableView, Foundation.NSIndexPath path)
		{
			if (splitView != null)
				splitView.ShowSession(session.ID);
			else {
				var sds = new MWC.iOS.Screens.iPhone.Sessions.SessionDetailsScreen (session.ID);
				sds.Title = "Session";
				dvc.ActivateController (sds);
			}
		}
	}
}