using System;
using System.Collections.Generic;
using System.Linq;

namespace MWC.BL.Managers {
	public static class FavoritesManager {

		static FavoritesManager ()
		{
		}
		
		public static IList<Favorite> GetFavorites ()
		{
			return new List<Favorite> (DAL.DataManager.GetFavorites ());
		}
		public static bool IsFavorite(string sessionKey)
		{
			return DAL.DataManager.GetIsFavorite (sessionKey);
		}
		public static void AddFavoriteSession(Favorite favorite)
		{
            DAL.DataManager.SaveFavorite(favorite);
		}
		public static void RemoveFavoriteSession(string sessionKey)
		{
			DAL.DataManager.DeleteFavorite (sessionKey);
		}
		
		public static IList<SessionTimeslot> GetFavoriteTimeslots()
        {
			var sessions = SessionManager.GetSessions();
            var timeslotHeaderFormat = "dddd H:mm";
            return new List<SessionTimeslot>(GroupSessionsByTimeslot(sessions, timeslotHeaderFormat));
		}
		/// <summary>
        /// Split favorite sessions up into timeslot groups
        /// </summary>
        static IEnumerable<SessionTimeslot> GroupSessionsByTimeslot (IList<Session> sessions, string headerFormat)
        {
			var favs = BL.Managers.FavoritesManager.GetFavorites();
			// extract IDs from Favorites query
			List<string> favoriteIDs = new List<string>();
			foreach (var f in favs) favoriteIDs.Add (f.SessionKey);

             return from session in sessions
					where favoriteIDs.Contains(session.Key)
                    group session by session.Start.Ticks into timeslot
                    orderby timeslot.Key
                    select new SessionTimeslot(
                        new DateTime(timeslot.Key).ToString(headerFormat)
                    ,
                        from eachSession in timeslot
                        select eachSession
                    );
        }
	}
}