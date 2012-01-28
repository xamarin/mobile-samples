using System;
using System.Collections.Generic;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using System.Drawing;
using MWC.BL;
using System.Linq;

namespace MWC.iOS.AL
{
	public class FavoritesTableSource : UITableViewSource
	{
		IList<Session> _favorites;
		static NSString _cellId = new NSString("FavoritesCell");

		public FavoritesTableSource ()
		{
			var sessions = BL.Managers.SessionManager.GetSessions ();
			var favs = BL.Managers.FavoritesManager.GetFavorites();
			// extract IDs from Favorites query
			List<string> favoriteIDs = new List<string>();
			foreach (var f in favs) favoriteIDs.Add (f.SessionKey);
			_favorites = (from s in sessions
							where favoriteIDs.Contains(s.Key)
							select s).ToList();
		}

		public override UITableViewCell GetCell (UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
		{
			MWC.iOS.UI.CustomElements.SessionCell cell = tableView.DequeueReusableCell(_cellId) as MWC.iOS.UI.CustomElements.SessionCell;
			var favSession = _favorites[indexPath.Row];
			if(cell == null)
				cell = new MWC.iOS.UI.CustomElements.SessionCell(MonoTouch.UIKit.UITableViewCellStyle.Default
							, _cellId
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
			return this._favorites.Count;
		}
		
		public override UIView GetViewForHeader (UITableView tableView, int section)
		{
			var label = new UILabel();
	        label.BackgroundColor = UIColor.Clear;
	        label.Text = "Favorites";
	
	        return label;
		}
		public override float GetHeightForHeader (UITableView tableView, int section)
		{
			return 18;
		}
		public override int NumberOfSections (UITableView tableView)
		{
			return 1;
		}	
	}
}

