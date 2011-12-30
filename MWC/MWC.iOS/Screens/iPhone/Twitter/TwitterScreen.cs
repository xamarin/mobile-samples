using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.Dialog;
using MWC.BL;
using MWC.SAL;

namespace MWC.iOS.Screens.iPhone.Twitter
{
	public partial class TwitterScreen : DialogViewController
	{
		protected TweetDetailsScreen _twitterDetailsScreen;
		TwitterParser<Tweet> twitterParser;
		public List<Tweet> TwitterFeed;

		public TwitterScreen () : base (UITableViewStyle.Plain, null)
		{
			Console.WriteLine("not updating, populating twitter.");
			this.PopulatePage();
			RefreshRequested += HandleRefreshRequested;
		}

		void HandleRefreshRequested (object sender, EventArgs e)
		{
			var dvc = (TwitterScreen)sender;
			Section section;
			UI.CustomElements.TweetElement twitterElement;
			//TODO: implement pull-to-refresh!!
			twitterParser.Refresh(delegate {
				using (var pool = new NSAutoreleasePool())
				{
					MonoTouch.UIKit.UIApplication.SharedApplication.NetworkActivityIndicatorVisible = false;
					var tweets = twitterParser.AllItems;	
					// create a root element and a new section (MT.D requires at least one)
					this.Root = new RootElement ("Twitter");
					section = new Section();
		
					// for each exhibitor, add a custom ExhibitorElement to the elements collection
					foreach ( var tw in tweets )
					{
						var currentTweet = tw; //cloj
						twitterElement = new UI.CustomElements.TweetElement (currentTweet);
						section.Add(twitterElement);
					}
					
					// add the section to the root
					Root.Add(section);

					dvc.ReloadComplete ();
				}
			});

		}
		
		/// <summary>
		/// Populates the page with tweets
		/// </summary>
		public void PopulatePage()
		{
			// declare vars
			Section section;
			UI.CustomElements.TweetElement twitterElement;

			// get the tweets
			TwitterFeed = new List<Tweet>();
			twitterParser = new TwitterParser<Tweet>(AppDelegate.TwitterUrl);

			twitterParser.Refresh(delegate {
				using (var pool = new NSAutoreleasePool())
				{
					MonoTouch.UIKit.UIApplication.SharedApplication.NetworkActivityIndicatorVisible = false;
					var tweets = twitterParser.AllItems;	
					// create a root element and a new section (MT.D requires at least one)
					this.Root = new RootElement ("Twitter");
					section = new Section();
		
					// for each exhibitor, add a custom ExhibitorElement to the elements collection
					foreach ( var tw in tweets )
					{
						var currentTweet = tw; //cloj
						twitterElement = new UI.CustomElements.TweetElement (currentTweet);
						section.Add(twitterElement);
					}
					
					// add the section to the root
					Root.Add(section);
				}
			});


			// create a root element and a new section (MT.D requires at least one)
			this.Root = new RootElement ("Twitter");
			section = new Section();
		
			// add the section to the root
			Root.Add(section);
		}
	}
}