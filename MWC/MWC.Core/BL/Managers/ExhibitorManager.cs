using System;
using System.Collections.Generic;
using System.Linq;

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
            var iexhibitors = DAL.DataManager.GetExhibitors();
            return iexhibitors.ToList(); // new List<Exhibitor>();
		}
		
		public static Exhibitor GetExhibitor (int exhibitorID)
		{
			return DAL.DataManager.GetExhibitor (exhibitorID);
		}

        public static Exhibitor GetExhibitorWithName (string exhibitorName)
        {
            return DAL.DataManager.GetExhibitorWithName (exhibitorName);
        }
	}
}

