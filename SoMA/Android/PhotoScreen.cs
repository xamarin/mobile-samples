using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using Xamarin.Media;
using Xamarin.Social;
using Xamarin.Social.Services;
using Xamarin.Geolocation;
using Android.Graphics;
using System.Threading.Tasks;
using Android.Content.PM;

using Core;
/*
 SoMA : Social Mobile Auth

This file includes both the 'deprecated' and 'new' Photo Picker API.

It will be updated shortly to *just* use the new API.

 */
using Java.IO;


namespace Droid
{
	[Activity (Label = "Share", Theme = "@style/AppTheme", ConfigurationChanges = ConfigChanges.Orientation)]
	public class PhotoScreen : Activity
	{
		public static string ShareItemIdExtraName = "ShareItemId";

		Button facebookButton, flickrButton, twitterButton, appnetButton;
		ImageView photoImageView;
		TextView locationText;

		string fileName = "", fileNameThumb = "";
		string location = "";

		bool isLocationSet {
			get {
				return !(location == "");
			}
		}

		Core.ShareItem shareItem { get; set; }

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.PhotoScreen);

			photoImageView = FindViewById<ImageView> (Resource.Id.photoImageView);
			locationText = FindViewById<TextView> (Resource.Id.locationText);
			facebookButton = FindViewById<Button> (Resource.Id.facebookButton);
			flickrButton = FindViewById<Button> (Resource.Id.flickrButton);
			twitterButton = FindViewById<Button> (Resource.Id.twitterButton);
			appnetButton = FindViewById<Button> (Resource.Id.appnetButton);

			facebookButton.Click += ShareFacebook_Click;
			flickrButton.Click += ShareFlickr_Click;
			twitterButton.Click += ShareTwitter_Click;
			appnetButton.Click += ShareAppnet_Click;
		}


		protected override async void OnResume ()
		{
			base.OnResume ();

			int itemId = Intent.GetIntExtra(ShareItemIdExtraName, 0);
			if(itemId > 0) {
				shareItem = App.Database.GetItem(itemId);

				fileName = shareItem.ImagePath;
				System.Console.WriteLine("Image path: " + fileName);
				Bitmap b = BitmapFactory.DecodeFile (fileName);
				// Display the bitmap
				photoImageView.SetImageBitmap (b);
				locationText.Text = shareItem.Location;
				return;
			}

			if (fileName == "") {
				fileName = "in-progress";
				var picker = new MediaPicker (this);
				//           new MediaPicker (); on iOS
				if (!picker.IsCameraAvailable)
					System.Console.WriteLine ("No camera!");
				else {
					var options = new StoreCameraMediaOptions {
						Name = DateTime.Now.ToString("yyyyMMddHHmmss"),
						Directory = "MediaPickerSample"
					};
#if !VISUALSTUDIO
					#region new style
					if (!picker.IsCameraAvailable || !picker.PhotosSupported) {
						ShowUnsupported();
						return;
					}

					Intent intent = picker.GetTakePhotoUI (options);

					StartActivityForResult (intent, 1);
					#endregion
#else 
					#region old style (deprecated)
					var t = picker.TakePhotoAsync (options); 
					await t;
					if (t.IsCanceled) {
						System.Console.WriteLine ("User canceled");
						fileName = "cancelled";
						// TODO: return to main screen
						StartActivity(typeof(MainScreen));
						return;
					}
					System.Console.WriteLine (t.Result.Path);
					fileName = t.Result.Path;
					fileNameThumb = fileName.Replace(".jpg", "_thumb.jpg"); 

					Bitmap b = BitmapFactory.DecodeFile (fileName);
					RunOnUiThread (() =>
					               {
						// Display the bitmap
						photoImageView.SetImageBitmap (b);

						// Cleanup any resources held by the MediaFile instance
						t.Result.Dispose();
					});
					var boptions = new BitmapFactory.Options {OutHeight = 128, OutWidth = 128};
					var newBitmap = await BitmapFactory.DecodeFileAsync (fileName, boptions);
					var @out = new System.IO.FileStream(fileNameThumb, System.IO.FileMode.Create);
					newBitmap.Compress(Android.Graphics.Bitmap.CompressFormat.Jpeg, 90, @out);
					//});
					#endregion
#endif
				}
			} 

			try {
				var locator = new Geolocator (this) { DesiredAccuracy = 50 };
				//            new Geolocator () { ... }; on iOS
				var position = await locator.GetPositionAsync (timeout: 10000);
				System.Console.WriteLine ("Position Latitude: {0}", position.Latitude);
				System.Console.WriteLine ("Position Longitude: {0}", position.Longitude);

				location = string.Format("{0},{1}", position.Latitude, position.Longitude);
				locationText.Text = location;
			} catch (Exception e) {
				System.Console.WriteLine ("Position Exception: " + e.Message);
			}
		}


