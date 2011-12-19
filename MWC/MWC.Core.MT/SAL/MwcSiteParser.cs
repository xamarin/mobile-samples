using System;
using System.Collections.Generic;
using MWC.BL;

namespace MWC.SAL
{
	//TODO: get an elancer to fill this out.
	public static class MwcSiteParser
	{
		static MwcSiteParser ()
		{
		}
		
		public static IList<Exhibitor> GetExhibitors()
		{
			//stub
			return new List<Exhibitor> () { new Exhibitor() { Name = "Test", City = "Somwhere", Country = "USA", Locations = new List<string>() { "h1b", "e12" } } };			
		}
		
		public static IList<Session> GetSessions()
		{
			//stub
			return new List<Session> () { new Session() { Title = "Winning, with Charlie Sheen", Overview = "Lorem, ipsum, bowl of ...", Start = DateTime.Now, End = DateTime.Now, Speakers = new List<string>() { "Charlie Sheen", "Cap'n Haddock" } } };
		}
		
		public static IList<Speaker> GetSpeakers()
		{
			return new List<Speaker> () { new Speaker() { Name = "Bryan Costanich", Title = "Awesomest of All", Company = "Xamarin" } };
		}
	}
}

