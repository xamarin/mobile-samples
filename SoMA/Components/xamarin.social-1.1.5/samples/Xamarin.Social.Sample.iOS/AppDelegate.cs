using System;
using System.Collections.Generic;
using System.Threading.Tasks;
#if ! __UNIFIED__
using MonoTouch.Foundation;
using MonoTouch.UIKit;
#else
using Foundation;
using UIKit;
#endif
using MonoTouch.Dialog;
using Xamarin.Media;
using Xamarin.Social.Services;

namespace Xamarin.Social.Sample.iOS
{
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		UIWindow window;
		DialogViewController dialog;

		#region Fields

		private static FacebookService mFacebook;
		private static FlickrService mFlickr;
		private static TwitterService mTwitter;
		private static Twitter5Service mTwitter5;
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

		public static Twitter5Service Twitter5
		{
			get
			{
				if (mTwitter5 == null)
				{
					mTwitter5 = new Twitter5Service();
				}

				return mTwitter5;
			}
		}
			
		private void Share (Service service, StringElement button)
		{
			Item item = new Item {
				Text = "I'm sharing great things using Xamarin!",
				Links = new List<Uri> {
					new Uri ("http://xamarin.com"),
				},
			};

			UIViewController vc = service.GetShareUI (item, shareResult => {
				dialog.DismissViewController (true, null);

				button.GetActiveCell().TextLabel.Text = service.Title + " shared: " + shareResult;
			});
			dialog.PresentViewController (vc, true, null);
		}

		private void ShowMessage(string Message)
		{
			var msgView = new UIAlertView("Error", Message,null,"OK", null);
			msgView.Show();
		}

		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			var root = new RootElement ("Xamarin.Social Sample");

			var section = new Section ("Services");

			var facebookButton = new StringElement ("Share with Facebook");
			facebookButton.Tapped += delegate 
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
			section.Add (facebookButton);

			var twitterButton = new StringElement ("Share with Twitter");
			twitterButton.Tapped += delegate 
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
			section.Add (twitterButton);

			var twitter5Button = new StringElement ("Share with built-in Twitter");
			twitter5Button.Tapped += delegate 
			{
				try
				{
					Share (Twitter5, twitter5Button); 
				}
				catch (Exception ex)
				{
					ShowMessage("Twitter5: " +ex.Message);
				}
			};
			section.Add (twitter5Button);

			var flickr = new StringElement ("Share with Flickr");
			flickr.Tapped += () => {
				var picker = new MediaPicker(); // Set breakpoint here
				picker.PickPhotoAsync().ContinueWith (t =>
				{
					if (t.IsCanceled)
						return;

					var item = new Item ("I'm sharing great things using Xamarin!") {
						Images = new[] { new ImageData (t.Result.Path) }
					};

					Console.WriteLine ("Picked image {0}", t.Result.Path);

					UIViewController viewController = Flickr.GetShareUI (item, shareResult =>
					{
						dialog.DismissViewController (true, null);
						flickr.GetActiveCell().TextLabel.Text = "Flickr shared: " + shareResult;
					});

					dialog.PresentViewController (viewController, true, null);
				}, TaskScheduler.FromCurrentSynchronizationContext());
			};
			section.Add (flickr);
			root.Add (section);

			dialog = new DialogViewController (root);

			window = new UIWindow (UIScreen.MainScreen.Bounds);
			window.RootViewController = new UINavigationController (dialog);
			window.MakeKeyAndVisible ();
			
			return true;
		}

		// This is the main entry point of the application.
		static void Main (string[] args)
		{
			UIApplication.Main (args, null, "AppDelegate");
		}
	}
}

