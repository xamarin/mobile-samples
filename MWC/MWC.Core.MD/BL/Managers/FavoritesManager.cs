using System;
using System.Collections.Generic;

namespace MWC.BL.Managers
{
	public static class FavoritesManager
	{
		static FavoritesManager () {}
		
		public static IList<Favorite> GetFavorites ()
		{
			return new List<Favorite> ( DAL.DataManager.GetFavorites () );
		}
		public static bool IsFavorite(string sessionName)
		{
			return DAL.DataManager.GetIsFavorite (sessionName);
		}
		public static void AddFavoriteSession(Favorite favorite)
		{
            DAL.DataManager.SaveFavorite(favorite);
		}
		public static void RemoveFavoriteSession(string sessionName)
		{
			DAL.DataManager.DeleteFavorite (sessionName);
		}
	}
}