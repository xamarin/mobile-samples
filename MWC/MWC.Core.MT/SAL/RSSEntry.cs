using System;

namespace MWC.SAL
{
	/// <summary>
	/// http://softwareandservice.wordpress.com/2009/09/21/building-a-rss-reader-iphone-app-using-monotouch/
	/// </summary>
	public class RSSEntry
	{
		public RSSEntry ()
		{
		}
		
		public string Title
		{
			get;set;
		}
		
		public string Content
		{
			get;set;
		}
		
		public DateTime Published
		{
			get;set;
		}
		
		public int NumComments
		{
			get;set;
		}
		
	    public string Url
		{
			get;set;
		}
	}
}

