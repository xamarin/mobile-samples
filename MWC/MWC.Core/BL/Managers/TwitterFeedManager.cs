using System;
using System.Collections.Generic;
using MWC.SAL;

namespace MWC.BL.Managers {
	public static class TwitterFeedManager {
		static object locker = new object();
		
		public static event EventHandler UpdateStarted = delegate {};
		public static event EventHandler UpdateFinished = delegate {};
		
		public static bool IsUpdating
		{
			get { return _isUpdating; }
			set { _isUpdating = value; }
		}
		private static bool _isUpdating = false;
		
		static TwitterFeedManager ()
		{}

        public static Tweet GetTweet(int tweetID)
        {
            return DAL.DataManager.GetTweet(tweetID);
        }

		public static IList<Tweet> GetTweets()
		{
			return new List<Tweet> ( DAL.DataManager.GetTweets () );
		}

		public static void Update()
		{
			// make this a critical section to ensure that access is serial
			lock(locker)
			{
				UpdateStarted (null, EventArgs.Empty);

				TwitterParser<Tweet> _twitterParser = new TwitterParser<Tweet>(Constants.TwitterUrl);

				_isUpdating = true;
				_twitterParser.Refresh(delegate {
					var tweets = _twitterParser.AllItems;	
					
					DAL.DataManager.DeleteTweets ();
					DAL.DataManager.SaveTweets (tweets);

					UpdateFinished (null, EventArgs.Empty);
					_isUpdating = false;
				});
			}
		}
	}
}