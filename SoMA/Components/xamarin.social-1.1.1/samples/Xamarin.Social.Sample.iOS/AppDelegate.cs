using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
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

		private static readonly FacebookService Facebook = new FacebookService {
			ClientId = "App ID/API Key from https://developers.facebook.com/apps",
			RedirectUrl = new Uri ("Redirect URL from https://developers.facebook.com/apps")
		};

		private static readonly FlickrService Flickr = new FlickrService {
			ConsumerKey = "Key from http://www.flickr.com/services/apps/by/me",
			ConsumerSecret = "Secret from http://www.flickr.com/services/apps/by/me",
		};

		private static readonly TwitterService Twitter = new TwitterService {
			ConsumerKey = "Consumer key from https://dev.twitter.com/apps",
			ConsumerSecret = "Consumer secret from https://dev.twitter.com/apps",
			CallbackUrl = new Uri ("Callback URL from https://dev.twitter.com/apps")
		};

		private static readonly Twitter5Service Twitter5 = new Twitter5Service();

		void Share (Service service, StringElement button)
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

		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			var root = new RootElement ("Xamarin.Social Sample");

			var section = new Section ("Services");

			var facebookButton = new StringElement ("Share with Facebook");
			facebookButton.Tapped += delegate { Share (Facebook, facebookButton); };
			section.Add (facebookButton);

			var twitterButton = new StringElement ("Share with Twitter");
			twitterButton.Tapped += delegate { Share (Twitter, twitterButton); };
			section.Add (twitterButton);

			var twitter5Button = new StringElement ("Share with built-in Twitter");
			twitter5Button.Tapped += delegate { Share (Twitter5, twitter5Button); };
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

