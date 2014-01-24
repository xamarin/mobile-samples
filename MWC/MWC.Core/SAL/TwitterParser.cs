using System;
using System.Linq;
using System.Xml.Linq;
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
	/// Load a twitter feed
	/// </summary>
	public class TwitterParser<Tweet> : FeedParserBase<MWC.BL.Tweet>
	{
		private List<MWC.BL.Tweet> tweets;
		private TwitterAuthorizer twitterAuthorizer;
		const string fileName = "TwitterFeed.json";
		const string urlFormat = "http://search.twitter.com/search.atom?q={0}&show-user=true&rpp={1}";

		public override List<MWC.BL.Tweet> AllItems {
			get {
				return tweets;
			}
		}

		public TwitterParser (string url) : base (url, fileName)
		{
			Initialize ();
			Debug.WriteLine ("Created new Twitter Repository");
		}

		public TwitterParser (string keyWord, string amountOfTweets) : base (String.Format (urlFormat, keyWord, amountOfTweets), fileName)
		{
			Initialize ();
			Debug.WriteLine ("setting up twitter repository");
		}

		public override void Refresh (Action action)
		{
			var webClient = new WebClient ();
			Debug.WriteLine ("Get remote json data");
			webClient.DownloadStringCompleted += (sender, e) => {
				try {
					SaveLocal (e.Result);
					tweets = Parse (e.Result);
				} catch (Exception ex) {
					Debug.WriteLine ("ERROR saving downloaded JSON: " + ex);
				}
				action ();
			};

			webClient.Headers.Set ("Authorization", string.Format ("{0} {1}", twitterAuthorizer.AuthenticationInfo.TokenType,
				twitterAuthorizer.AuthenticationInfo.AccessToken));
			webClient.Encoding = System.Text.Encoding.UTF8;
			webClient.DownloadStringAsync (new Uri (Constants.TwitterUrl));
		}

		protected override List<MWC.BL.Tweet> Parse (string data)
		{
			Debug.WriteLine ("Starting Parsing JSON");

			var tweets = new List<MWC.BL.Tweet> ();
			var statusesJson = JsonValue.Parse (data) ["statuses"];
			foreach (var jsonItem in statusesJson) {
				var tweet = jsonItem as JsonObject;
				tweets.Add (new MWC.BL.Tweet () {
					Title = tweet ["user"] ["name"],
					Content = tweet ["text"],
					RealName = tweet ["user"] ["name"],
					Author = tweet ["user"] ["screen_name"],
					Published = DateTime.ParseExact (tweet ["created_at"], "ddd MMM dd HH:mm:ss +ffff yyyy", new CultureInfo ("en-US")),
					ImageUrl = tweet ["user"] ["profile_image_url"]
				});
			}
			return tweets;
		}

		private void Initialize ()
		{
			twitterAuthorizer = new TwitterAuthorizer ();
			tweets = new List<MWC.BL.Tweet> ();
		}
	}
}