using System;
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

using Console = System.Console;

namespace Droid
{
	[Activity (Label = "Share", Theme = "@style/AppTheme", ConfigurationChanges = ConfigChanges.Orientation)]
	public class PhotoScreen : Activity
	{
		public static string ShareItemIdExtraName = "ShareItemId";
		const string FileNameKey = "FileName";

		Button facebookButton, flickrButton, twitterButton, appnetButton;
		ImageView photoImageView;
		TextView locationText;

		//save the bitmap between device rotations
		static Bitmap bitmap;

		string fileName = string.Empty;
		string fileNameThumb = string.Empty;
		string location = string.Empty;

		bool isLocationSet {
			get {
				return location != string.Empty;
			}
		}

		ShareItem shareItem { get; set; }

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

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

			//if reuse Bitmap if present
			fileName = savedInstanceState == null ? string.Empty : savedInstanceState.GetString (FileNameKey, string.Empty);
			bitmap = savedInstanceState == null ? null : (Bitmap) savedInstanceState.GetParcelable ("image");
		}


		protected override async void OnResume ()
		{
			base.OnResume ();

			int itemId = Intent.GetIntExtra(ShareItemIdExtraName, 0);
			if(itemId > 0) {
				shareItem = App.Database.GetItem(itemId);

				fileName = shareItem.ImagePath;
				Console.WriteLine ("Image path: {0}", fileName);
				Bitmap b = BitmapFactory.DecodeFile (fileName);
				// Display the bitmap
				photoImageView.SetImageBitmap (b);
				locationText.Text = shareItem.Location;
				return;
			}

			if (string.IsNullOrEmpty (fileName)) {
				fileName = "in-progress";
				var picker = new MediaPicker (this);
				if (!picker.IsCameraAvailable) {
					Console.WriteLine ("No camera!");
				} else {
					var options = new StoreCameraMediaOptions {
						Name = DateTime.Now.ToString ("yyyyMMddHHmmss"),
						Directory = "MediaPickerSample"
					};

					if (!picker.IsCameraAvailable || !picker.PhotosSupported) {
						ShowUnsupported();
						return;
					}

					Intent intent = picker.GetTakePhotoUI (options);

					StartActivityForResult (intent, 1);
				}
			} else {
				SetImage ();
			}

			try {
				var locator = new Geolocator (this) {
					DesiredAccuracy = 50
				};
				var position = await locator.GetPositionAsync (10000);
				Console.WriteLine ("Position Latitude: {0}", position.Latitude);
				Console.WriteLine ("Position Longitude: {0}", position.Longitude);

				location = string.Format ("{0},{1}", position.Latitude, position.Longitude);
				locationText.Text = location;
			} catch (Exception e) {
				Console.WriteLine ("Position Exception: {0}", e.Message);
			}
		}

		protected override void OnSaveInstanceState (Bundle outState)
		{
			base.OnSaveInstanceState (outState);
			//save the file path
			outState.PutString (FileNameKey, fileName); 
			outState.PutParcelable ("image", bitmap);
		}


		protected override async void OnActivityResult (int requestCode, Result resultCode, Intent data)
		{
			Console.WriteLine ("requestCode: {0}", requestCode);
			Console.WriteLine ("resultCode: {0}", resultCode);
			// User canceled
			if (resultCode == Result.Canceled)
				return;

			if (requestCode == 1) {
				var fileTask = data.GetMediaFileExtraAsync (this);
				await fileTask;

				fileName = fileTask.Result.Path;
				fileNameThumb = fileName.Replace(".jpg", "_thumb.jpg");
				Console.WriteLine("Image path: {0}", fileName);

				SetImage ();

				// Cleanup any resources held by the MediaFile instance
				fileTask.Result.Dispose();

				//savet the file
				var options = new BitmapFactory.Options {OutHeight = 128, OutWidth = 128};
				var newBitmap = await BitmapFactory.DecodeFileAsync (fileName, options);
				using (var @out = new System.IO.FileStream(fileNameThumb, System.IO.FileMode.Create)) {
					newBitmap.Compress (Bitmap.CompressFormat.Jpeg, 90, @out);
				}
				newBitmap.Dispose ();
			}
		}

		async void SetImage ()
		{
			if (bitmap == null) {
				//get the raw image dimensions
				var options = new BitmapFactory.Options ();
				options.InJustDecodeBounds = true;
				BitmapFactory.DecodeFile (fileName, options);
				//scale the image for the ImageView
				int height = options.OutHeight;
				int width = options.OutHeight;
				options.InSampleSize = CalculateSampleSize (options, height, width);
				options.InJustDecodeBounds = false;
				bitmap = await BitmapFactory.DecodeFileAsync (fileName, options);
				if (bitmap == null) { 
					//then there wasn't enough memory
					Console.WriteLine ("Ran out of memory");
					return;
				}
			} else
				Console.WriteLine ("Reusing image");
			photoImageView.SetImageBitmap (bitmap);
		}

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
				RedirectUrl = new Uri (ServiceConstants.FacebookRedirectUrl)
			};
			Share (facebook);
		}

		/// <summary>
		/// Obtain 
		/// http://www.flickr.com/services/apps/by/me
		/// </summary>
		void ShareAppnet_Click (object sender, EventArgs ea)
		{
			throw new NotImplementedException ("waiting for appnet to be implemented for Android");
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
			if (fileName == string.Empty || fileName == "in-progress")
				return;

			// 2. Create an item to share
			var text = "Xamarin.SoMA ... Social Mobile & Auth! ";
			if (shareItem != null) { // use the existing one passed to the activity
				text = shareItem.Text;
				fileName = shareItem.ImagePath;
				location = shareItem.Location;
			}
			var item = new Item { Text = text };
			item.Images.Add (new ImageData (fileName));
			if (isLocationSet)
				item.Links.Add (new Uri ( "https://maps.google.com/maps?q=" + location));

			// 3. Present the UI on Android
			var shareIntent = service.GetShareUI (this, item, result => {
				// result lets you know if the user shared the item or canceled
				if (result == ShareResult.Cancelled)
					return;

				Console.WriteLine ("{0} shared", service.Title);

				// 4. Now save to the database for the MainScreen list
				var si = new ShareItem {
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
		void ShowUnsupported ()
		{
			if (unsupportedToast != null) {
				unsupportedToast.Cancel ();
				unsupportedToast.Dispose ();
			}
			unsupportedToast = Toast.MakeText (this, "Your device does not support this feature", ToastLength.Long);
			unsupportedToast.Show ();
		}

		static int CalculateSampleSize(BitmapFactory.Options options, int rHeight, int rWidth)
		{
			// from http://developer.android.com/training/displaying-bitmaps/load-bitmap.html
			int height = options.OutHeight;
			int width = options.OutWidth;
			int inSampleSize = 1;

			if (height > rHeight || width > rWidth) {

				int halfHeight = height / 2;
				int halfWidth = width / 2;

				// Calculate the largest inSampleSize value that is a power of 2 and keeps both
				// height and width larger than the requested height and width.
				while ((halfHeight / inSampleSize) > rHeight
					&& (halfWidth / inSampleSize) > rWidth) {
					inSampleSize *= 2;
				}
			}
			return inSampleSize;
		}
	}
}

