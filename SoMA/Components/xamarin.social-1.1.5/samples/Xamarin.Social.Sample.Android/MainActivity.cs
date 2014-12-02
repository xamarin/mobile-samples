using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using Xamarin.Media;
using Xamarin.Social.Services;

namespace Xamarin.Social.Sample.Android
{
	[Activity (Label = "Xamarin.Social Sample", MainLauncher = true)]
	public class MainActivity : Activity
	{

		#region Fields

		private static FacebookService mFacebook;
		private static FlickrService mFlickr;
		private static TwitterService mTwitter;
		#endregion

		public static FacebookService Facebook
		{
			get
			{
				if (mFacebook == null)
				{
					mFacebook = new FacebookService() {
						ClientId = "App ID/API Key from https://developers.facebook.com/apps",
						RedirectUrl = new Uri ("Redirect URL from https://developers.facebook.com/apps")
					};
				}

				return mFacebook;
			}
		}

		public static FlickrService Flickr
		{
			get
			{
				if (mFlickr == null)
				{
					mFlickr = new FlickrService() {
						ConsumerKey = "Key from http://www.flickr.com/services/apps/by/me",
						ConsumerSecret = "Secret from http://www.flickr.com/services/apps/by/me",
					};
				}

				return mFlickr;
			}
		}

		public static TwitterService Twitter
		{
			get
			{
				if (mTwitter == null)
				{
					mTwitter = new TwitterService {
						ConsumerKey = "Consumer key from https://dev.twitter.com/apps",
						ConsumerSecret = "Consumer secret from https://dev.twitter.com/apps",
						CallbackUrl = new Uri ("Callback URL from https://dev.twitter.com/apps")
					};
				}

				return mTwitter;
			}
		}
			
		void Share (Service service, Button shareButton)
		{
			Item item = new Item {
				Text = "I'm sharing great things using Xamarin!",
				Links = new List<Uri> {
					new Uri ("http://xamarin.com"),
				},
			};

			Intent intent = service.GetShareUI (this, item, shareResult => {
				shareButton.Text = service.Title + " shared: " + shareResult;
			});

			StartActivity (intent);
		}

		private void ShowMessage(String message)
		{
			Toast.MakeText(this, message, ToastLength.Long).Show();

		}
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.Main);

			Button flickrButton = FindViewById<Button> (Resource.Id.Flickr);
			flickrButton.Click += (sender, args) =>
			{

				var picker = new MediaPicker (this);

				var intentPicker = picker.GetPickPhotoUI ();

				StartActivityForResult (intentPicker, 1);
			};

			Button facebookButton = FindViewById<Button>(Resource.Id.Facebook);
			facebookButton.Click += (sender, args) =>
			{
				try
				{
					Share (Facebook, facebookButton); 
				}
				catch (Exception ex)
				{
					ShowMessage("Facebook: " + ex.Message);
				}
			};
				
			Button twitterButton = FindViewById<Button> (Resource.Id.Twitter);
			twitterButton.Click += (sender, args) => 
			{
				try
				{
					Share (Twitter, twitterButton);
				}
				catch (Exception ex)
				{
					ShowMessage("Twitter: " + ex.Message);
				}
			};
				
		}

		protected async override void OnActivityResult (int requestCode, Result resultCode, Intent data)
		{
			if (requestCode != 1)
				return;

			if (resultCode == Result.Canceled)
				return;

			var file = await data.GetMediaFileExtraAsync (this);

			try
			{
				using (var stream = file.GetStream ()) {
					var item = new Item ("I'm sharing great things using Xamarin!") {
						Images = new[] { new ImageData (file.Path) }
					};

					Intent intent = Flickr.GetShareUI (this, item, shareResult => {
						FindViewById<Button> (Resource.Id.Flickr).Text = "Flickr shared: " + shareResult;
					});

					StartActivity (intent);
				}
			}
			catch (Exception ex)
			{
				ShowMessage("Facebook: " + ex.Message);
			}


		}
	}
}