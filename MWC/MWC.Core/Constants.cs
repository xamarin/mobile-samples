using System;
using System.Collections.Generic;

namespace MWC {
	public class Constants {

		public Constants ()
		{
		}
		
		public static DateTime StartDateMin = new DateTime ( 2012, 02, 27, 0, 0, 0 );
		public static DateTime EndDateMax = new DateTime ( 2012, 03, 1, 23, 59, 59 );

		public const string NewsBaseUrl = "news.google.com"; // for Reachability test
		public const string NewsUrl = "http://news.google.com/news?q=mobile%20world%20congress&output=rss";
		public const string TwitterUrl = "http://search.twitter.com/search.atom?q=%40mobileworldlive&show-user=true&rpp=30&result-type=recent";
		
		public const string ConferenceDataBaseUrl = "mwc.xamarin.com";
		public const string ConferenceDataUrl = "http://mwc.xamarin.com/conference.xml";
		public const string ExhibitorDataUrl = "http://mwc.xamarin.com/exhibitors.xml";

		public const float MapPinLatitude = 41.374377f;
		public const float MapPinLongitude = 2.152226f;
		public const string MapPinTitle = "Mobile World Congress 2012";
		public const string MapPinSubtitle = "Fira de Barcelona";
		
		public const string AboutUrlLinkedIn = "http://www.linkedin.com/company/xamarin";
		public const string AboutUrlStackOverflow = "http://stackoverflow.com/questions/tagged/monotouch";
		public const string AboutUrlTwitter = "https://twitter.com/#!/xamarinhq";
		public const string AboutUrlYouTube = "http://www.youtube.com/xamarinhq";
		public const string AboutUrlFacebook = "http://www.facebook.com/pages/Xamarin/283148898401104";
        public const string AboutUrlBlogRss = "http://blog.xamarin.com/feed/";
        public const string AboutMailto = "hello@xamarin.com";
        public const string AboutTwitter = "@xamarinhq";

		public const string AboutText = @"Xamarin was founded in 2011 with the mission to make it fast, easy and fun to build great mobile apps. Xamarin’s products are used by individual developers and companies, including VMware, Target, Rdio, and Medtronic, to simplify creation and operation of high-performance, cross-platform mobile consumer and corporate applications, targeting phones and tablets running iOS, Android and Windows. For more information, visit http://xamarin.com or email hello@xamarin.com. To download the code for this app go to http://mwc.xamarin.com.";
	}
}