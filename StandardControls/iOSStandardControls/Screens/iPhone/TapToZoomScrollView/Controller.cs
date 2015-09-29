using System;
using UIKit;
using CoreGraphics;
using Example_StandardControls.Controls;

namespace Example_StandardControls.Screens.iPhone.TapToZoomScrollView
{
	public class Controller : UIViewController
	{
		TapZoomScrollView scrollView;
		UIImageView imageView;

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			// set the background color of the view to white
			View.BackgroundColor = UIColor.White;

			imageView = new UIImageView (UIImage.FromFile ("Images/halloween.jpg"));

			var frame = new CGRect (0, 0, View.Frame.Width, View.Frame.Height - NavigationController.NavigationBar.Frame.Height);
			scrollView = new TapZoomScrollView (frame) {
				ContentSize = imageView.Image.Size,
				MaximumZoomScale = 3f,
				MinimumZoomScale = .1f,
			};
			scrollView.AddSubview (imageView);
			View.AddSubview (scrollView);

			// when the scroll view wants to zoom, it asks for the view to zoom, so
			// in this case, we tell it that we want it to zoom the image view
			scrollView.ViewForZoomingInScrollView += (UIScrollView sv) => {
				return imageView;
			};
		}
	}
}
