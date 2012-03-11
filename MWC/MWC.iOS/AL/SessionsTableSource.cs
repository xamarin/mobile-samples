using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MWC.BL;

namespace MWC.iOS {
	public class SessionsTableSource : UITableViewSource {
		IList<Session> sessions;
		MWC.iOS.Screens.iPhone.Speakers.SpeakerDetailsScreen view;
		static NSString cellId = new NSString("SessionCell");

		public SessionsTableSource (List<Session> sessions, MWC.iOS.Screens.iPhone.Speakers.SpeakerDetailsScreen view)
		{
			this.sessions = sessions;
			this.view = view;
		}

		public override UITableViewCell GetCell (UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
		{
			var speaker = sessions[indexPath.Row];
			var cell = tableView.DequeueReusableCell(cellId);
			if(cell == null) 
				cell = new UITableViewCell (UITableViewCellStyle.Default, cellId);
			
			cell.TextLabel.Text = speaker.Title;
			return cell;
		}

		public override float GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
		{
			return 40f;
		}

		public override int RowsInSection (UITableView tableview, int section)
		{
			return sessions.Count;
		}
		
		public override string TitleForHeader (UITableView tableView, int section)
		{
			return "Sessions";
		}	

		public override void RowSelected (UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
		{
			var session = sessions[indexPath.Row];
			ConsoleD.WriteLine("SessionsTableSource.RowSelected");			
			view.SelectSession(session);
			if (AppDelegate.IsPhone) tableView.DeselectRow (indexPath, true);
		}
	}
}
