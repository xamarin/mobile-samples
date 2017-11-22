using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Core;
using Foundation;
using UIKit;

using Xamarin.Geolocation;
using Xamarin.Media;
using Xamarin.Social;
using Xamarin.Social.Services;

namespace SoMA {
	public partial class PhotoScreen : UIViewController {

		MediaPickerController pickerController;

		string fileName = string.Empty;
		string fileNameThumb = string.Empty;
		string location = string.Empty;

		ShareItem shareItem { get; set; }

		public void SetItem (ShareItem item)
		{
			fileName = item.ImagePath;
			location = item.Location;
		}

		bool isLocationSet {
			get {
				return location != string.Empty;
			}
		}

		public PhotoScreen (IntPtr handle) : base (handle)
		{
			Title = "Share";
		}

		public override async void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			if (string.IsNullOrEmpty (fileName)) {
				fileName = "in-progress";
				var picker = new MediaPicker ();
				if (!picker.IsCameraAvailable || !picker.PhotosSupported)
					Console.WriteLine ("No camera!");
				else {
					var options = new StoreCameraMediaOptions {
						Name = DateTime.Now.ToString ("yyyyMMddHHmmss"),
						Directory = "MediaPickerSample"
					};

					pickerController = picker.GetTakePhotoUI (options);
					PresentViewController (pickerController, true, null);

					MediaFile media;

					try {
						var pickerTask = pickerController.GetResultAsync ();
						await pickerTask;

						// User canceled or something went wrong
						if (pickerTask.IsCanceled || pickerTask.IsFaulted) {
							fileName = string.Empty;
							return;
						}

						media = pickerTask.Result;
						fileName = media.Path;

						// We need to dismiss the controller ourselves
						await DismissViewControllerAsync (true); // woot! async-ified iOS method
					}
					catch(AggregateException ae) {
						fileName = string.Empty;
						Console.WriteLine ("Error while huh: {0}", ae.Message);
					} catch(Exception e) {
						fileName = string.Empty;
						Console.WriteLine ("Error while cancelling: {0}", e.Message);
					}

					if (String.IsNullOrEmpty (fileName)) {
						await DismissViewControllerAsync (true); 
					} else {
						PhotoImageView.Image = new UIImage (fileName);
						SavePicture (fileName);
					}
				}
			} else if (fileName == "cancelled") {
				NavigationController.PopToRootViewController (true);
			} else {
				// populate screen with existing item
				PhotoImageView.Image = FileExists (fileName) ? new UIImage (fileName) : null;
				LocationText.Text = location;
			}

			var locator = new Geolocator { DesiredAccuracy = 50 };
			var position = await locator.GetPositionAsync ( new CancellationToken(), false);
			Console.WriteLine ("Position Latitude: {0}", position.Latitude);
			Console.WriteLine ("Position Longitude: {0}", position.Longitude);

			location = string.Format ("{0},{1}", position.Latitude, position.Longitude);

			LocationText.Text = location;
		}

		static bool FileExists (string fileName)
		{
			string path = Environment.GetFolderPath (Environment.SpecialFolder.Personal);
			string filePath = Path.Combine (path, fileName);
			return File.Exists (filePath);
		}

		/// <summary>
		/// Expects a path to the file, but also accesses fileNameThumb field declared in the class (be warned)
		/// </summary>
		void SavePicture (string file) 
		{
			fileNameThumb = file.Replace (".jpg", "_thumb.jpg");
			var thumbnail = PhotoImageView.Image.Scale(new SizeF(88, 88));
			NSData imgData = thumbnail.AsJPEG ();
			NSError err;

			if (imgData.Save(fileNameThumb, false, out err))
				Console.WriteLine ("saved as {0}", fileNameThumb);
			else
				Console.WriteLine ("NOT saved as {0} because {1}", fileNameThumb, err.LocalizedDescription);
		}
		/// <summary>
		/// Build-in twitter support on iOS doesn't require you to create a separate developer account
		/// </summary>
		partial void ShareTwitter_TouchUpInside (UIButton sender)
		{
			var twitter = new Twitter5Service ();
			Share (twitter);
		}

		/// <summary>
		/// Create an app and get a ClientId at  
		/// https://developers.facebook.com/apps
		/// </summary>
		partial void ShareFacebook_TouchUpInside (UIButton sender)
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
		/// 
		/// </summary>
		partial void ShareAppnet_TouchUpInside (UIButton sender)
		{
			var appnet = new AppDotNetService { 
				ClientId = ServiceConstants.AppDotNetClientId,
				RedirectUrl = new Uri (ServiceConstants.AppDotRedirectUrl)
			};
			Share (appnet);
		}

		/// <summary>
		/// Obtain ConsumerKey and ConsumerSecret from
		/// http://www.flickr.com/services/apps/by/me
		/// </summary>
		partial void ShareFlickr_TouchUpInside (UIButton sender)
		{
			var flickr = new FlickrService {
				ConsumerKey = ServiceConstants.FlickrConsumerKey,
				ConsumerSecret = ServiceConstants.FlickrConsumerSecret,
			};
			Share (flickr);
		}

		void Share (Service service)
		{
			// 2. Create an item to share
			var item = new Item { Text = "Xamarin.SoMA ... Social Mobile & Auth! " };

			if (fileName != "in-progress" && fileName != "cancelled" && fileName != string.Empty) // was never set, no image 
				item.Images.Add (new ImageData (fileName));
	
			if (isLocationSet)
				item.Links.Add (new Uri ( "https://maps.google.com/maps?q=" + location));

			// 3. Present the UI on iOS
			var shareController = service.GetShareUI (item, result => {
				DismissViewController (true, null);

				// result lets you know if the user shared the item or canceled
				if (result == ShareResult.Cancelled) return;

				Console.WriteLine (service.Title + " shared");

				// 4. Now save to the database for the MainScreen list
				var si = new ShareItem {
					Text = item.Text, // get the edited text from the share UI
					ImagePath = fileName,
					ThumbImagePath = fileNameThumb,
					Location = location
				};

				if (item.Links.Count > 0) si.Link = item.Links[0].AbsoluteUri;
					si.SocialType = service.Title;

				AppDelegate.Database.SaveItem (si);
				shareItem = si;

				// 5. Return to the MainScreen
				NavigationController.PopViewController (true);
			});

			PresentViewController (shareController, true, null);
		}
	}
}
