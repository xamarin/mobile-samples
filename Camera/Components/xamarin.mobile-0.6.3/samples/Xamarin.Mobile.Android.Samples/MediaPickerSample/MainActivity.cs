using System;
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
				// The MediaPicker is the class used to invoke the
				// camera and gallery picker for selecting and
				// taking photos and videos
				var picker = new MediaPicker (this);

				// We can check to make sure the device has a camera
				// and supports dealing with video.
				if (!picker.IsCameraAvailable || !picker.VideosSupported) {
					ShowUnsupported();
					return;
				}
				
				// TakeVideoAsync is an async API that takes a 
				// StoreVideoOptions object with various 
				// properties, such as the name and folder to
				// store the resulting video. You can
				// also limit the length of the video
				picker.TakeVideoAsync (new StoreVideoOptions {
					Name = "MyVideo",
					Directory = "MyVideos",
					DesiredLength = TimeSpan.FromSeconds (10)
				})
				.ContinueWith (t => {
					if (t.IsCanceled)
						return;

					RunOnUiThread (() => ShowVideo (t.Result.Path));
				});
			};
			
			Button photoButton = FindViewById<Button> (Resource.Id.takePhotoButton);
			photoButton.Click += delegate
			{
				var picker = new MediaPicker (this);

				if (!picker.IsCameraAvailable || !picker.PhotosSupported) {
					ShowUnsupported();
					return;
				}

				picker.TakePhotoAsync (new StoreCameraMediaOptions {
					Name = "test.jpg",
					Directory = "MediaPickerSample"
				})
				.ContinueWith (t => {
					if (t.IsCanceled)
						return;

					RunOnUiThread (() => ShowImage (t.Result.Path));
				});
			};
			
			Button pickVideoButton = FindViewById<Button> (Resource.Id.pickVideoButton);
			pickVideoButton.Click += delegate
			{
				// The MediaPicker is the class used to  invoke the camera
				// and gallery picker for selecting and taking photos
				// and videos
				var picker = new MediaPicker (this);
				
				if (!picker.VideosSupported) {
					ShowUnsupported();
					return;
				}

				// PickVideoAsync is an async API that invokes
				// the native gallery
				picker.PickVideoAsync ().ContinueWith (t => {
					if (t.IsCanceled)
						return;

					RunOnUiThread (() => ShowVideo (t.Result.Path));
				});
			};

			Button pickPhotoButton = FindViewById<Button> (Resource.Id.pickPhotoButton);
			pickPhotoButton.Click += delegate {
				var picker = new MediaPicker (this);

				if (!picker.PhotosSupported) {
					ShowUnsupported();
					return;
				}

				picker.PickPhotoAsync ().ContinueWith (t => {
					if (t.IsCanceled)
						return;

					RunOnUiThread (() => ShowImage (t.Result.Path));
				});
			};
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