using System;
using System.Linq;
using System.Xml.Linq; // requires System.Xml.Linq added to References
using System.Collections.Generic;
using System.Net;
using System.Diagnostics;
using MWC.BL;
using System.IO;
using System.Json;
using System.Globalization;

namespace MWC.SAL
{	
	/// <summary>
	/// Load a twitter Atom feed
	/// </summary>
	/// <remarks>
	/// Inspired by the RSSRepository from 
	/// http://softwareandservice.wordpress.com/2009/09/21/building-a-rss-reader-iphone-app-using-monotouch/
	/// </remarks>
	public class TwitterParser<Tweet> : FeedParserBase<MWC.BL.Tweet>
	{	
		private TwitterAuthorizer twitterAuthorizer;
		//https://dev.twitter.com/docs/api/1/get/search
		const string fileName = "TwitterFeed.json";
		//static string url = "http://search.twitter.com/search.atom?q=%40mobileworldlive&show-user=true&rpp=20";
		const string urlFormat = "http://search.twitter.com/search.atom?q={0}&show-user=true&rpp={1}";
		
		public TwitterParser(string url) : base(url, fileName)
		{
			twitterAuthorizer = new TwitterAuthorizer ();
			Debug.WriteLine ("Created new Twitter Repository");
		}
		
		public TwitterParser(string keyWord, string amountOfTweets) : base(String.Format(urlFormat, keyWord, amountOfTweets), fileName)
		{
			twitterAuthorizer = new TwitterAuthorizer ();
			Debug.WriteLine ("setting up twitter repository");
		}

		public override void Refresh (Action action)
		{
			var webClient = new WebClient ();
			Debug.WriteLine ("Get remote json data");
			webClient.DownloadStringCompleted += (sender, e) =>
			{
				try {
					SaveLocal (e.Result);
					var items = Parse (e.Result);
				} catch (Exception ex) {
					Debug.WriteLine ("ERROR saving downloaded JSON: " + ex);
				}
				action();
			};

			webClient.Headers.Set ("Authorization", string.Format ("{0} {1}", twitterAuthorizer.AuthenticationInfo.TokenType,
				twitterAuthorizer.AuthenticationInfo.AccessToken));
			webClient.Encoding = System.Text.Encoding.UTF8;webClient.DownloadStringAsync (new Uri(Constants.TwitterUrl));
		}
		
		protected override List<MWC.BL.Tweet> Parse (string data)
		{
			Debug.WriteLine ("Starting Parsing JSON");

			var tweets = new List<MWC.BL.Tweet> ();
			var statusesJson = JsonValue.Parse (data)["statuses"];
			foreach(var jsonItem in statusesJson) {
				var tweet = jsonItem as JsonObject;
				tweets.Add (new MWC.BL.Tweet() {
					Title = tweet["user"]["name"],
					Content = tweet["text"],
					RealName = tweet["user"]["name"],
					Author = tweet["user"]["screen_name"],
					Published = DateTime.ParseExact(tweet["created_at"], "ddd MMM dd HH:mm:ss +ffff yyyy", new CultureInfo("en-US")),
					ImageUrl = tweet["user"]["profile_image_url"]
				});
			}
			return tweets;
		}
	}
}