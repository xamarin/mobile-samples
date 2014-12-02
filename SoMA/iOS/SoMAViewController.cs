using System;
using System.Drawing;
using Foundation;
using UIKit;
using Xamarin.Media;

namespace SoMA
{
	public partial class SoMAViewController : UIViewController
	{
		public SoMAViewController (IntPtr handle) : base (handle)
		{
		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

		partial void PhotoButton_TouchUpInside (UIButton sender)
		{
			var picker = new MediaPicker ();
			//           new MediaPicker (this); on Android
			if (!picker.IsCameraAvailable)
				Console.WriteLine ("No camera!");
			else {
				picker.TakePhotoAsync (new StoreCameraMediaOptions {
					Name = "test.jpg",
					Directory = "MediaPickerSample"
				}).ContinueWith (t => {
					if (t.IsCanceled) {
						Console.WriteLine ("User canceled");
						return;
					}
					Console.WriteLine (t.Result.Path);
				});
			}
		}

		#region View lifecycle
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			// Perform any additional setup after loading the view, typically from a nib.
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
		}

		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);
		}

		public override void ViewDidDisappear (bool animated)
		{
			base.ViewDidDisappear (animated);
		}
		#endregion

		[Obsolete]
		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			// Return true for supported orientations
			return (toInterfaceOrientation != UIInterfaceOrientation.PortraitUpsideDown);
		}
	}
}