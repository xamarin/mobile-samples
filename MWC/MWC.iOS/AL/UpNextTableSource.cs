using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MWC.BL;

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
			now = new DateTime(2012,2,29,11,51,0);
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
	
			var _upnextGroup = allUpcoming.FirstOrDefault ();
			upNext = _upnextGroup.Sessions.ToList();
			upNextTime = new DateTime(_upnextGroup.Start);
		}

		public override UITableViewCell GetCell (UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
		{
			MWC.iOS.UI.CustomElements.SessionCell cell = tableView.DequeueReusableCell(cellId) as MWC.iOS.UI.CustomElements.SessionCell;
			var favSession = upNext[indexPath.Row];
			if (cell == null)
				cell = new MWC.iOS.UI.CustomElements.SessionCell(MonoTouch.UIKit.UITableViewCellStyle.Default
							, cellId
							, favSession
							, favSession.Title, favSession.Room);
			else
				cell.UpdateCell (favSession, favSession.Title, favSession.Room);
			return cell;
		}

		public override float GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
		{
			return 60;
		}

		public override int RowsInSection (UITableView tableview, int section)
		{
			return this.upNext.Count;
		}
		
		public override string TitleForHeader (UITableView tableView, int section)
		{
			return "Up Next at " + upNextTime.ToString ("H:mm dddd");
		}

		public override UIView GetViewForHeader (UITableView tableView, int section)
		{
			if (AppDelegate.IsPhone) return null;
			var title = "Up Next at " + upNextTime.ToString ("H:mm dddd");
			return DaysTableSource.BuildSectionHeaderView(title);
		}

		public override int NumberOfSections (UITableView tableView)
		{
			return 1;
		}	

		public override void RowSelected (UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
		{
			this.SessionClicked (this, new FavoriteClickedEventArgs (this.upNext [indexPath.Row]));
			tableView.DeselectRow (indexPath, true);
		}
	}
}