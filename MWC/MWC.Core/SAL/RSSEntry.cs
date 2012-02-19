using System;
using MWC.BL.Contracts;

namespace MWC.BL {
	/// <summary>
	/// http://softwareandservice.wordpress.com/2009/09/21/building-a-rss-reader-iphone-app-using-monotouch/
	/// </summary>
	public class RSSEntry : BusinessEntityBase {

		public RSSEntry ()
		{
		}
		
		public string Title {
			get;set;
		}
		
		public string Content {
			get;set;
		}
		
		public DateTime Published {
			get;set;
		}
		
	    public string Url {
			get;set;
		}
	}
}

