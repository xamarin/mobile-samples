using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Foundation;
using UIKit;
using MWC.BL;
using CoreGraphics;

namespace MWC.iOS.AL {
	public class UpNextTableSource : UITableViewSource {
		public event EventHandler<FavoriteClickedEventArgs> SessionClicked = delegate 
		{
		};
		
		DateTime upNextTime;
		IList<Session> upNext;
		static NSString cellId = new NSString("UpNextCell");

		public UpNextTableSource ()
		{
			var sessions = BL.Managers.SessionManager.GetSessions ();
			var now = DateTime.Now;
#if DEBUG
			//TEST: for 'up next' code
			//now = new DateTime(2012,3,28,9,11,0);
			now = new DateTime(2012,2,28,9,11,0);
#endif	
			
			var nowStart = now.AddMinutes ( - (now.Minute % 5)); // round to 5 minutes
//			var happeningNow = from s in sessions
//				where s.Start <= now 
//				&& now < (s.End == DateTime.MinValue ? s.Start.AddHours(1): s.End) 
//				//&& (s.Start.Minute % 10 == 0 || s.Start.Minute % 15 == 0) // fix for short-sessions (which start at :05 after the hour)
//				select s;
//			_upnext = happeningNow.ToList();

			var nextStart = nowStart.AddMinutes (14); // possible fix to (within 30 minutes bug)
			nextStart = nextStart.AddSeconds(-nextStart.Second);
			
			var allUpcoming = from s in sessions
					where s.Start >= nextStart //&& (s.Start.Minute % 10 == 0 || s.Start.Minute % 15 == 0)
					orderby s.Start.Ticks
					group s by s.Start.Ticks into g
					select new { Start = g.Key, Sessions = g };
	
			var upnextGroup = allUpcoming.FirstOrDefault ();
			if (upnextGroup != null) { // conference is over
				upNext = upnextGroup.Sessions.ToList();
				upNextTime = new DateTime(upnextGroup.Start);
			}
		}

		public override UITableViewCell GetCell (UITableView tableView, Foundation.NSIndexPath indexPath)
		{
			MWC.iOS.UI.CustomElements.SessionCell cell = tableView.DequeueReusableCell(cellId) as MWC.iOS.UI.CustomElements.SessionCell;
			var favSession = upNext[indexPath.Row];
			if (cell == null)
				cell = new MWC.iOS.UI.CustomElements.SessionCell(UIKit.UITableViewCellStyle.Default
							, cellId
							, favSession
							, favSession.Title, favSession.Room);
			else
				cell.UpdateCell (favSession, favSession.Title, favSession.Room);

			cell.StyleForHome();

			return cell;
		}

		public override nfloat GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
		{
			float height = 60f;
			CGSize maxSize = new SizeF (230, float.MaxValue);
			var favSession = upNext[indexPath.Row];
			// test if we need two lines to display more of the Session.Title
			CGSize size = tableView.StringSize (favSession.Title
						, UIFont.FromName ("Helvetica-Light", AppDelegate.Font16pt)
						, maxSize);
			if (size.Height > 27) {
				height += 27;
			}
			return height;
		}

		public override nint RowsInSection (UITableView tableview, nint section)
		{
			if (upNext == null) return 0; // conference is over
			return upNext.Count;
		}
		
		public override string TitleForHeader (UITableView tableView, nint section)
		{
			if (upNext == null) return ""; // conference is over
			return "Up Next at " + upNextTime.ToString ("H:mm dddd");
		}

		public override UIView GetViewForHeader (UITableView tableView, nint section)
		{
			if (AppDelegate.IsPhone) return null; // phone doesn't have header
			if (upNext == null) return null; // conference is over
			var title = "Up Next at " + upNextTime.ToString ("H:mm dddd");
			return DaysTableSource.BuildSectionHeaderView(title);
		}

		public override nint NumberOfSections (UITableView tableView)
		{
			return 1;
		}	

		public override void RowSelected (UITableView tableView, Foundation.NSIndexPath indexPath)
		{
			this.SessionClicked (this, new FavoriteClickedEventArgs (this.upNext [indexPath.Row]));
			tableView.DeselectRow (indexPath, true);
		}
	}
}