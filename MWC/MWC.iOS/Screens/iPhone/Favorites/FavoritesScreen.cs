using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.Dialog;
using MWC.BL;
using MWC.iOS.Screens.Common.Session;
using MWC.iOS.UI.CustomElements;

namespace MWC.iOS.Screens.iPhone.Favorites
{
	/// <summary>
	/// Favorites list, shows ONLY sessions that are user-favorites
	/// </summary>
	public partial class FavoritesScreen : DialogViewController
	{
		protected SessionDetailsScreen _sessionDetailsScreen;

		public FavoritesScreen () : base (UITableViewStyle.Grouped, null) {}
		
		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);

			if(BL.Managers.UpdateManager.IsUpdating)
			{
				Console.WriteLine("Waiting for updates to finish (sessions screen)");
				BL.Managers.UpdateManager.UpdateFinished += (sender, e) => {
					Console.WriteLine("Updates finished, going to populate sessions screen.");
					this.InvokeOnMainThread ( () => { this.PopulatePage(); } );
					//TODO: unsubscribe from static event so GC can clean
				};
			}
			else
			{
				Console.WriteLine("not updating, populating favorite sessions.");
				this.PopulatePage();
			}
		}
		

		/// <summary>
		/// Populates the page with sessions, grouped by time slot
		/// that are marked as 'favorite'
		/// </summary>
		public void PopulatePage()
		{
			// get the sessions from the database
			var sessions = BL.Managers.SessionManager.GetSessions ();
			var favs = BL.Managers.FavoritesManager.GetFavorites();
			List<string> favoriteIDs = new List<string>();
			foreach (var f in favs) favoriteIDs.Add (f.SessionName);

			var root = 	new RootElement ("Favorites") {
						from s in sessions
							where favoriteIDs.Contains(s.Title)
							group s by s.Start.Ticks into g
							orderby g.Key
							select new Section (new DateTime (g.Key).ToString ("dddd HH:mm")) {
							from hs in g
							   select (Element) new SessionElement (hs)
			}};	
			
			if(favs.Count == 0)
			{
				var section = new Section("Whoops, 'favorite' a few sessions first!");
				root.Add(section);
			}
			
			Root = root;
		}		
	}
}