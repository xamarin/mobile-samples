using System;

namespace Core
{
	/// <summary>
	/// Keep all the social service credentials in one place...
	/// </summary>
	public static class ServiceConstants
	{
		// Set up a Twitter developer account at
		// https://dev.twitter.com/apps
		public static string TwitterConsumerKey = "CONSUMER_KEY";
		public static string TwitterConsumerSecret = "CONSUMER_SECRET";
		public static string TwitterCallbackUrl = "CALLBACK_URL";


		// Create a Facebook app and get a ClientId at  
		// https://developers.facebook.com/apps
		public static string FacebookClientId = "FACEBOOK_CLIENT_ID";
		public static string FacebookRedirectUrl = "http://www.facebook.com/connect/login_success.html";

	
		// Obtain Flickr credentials from
		// http://www.flickr.com/services/apps/by/me
		public static string FlickrConsumerKey = "CONSUMER_KEY";
		public static string FlickrConsumerSecret = "SECRET";


		// Learn how to develop for App.net on
		// http://developers.app.net/
		public static string AppDotNetClientId = "APPNET_CLIENT_ID";
	}
}