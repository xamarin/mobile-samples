using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MWC.BL;

namespace MWC.BL
{
	public static class ExhibitorManager
	{
		public static List<Exhibitor> GetExhibitorList(bool doPartial)
		{
			return SAL.ExhibitorManager.GetExhibitorList(doPartial);
		}
	}
}
