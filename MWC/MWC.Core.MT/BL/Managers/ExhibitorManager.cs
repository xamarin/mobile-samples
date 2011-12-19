using System;

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
		
	}
}

