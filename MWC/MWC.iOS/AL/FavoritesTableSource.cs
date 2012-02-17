using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MWC.BL;

namespace MWC.iOS.AL {
	public class FavoriteClickedEventArgs : EventArgs {
		public Session SessionClicked;
		
		public FavoriteClickedEventArgs (Session session) : base ()
		{
			this.SessionClicked = session;
		}
	}

	public class FavoritesTableSource : UITableViewSource {
		public event EventHandler<FavoriteClickedEventArgs> FavoriteClicked = delegate 
		{
		};


		IList<SessionTimeslot> groupedFavorites;
		static NSString cellId = new NSString("FavoritesCell");

		public FavoritesTableSource ()
		{
			groupedFavorites = BL.Managers.FavoritesManager.GetFavoriteTimeslots();
		}

		public override UITableViewCell GetCell (UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
		{
			MWC.iOS.UI.CustomElements.SessionCell cell = tableView.DequeueReusableCell(cellId) as MWC.iOS.UI.CustomElements.SessionCell;

			var favSession = groupedFavorites[indexPath.Section].Sessions[indexPath.Row];
			
			if(cell == null)
				cell = new MWC.iOS.UI.CustomElements.SessionCell(MonoTouch.UIKit.UITableViewCellStyle.Default
							, cellId
							, favSession
							, favSession.Title
							, favSession.Room);
			else
				cell.UpdateCell (favSession, favSession.Title, favSession.Room);

			cell.StyleForHome();

			return cell;
		}

		public override float GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
		{
			float height = 60f;
			SizeF maxSize = new SizeF (230, float.MaxValue);
			var favSession = groupedFavorites[indexPath.Section].Sessions[indexPath.Row];
			// test if we need two lines to display more of the Session.Title
			SizeF size = tableView.StringSize (favSession.Title
						, UIFont.FromName ("Helvetica-Light", AppDelegate.Font16pt)
						, maxSize);
			if (size.Height > 27) {
				height += 27;
			}
			return height;
		}

		public override int RowsInSection (UITableView tableview, int section)
		{
			return groupedFavorites[section].Sessions.Count;
		}
		
		public override string TitleForHeader (UITableView tableView, int section)
		{
			return "Favorites";
		}

		public override int NumberOfSections (UITableView tableView)
		{
			return groupedFavorites.Count;
		}	
		
		public override UIView GetViewForHeader (UITableView tableView, int section)
		{
			if (AppDelegate.IsPhone) return null;
			var headerText = groupedFavorites[section].Timeslot;
			if (section == 0) headerText = "Favorites for " + headerText;
			return DaysTableSource.BuildSectionHeaderView(headerText);
		}

		public override void RowSelected (UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
		{
			var session = groupedFavorites[indexPath.Section].Sessions[indexPath.Row];
			FavoriteClicked (this, new FavoriteClickedEventArgs (session));
			tableView.DeselectRow (indexPath, true);
		}
	}
}