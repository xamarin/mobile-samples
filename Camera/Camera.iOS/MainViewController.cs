using System;
using System.Drawing;
using System.Threading.Tasks;

using Foundation;
using UIKit;

using Xamarin.Media;

namespace Camera.iOS
{
	public partial class MainViewController : UIViewController
	{
		public MainViewController () : base ("MainViewController", null)
		{
		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			var picker = new MediaPicker ();

			this.bntCamera.TouchUpInside += (sender, e) => {
				if (!picker.IsCameraAvailable)
					lblSuccess.Text = "No camera!";
				else {
					picker.TakePhotoAsync (new StoreCameraMediaOptions {
						Name = "test.jpg",
						Directory = "MediaPickerSample"
					}).ContinueWith (t => {
						if (t.IsCanceled) {
							lblSuccess.Text = "User cancelled";
							return;
						}
						lblSuccess.Text = "Photo succeeded";
						Console.WriteLine(t.Result.Path);
					}, TaskScheduler.FromCurrentSynchronizationContext());
				}
			};

		}
	}
}

