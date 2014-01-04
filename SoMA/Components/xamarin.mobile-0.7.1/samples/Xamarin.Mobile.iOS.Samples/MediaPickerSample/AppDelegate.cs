using System;
using System.Threading.Tasks;
using MonoTouch.Dialog;
using MonoTouch.Foundation;
using MonoTouch.MediaPlayer;
using MonoTouch.UIKit;
using Xamarin.Media;

namespace MediaPickerSample
{
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		readonly MediaPicker mediaPicker = new MediaPicker();
		readonly TaskScheduler uiScheduler = TaskScheduler.FromCurrentSynchronizationContext();

		private MediaPickerController mediaPickerController;
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			pickPhoto = new StringElement ("Pick Photo");
			pickPhoto.Tapped += () => {
				mediaPickerController = mediaPicker.GetPickPhotoUI();
				dialogController.PresentViewController (mediaPickerController, true, null);

				mediaPickerController.GetResultAsync().ContinueWith (t => {
					// We need to dismiss the controller ourselves
					dialogController.DismissViewController (true, () => {
						// User canceled or something went wrong
						if (t.IsCanceled || t.IsFaulted)
							return;

						// We get back a MediaFile
						MediaFile media = t.Result;
						ShowPhoto (media);
					});
				}, uiScheduler); // Make sure we use the UI thread to show our photo.
			};

			takePhoto = new StringElement ("Take Photo");
			takePhoto.Tapped += () => {
				// Make sure we actually have a camera
				if (!mediaPicker.IsCameraAvailable) {
					ShowUnsupported();
					return;
				}

				// When capturing new media, we can specify it's name and location
				mediaPickerController = mediaPicker.GetTakePhotoUI (new StoreCameraMediaOptions {
					Name = "test.jpg",
					Directory = "MediaPickerSample"
				});

				dialogController.PresentViewController (mediaPickerController, true, null);

				mediaPickerController.GetResultAsync().ContinueWith (t => {
					// We need to dismiss the controller ourselves
					dialogController.DismissViewController (true, () => {
						// User canceled or something went wrong
						if (t.IsCanceled || t.IsFaulted)
							return;

						// We get back a MediaFile
						MediaFile media = t.Result;
						ShowPhoto (media);
					});
				}, uiScheduler); // Make sure we use the UI thread to show our photo.
			};

			takeVideo = new StringElement ("Take Video");
			takeVideo.Tapped += () => {
				// Make sure video is supported and a camera is available
				if (!mediaPicker.VideosSupported || !mediaPicker.IsCameraAvailable) {
					ShowUnsupported();
					return;
				}

				// When capturing video, we can hint at the desired quality and length.
				// DesiredLength is only a hint, however, and the resulting video may
				// be longer than desired.
				mediaPickerController = mediaPicker.GetTakeVideoUI (new StoreVideoOptions {
					Quality = VideoQuality.Medium,
					DesiredLength = TimeSpan.FromSeconds (10),
					Directory = "MediaPickerSample",
					Name = "test.mp4"
				});

				dialogController.PresentViewController (mediaPickerController, true, null);

				mediaPickerController.GetResultAsync().ContinueWith (t => {
					// We need to dismiss the controller ourselves
					dialogController.DismissViewController (true, () => {
						// User canceled or something went wrong
						if (t.IsCanceled || t.IsFaulted)
							return;

						// We get back a MediaFile
						MediaFile media = t.Result;
						ShowVideo (media);
					});
				}, uiScheduler); // Make sure we use the UI thread to show our video.
			};

			pickVideo = new StringElement ("Pick Video");
			pickVideo.Tapped += () => {
				if (!mediaPicker.VideosSupported) {
					ShowUnsupported();
					return;
				}

				mediaPickerController = mediaPicker.GetPickVideoUI();
				dialogController.PresentViewController (mediaPickerController, true, null);

				mediaPickerController.GetResultAsync().ContinueWith (t => {
					// We need to dismiss the controller ourselves
					dialogController.DismissViewController (true, () => {
						// User canceled or something went wrong
						if (t.IsCanceled || t.IsFaulted)
							return;

						// We get back a MediaFile
						MediaFile media = t.Result;
						ShowVideo (media);
					});
				}, uiScheduler); // Make sure we use the UI thread to show our video.
			};

			var root = new RootElement("Xamarin.Media Sample") {
				new Section ("Picking media") { pickPhoto, pickVideo },
				new Section ("Capturing media") { takePhoto, takeVideo }
			};

			dialogController = new DisposingMediaViewController (root);
			viewController = new UINavigationController (dialogController);

			window = new UIWindow (UIScreen.MainScreen.Bounds);
			window.RootViewController = viewController;
			window.MakeKeyAndVisible ();

			return true;
		}

		private void ShowVideo (MediaFile media)
		{
			dialogController.Media = media;

			moviePlayerView = new MPMoviePlayerViewController (NSUrl.FromFilename (media.Path));
			viewController.PresentMoviePlayerViewController (moviePlayerView);
		}

		private void ShowPhoto (MediaFile media)
		{
			dialogController.Media = media;

			image = new UIImageView (dialogController.View.Bounds);
			image.ContentMode = UIViewContentMode.ScaleAspectFit;
			image.Image = UIImage.FromFile (media.Path);

			mediaController = new UIViewController();
			mediaController.View.AddSubview (image);
			mediaController.NavigationItem.LeftBarButtonItem = 
				new UIBarButtonItem (UIBarButtonSystemItem.Done, (s, e) => viewController.PopViewControllerAnimated (true));

			viewController.PushViewController (mediaController, true);
		}

		private class DisposingMediaViewController : DialogViewController
		{
			public DisposingMediaViewController (RootElement root)
				: base (root)
			{
			}

			public MediaFile Media
			{
				get;
				set;
			}

			public override void ViewDidAppear (bool animated)
			{
				// When we're done viewing the media, we should clean it up
				if (Media != null) {
					Media.Dispose();
					Media = null;
				}

				base.ViewDidAppear (animated);
			}
		}

		private UIAlertView errorAlert;
		private void ShowUnsupported()
		{
			if (this.errorAlert != null)
				this.errorAlert.Dispose();

			this.errorAlert = new UIAlertView ("Device unsupported", "Your device does not support this feature",
			                                   new UIAlertViewDelegate(), "OK");
			this.errorAlert.Show();
		}

		MPMoviePlayerController moviePlayer;
		MPMoviePlayerViewController moviePlayerView;
		UIViewController mediaController;
		UIImageView image;

		UIWindow window;
		UINavigationController viewController;
		DisposingMediaViewController dialogController;

		StringElement pickPhoto, pickVideo, takePhoto, takeVideo;
	}
}

