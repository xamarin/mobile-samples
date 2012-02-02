using System;

namespace MWC.iOS.AL {
	public class DayClickedEventArgs : EventArgs {
		public int Day;
		public string DayName;
		
		public DayClickedEventArgs (string dayName, int day) : base ()
		{
			this.DayName = dayName;
			this.Day = day;
		}
	}
}

