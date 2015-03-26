using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using UIKit;
using MonoTouch.Dialog;
using MWC.BL;
using MWC.iOS.Screens.Common.Session;
using MWC.iOS.UI.CustomElements;

namespace MWC.iOS.Screens.iPhone.Favorites {
	/// <summary>
	/// Favorites list, shows ONLY sessions that are user-favorites
	/// </summary>
	public partial class FavoritesScreen : UpdateManagerLoadingDialogViewController {
		protected SessionDetailsScreen _sessionDetailsScreen;

		public FavoritesScreen () : base () 
		{
			AlwaysRefresh = true;
		}
		
		/// <summary>
		/// Populates the page with sessions, grouped by time slot
		/// that are marked as 'favorite'
		/// </summary>
		protected override void PopulateTable()
		{
			// get the sessions from the database
			var sessions = BL.Managers.SessionManager.GetSessions ();
			var favs = BL.Managers.FavoritesManager.GetFavorites();
			// extract IDs from Favorites query
			List<string> favoriteIDs = new List<string>();
			foreach (var f in favs) favoriteIDs.Add (f.SessionKey);

			// build list, matching the Favorite.SessionKey with actual
			// Session.Key rows - which might have moved if the data changed
			var root = 	new RootElement ("Favorites") {
						from s in sessions
							where favoriteIDs.Contains(s.Key)
							group s by s.Start.Ticks into g
							orderby g.Key
							select new Section (new DateTime (g.Key).ToString ("dddd HH:mm")) {
							from hs in g
							   select (Element) new SessionElement (hs)
			}};	
			
			if(root.Count == 0) {
				var section = new Section("No favorites") {
					new StyledStringElement("Touch the star to favorite a session") 
				};
				root.Add(section);
			}
			Root = root;
		}	
		
		// scroll back to the point where you last were in the list
		NSIndexPath lastScrollY;
		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);
			lastScrollY = TableView.IndexPathForSelectedRow;
			
		}
		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			var sec = TableView.NumberOfSections();
			var row = TableView.NumberOfRowsInSection(sec);
			if (lastScrollY != null && lastScrollY.Section < sec && lastScrollY.Row < row)
				TableView.ScrollToRow (lastScrollY, UITableViewScrollPosition.Middle, false);
		}
	}
}