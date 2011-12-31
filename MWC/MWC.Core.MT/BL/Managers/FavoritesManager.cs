using System;

namespace MWC.BL.Managers
{
	public static class FavoritesManager
	{
		static FavoritesManager () {}

		public static bool IsFavorite(string sessionName)
		{
			return DAL.DataManager.GetIsFavorite (sessionName);
		}
		public static void AddFavoriteSession(string sessionName)
		{
			DAL.DataManager.SaveFavorite (sessionName);
		}
		public static void RemoveFavoriteSession(string sessionName)
		{
			DAL.DataManager.DeleteFavorite (sessionName);
		}
	}
}