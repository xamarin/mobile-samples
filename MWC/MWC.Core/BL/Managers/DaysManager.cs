using System;
using System.Collections.Generic;

namespace MWC
{
	/// <summary>
	/// Okay, maybe overkill, but it's nice to have a simple way
	/// to get the list of days for the conference that's xplatform
	/// </summary>
	public static class DaysManager
	{
		static DaysManager ()
		{
		}

		public static List<DateTime> GetDays ()
		{
			DateTime dayMin = Constants.StartDateMin; //new DateTime ( 2012, 02, 27, 0, 0, 0 );
			DateTime dayMax = Constants.EndDateMax;
			
			var days = new List<DateTime>();
			for (var i = 0; dayMin.AddDays (i) < dayMax; i++)
			{
				days.Add (dayMin.AddDays (i));
			}
			
			return days;
		}

	}
}