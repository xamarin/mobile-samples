using System;
using System.Linq;
using System.Xml.Linq; // requires System.Xml.Linq added to References
using System.Collections.Generic;
using System.Net;
using System.Diagnostics;
using MWC.BL;

namespace MWC.SAL
{	
	/// <summary>
	/// Load a twitter Atom feed
	/// </summary>
	/// <remarks>
	/// Inspired by the RSSRepository from 
	/// http://softwareandservice.wordpress.com/2009/09/21/building-a-rss-reader-iphone-app-using-monotouch/
	/// </remarks>
	public class TwitterParser<Tweet> : XmlFeedParserBase<MWC.BL.Tweet>
	{	
		//https://dev.twitter.com/docs/api/1/get/search
		const string fileName = "TwitterFeed.xml";
		//static string url = "http://search.twitter.com/search.atom?q=%40mobileworldlive&show-user=true&rpp=20";
		const string urlFormat = "http://search.twitter.com/search.atom?q={0}&show-user=true&rpp={1}";
		
		public TwitterParser(string url) : base(url, fileName)
		{
			Debug.WriteLine ("Created new Twitter Repository");
		}
		
		public TwitterParser(string keyWord, string amountOfTweets) : base(String.Format(urlFormat, keyWord, amountOfTweets), fileName)
		{
			Debug.WriteLine ("setting up twitter repository");
		}
		
		protected override List<MWC.BL.Tweet> ParseXml (string xml)
		{
			Debug.WriteLine ("Starting Parsing XML");

			XNamespace ns = "http://www.w3.org/2005/Atom";
			XDocument rssFeed = XDocument.Parse(xml);
			var items = 
				from item in rssFeed.Descendants(ns + "entry")
				select new MWC.BL.Tweet
				{
					Title  = item.Element(ns + "title").Value,
					Content = item.Element(ns + "content").Value,
					Author = item.Element(ns + @"author").Value,
					Published = DateTime.Parse (item.Element(ns + "published").Value),
					ImageUrl = item.Elements(ns + "link")
									.Where (x => x.Attribute("rel").Value == "image")
									.Select (x => x.Attribute ("href").Value).First ()		
				};
			Debug.WriteLine ("Finished Parsing Tweet XML");

			return items.ToList();
		}
	}
}