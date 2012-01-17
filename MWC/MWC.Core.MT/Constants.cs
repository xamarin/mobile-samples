using System;
using System.Collections.Generic;

namespace MWC
{
	public class Constants
	{
		public Constants () {}
		
		public static DateTime StartDateMin = new DateTime ( 2012, 02, 27, 0, 0, 0 );
		public static DateTime EndDateMax = new DateTime ( 2012, 03, 1, 23, 59, 59 );

		public const string NewsBaseUrl = "news.google.com"; // for Reachability test
		public const string NewsUrl = "http://news.google.com/news?q=mobile%20world%20congress&output=rss";
		public const string TwitterUrl = "http://search.twitter.com/search.atom?q=%40mobileworldlive&show-user=true&rpp=20";
		
		public const string ConferenceDataBaseUrl = "confapp.com";
		public const string ConferenceDataUrl = "http://confapp.com/Data/mwc12/full2.xml";
	}
}