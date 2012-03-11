using System;

namespace MWC.iOS {
	/// <summary>
	/// DEBUG version of ConsoleD.WriteLine - no output happens in RELEASE
	/// </summary>
	/// <remarks>
	/// There was a change in iOS5.1 related to how MT handles Console.WriteLine.
	/// It is resolved in MT 5.2.4, however just in case this code is downloaded
	/// and used with an older version, having the Console.WriteLine limited to 
	/// Debug builds will prevent a release version from misbehaving with iOS5.1
	/// </remarks>
	public class ConsoleD {
		public ConsoleD ()
		{
		}

		[System.Diagnostics.Conditional("DEBUG")]
		public static void WriteLine(string format, params object[] arg)
		{
			Console.WriteLine(format, arg);
		}
		[System.Diagnostics.Conditional("DEBUG")]
		public static void WriteLine(string message)
		{
			Console.WriteLine(message);
		}
	}
}