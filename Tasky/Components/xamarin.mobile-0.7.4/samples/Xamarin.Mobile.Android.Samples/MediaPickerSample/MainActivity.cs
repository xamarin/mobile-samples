using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Widget;
using Android.OS;
using Xamarin.Media;

namespace MediaPickerSample
{
	[Activity (Label = "MediaPickerSample", MainLauncher = true, Icon = "@drawable/icon", ConfigurationChanges = ConfigChanges.Orientation)]
	public class MainActivity : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.Main);
			
			Button videoButton = FindViewById<Button> (Resource.Id.takeVideoButton);
			videoButton.Click += delegate {
				// MediaPicker is the class used to invoke the
				// camera and gallery picker for selecting and
				// taking photos and videos
				var picker = new MediaPicker (this);

				// We can check to make sure the device has a camera
				// and supports dealing with video.
				if (!picker.IsCameraAvailable || !picker.VideosSupported) {
					ShowUnsupported();
					return;
				}
				
				// The GetTakeVideoUI method returns an Intent to start
				// the native camera app to record a video.
				Intent intent = picker.GetTakeVideoUI (new StoreVideoOptions {
					Name = "MyVideo",
					Directory = "MyVideos",
					DesiredLength = TimeSpan.FromSeconds (10)
				});

				StartActivityForResult (intent, 1);
			};
			
			Button photoButton = FindViewById<Button> (Resource.Id.takePhotoButton);
			photoButton.Click += delegate {
				var picker = new MediaPicker (this);

				if (!picker.IsCameraAvailable || !picker.PhotosSupported) {
					ShowUnsupported();
					return;
				}

				Intent intent = picker.GetTakePhotoUI (new StoreCameraMediaOptions {
					Name = "test.jpg",
					Directory = "MediaPickerSample"
				});

				StartActivityForResult (intent, 2);
			};
			
			Button pickVideoButton = FindViewById<Button> (Resource.Id.pickVideoButton);
			pickVideoButton.Click += delegate {
				var picker = new MediaPicker (this);
				
				if (!picker.VideosSupported) {
					ShowUnsupported();
					return;
				}

				// The GetPickVideoUI() method returns an Intent to start
				// the native gallery app to select a video.
				Intent intent = picker.GetPickVideoUI();
				StartActivityForResult (intent, 1);
			};

			Button pickPhotoButton = FindViewById<Button> (Resource.Id.pickPhotoButton);
			pickPhotoButton.Click += delegate {
				var picker = new MediaPicker (this);

				if (!picker.PhotosSupported) {
					ShowUnsupported();
					return;
				}

				Intent intent = picker.GetPickPhotoUI();
				StartActivityForResult (intent, 2);
			};
		}

		protected override void OnActivityResult (int requestCode, Result resultCode, Intent data)
		{
			// User canceled
			if (resultCode == Result.Canceled)
				return;

			data.GetMediaFileExtraAsync (this).ContinueWith (t => {
				if (requestCode == 1) { // Video request
					ShowVideo (t.Result.Path);
				} else if (requestCode == 2) { // Image request
					ShowImage (t.Result.Path);				
				}
			}, TaskScheduler.FromCurrentSynchronizationContext());
		}

		private void ShowVideo (string path)
		{
			Intent videoIntent = new Intent (this, typeof (VideoActivity));
			videoIntent.PutExtra ("path", path);
			StartActivity (videoIntent);
		}

		private void ShowImage (string path)
		{
			Intent imageIntent = new Intent (this, typeof (ImageActivity));
			imageIntent.PutExtra ("path", path);
			StartActivity (imageIntent);
		}

		private Toast unsupportedToast;
		private void ShowUnsupported()
		{
			if (this.unsupportedToast != null) {
				this.unsupportedToast.Cancel();
				this.unsupportedToast.Dispose();
			}

			this.unsupportedToast = Toast.MakeText (this, "Your device does not support this feature", ToastLength.Long);
			this.unsupportedToast.Show();
		}
	}
}