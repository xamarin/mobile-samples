using System;
using System.Collections.Generic;

namespace MWC.BL.Managers
{
	public static class ExhibitorManager
	{
		static ExhibitorManager ()
		{
		}
		
		internal static void UpdateExhibitorData(IList<Exhibitor> exhibitors)
		{
			DAL.DataManager.DeleteExhibitors();
			DAL.DataManager.SaveExhibitors (exhibitors); //SAL.MwcSiteParser.GetExhibitors ());
		}

		public static IList<Exhibitor> GetExhibitors ()
		{
			return new List<Exhibitor>(DAL.DataManager.GetExhibitors());
		}
		
		public static Exhibitor GetExhibitor (int exhibitorID)
		{
			return DAL.DataManager.GetExhibitor (exhibitorID);
		}
	}
}

