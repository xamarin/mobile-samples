using System;
using System.Collections.Generic;

namespace MWC
{
	public class Constants
	{
		public Constants () {}
		
		public static IList<String> DayNames = new List<String>() { "Monday", "Tuesday", "Wednesday", "Thursday" };
		public static DateTime StartDateMin = new DateTime ( 2012, 02, 27, 0, 0, 0 );
		public static DateTime StartDateMax = new DateTime ( 2012, 02, 27, 23, 59, 59 );

		public const string NewsBaseUrl = "news.google.com"; // for Reachability test
		public const string NewsUrl = "http://news.google.com/news?q=mobile%20world%20congress&output=rss";
		public const string TwitterUrl = "http://search.twitter.com/search.atom?q=%40mobileworldlive&show-user=true&rpp=20";
	}
}

