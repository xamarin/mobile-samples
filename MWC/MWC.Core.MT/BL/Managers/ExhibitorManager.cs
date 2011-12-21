using System;
using System.Collections.Generic;

namespace MWC.BL.Managers
{
	public static class ExhibitorManager
	{
		static ExhibitorManager ()
		{
		}
		
		public static void UpdateExhibitorData()
		{
			DAL.DataManager.SaveExhibitors (SAL.MwcSiteParser.GetExhibitors ());
		}

		public static IList<Exhibitor> GetExhibitors ()
		{
			return new List<Exhibitor>(DAL.DataManager.GetExhibitors());
		}

	}
}