#if !VISUALSTUDIO
		protected override async void OnActivityResult (int requestCode, Result resultCode, Intent data)
		{
			System.Console.WriteLine("requestCode: " + requestCode);
			System.Console.WriteLine("resultCode: " + resultCode);
			// User canceled
			if (resultCode == Result.Canceled)
				return;

			if (requestCode == 1) {
				var fileTask = data.GetMediaFileExtraAsync (this);
				await fileTask;

				fileName = fileTask.Result.Path;
				fileNameThumb = fileName.Replace(".jpg", "_thumb.jpg");
				System.Console.WriteLine("Image path: " + fileName);
				Bitmap b = BitmapFactory.DecodeFile (fileName);

				// Display the bitmap
				photoImageView.SetImageBitmap (b);
				// Cleanup any resources held by the MediaFile instance
				fileTask.Result.Dispose();

				b.Dispose();

				var options = new BitmapFactory.Options {OutHeight = 128, OutWidth = 128};
				var newBitmap = await BitmapFactory.DecodeFileAsync (fileName, options);
				using (var @out = new System.IO.FileStream(fileNameThumb, System.IO.FileMode.Create)) {
					newBitmap.Compress(Android.Graphics.Bitmap.CompressFormat.Jpeg, 90, @out);
				}
				newBitmap.Dispose();
			}
		}
#endif


		/// <summary>
		/// Set up an account at
		/// https://dev.twitter.com/apps
		/// </summary>
		void ShareTwitter_Click (object sender, EventArgs ea)
		{
			var twitter = new TwitterService {
				ConsumerKey = ServiceConstants.TwitterConsumerKey,
				ConsumerSecret = ServiceConstants.TwitterConsumerSecret,
				CallbackUrl = new Uri (ServiceConstants.TwitterCallbackUrl)
			};
			Share(twitter);
		}

		/// <summary>
		/// Create an app and get a ClientId at  
		/// https://developers.facebook.com/apps
		/// </summary>
		void ShareFacebook_Click (object sender, EventArgs ea)
		{
			// 1. Create the service
			var facebook = new FacebookService {
				ClientId = ServiceConstants.FacebookClientId,
				RedirectUrl = new System.Uri (ServiceConstants.FacebookRedirectUrl)
			};
			Share(facebook);
		}

		/// <summary>
		/// Obtain 
		/// http://www.flickr.com/services/apps/by/me
		/// </summary>
		void ShareAppnet_Click (object sender, EventArgs ea)
		{
			throw new NotImplementedException ("waiting for appnet to be implemented for Android");
			//			var appnet = new AppDotNetService { 
			//				ClientId = ServiceConstants.AppDotNetClientId
			//			};
			//			Share(appnet);
		}

		/// <summary>
		/// Obtain ConsumerKey and ConsumerSecret from
		/// http://www.flickr.com/services/apps/by/me
		/// </summary>
		void ShareFlickr_Click (object sender, EventArgs ea)
		{
			var flickr = new FlickrService {
				ConsumerKey = ServiceConstants.FlickrConsumerKey,
				ConsumerSecret = ServiceConstants.FlickrConsumerSecret,
			};
			Share(flickr);
		}

		void Share (Xamarin.Social.Service service)
		{
			if (fileName == "" || fileName == "in-progress")
				return;

			// 2. Create an item to share
			var text = "Xamarin.SoMA ... Social Mobile & Auth! ";
			if (shareItem != null) { // use the existing one passed to the activity
				text = shareItem.Text;
				fileName = shareItem.ImagePath;
				location = shareItem.Location;
			}
			var item = new Item { Text = text };
			item.Images.Add(new ImageData(fileName));
			if (isLocationSet) item.Links.Add(new Uri( "https://maps.google.com/maps?q=" + location));

			// 3. Present the UI on Android
			var shareIntent = service.GetShareUI (this, item, result => {
				// result lets you know if the user shared the item or canceled
				if (result == ShareResult.Cancelled) return;

				System.Console.WriteLine(service.Title + " shared");

				// 4. Now save to the database for the MainScreen list
				var si = new Core.ShareItem() {
					Text = item.Text, // get the edited text from the share UI
					ImagePath = fileName,
					Location = location
				};
				if (item.Links.Count > 0) si.Link = item.Links[0].AbsoluteUri;
				si.SocialType = service.Title;

				App.Database.SaveItem(si);
				shareItem = si; // replace the one in the activity
			});
			StartActivity (shareIntent);
		}



		/// <summary>shortcut back to the main screen</summary>
		public override bool OnCreateOptionsMenu (IMenu menu)
		{
			MenuInflater.Inflate (Resource.Menu.Share, menu);
			return true;
		}
		/// <summary>shortcut back to the main screen</summary>
		public override bool OnOptionsItemSelected (IMenuItem item)
		{
			if (item.ItemId == Resource.Id.gohome)
				StartActivity(typeof(MainScreen));
			return true;
		}




		Toast unsupportedToast;
		/// <summary>shortcut back to the main screen</summary>
		void ShowUnsupported()
		{
			if (unsupportedToast != null) {
				unsupportedToast.Cancel();
				unsupportedToast.Dispose();
			}
			unsupportedToast = Toast.MakeText (this, "Your device does not support this feature", ToastLength.Long);
			unsupportedToast.Show();
		}
	}
}

