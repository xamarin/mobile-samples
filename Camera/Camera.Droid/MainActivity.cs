using System;
using System.Threading.Tasks;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using Xamarin.Media;

namespace Camera.Droid
{
	[Activity (Label = "Camera.Droid", MainLauncher = true)]
	public class MainActivity : Activity
	{

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			// Get our button from the layout resource,
			// and attach an event to it
			Button button = FindViewById<Button> (Resource.Id.myButton);
			TextView label = FindViewById<TextView> (Resource.Id.lblSuccess);
			
			button.Click += delegate {
				var picker = new MediaPicker (this);
				if (!picker.IsCameraAvailable)
					 label.Text = "No camera!";
				else {
					picker.TakePhotoAsync (new StoreCameraMediaOptions {
						Name = "test.jpg",
						Directory = "MediaPickerSample"
					}).ContinueWith (t => {
						if (t.IsCanceled) {
							label.Text = "User canceled";
							return;
						}
						label.Text = "Photo succeeded";
						Console.WriteLine (t.Result.Path);
					}, TaskScheduler.FromCurrentSynchronizationContext());
				}
			};


		}
	}
}


