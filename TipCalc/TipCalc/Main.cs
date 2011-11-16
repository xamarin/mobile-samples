using System;

using TipCalc.Util;

namespace TipCalc
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Console.WriteLine ("Tip Calculator:");
			
			var info = new TipInfo {
				Subtotal    = ReadDecimal ("   Subtotal: "),
				Total       = ReadDecimal ("      Total: "),
				TipPercent  = ReadDecimal ("Tip Percent: "),
			};
			
			Console.WriteLine ("--");
			Console.WriteLine ("  Tip: {0}", info.TipValue);
			Console.WriteLine ("Total: {0}", info.Total + info.TipValue);
		}
		
		static decimal ReadDecimal (string prompt)
		{
			while (true) {
				string value = null;
				try {
					Console.Write (prompt);
					value = Console.ReadLine ();
					return Convert.ToDecimal (value);
				}
				catch (Exception e) {
					Console.WriteLine ("Unable to convert value '{0}' to a decimal: {1}", value, e.Message);
				}
			}
		}
	}
}
