using System;
using System.Linq;
using System.Xml.Linq; // requires System.Xml.Linq added to References
using System.Collections.Generic;
using System.Net;
using System.Diagnostics;

namespace MWC.SAL
{
	/// <summary>
	/// http://softwareandservice.wordpress.com/2009/09/21/building-a-rss-reader-iphone-app-using-monotouch/
	/// </summary>
	public class RSSParser<RSSEntry> : XmlFeedParserBase<MWC.BL.RSSEntry>
	{	
		const string FileName = "NewsFeed.xml";
		const string Url = "http://news.google.com/news?q=mobile%20world%20congress&output=rss";
			
		//public RSSRepository() : base(Url, FileName)
		//{
		//	Debug.WriteLine ("Created new RSS Repository");
		//}
		
		public RSSParser(string url) : base(url, FileName)
		{
			Debug.WriteLine ("Created new RSS Repository");
		}

		protected override List<MWC.BL.RSSEntry> ParseXml (string xml)
		{
			Debug.WriteLine ("Starting Parsing XML from " + Url);

			XDocument rssFeed = XDocument.Parse(xml);
			var items = 
				from item in rssFeed.Descendants("item")
				select new MWC.BL.RSSEntry
				{
					Title = item.Element("title").Value,
					Content = item.Element("description").Value,
					Published = DateTime.Parse(item.Element("pubDate").Value),
					Url = item.Element("link").Value
				};
			Debug.WriteLine ("Finished Parsing XML");

			return items.ToList();
		}
	}
}