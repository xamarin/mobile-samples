using System;
using UIKit;
using CoreGraphics;

namespace Example_StandardControls.Screens.iPhone.ScrollView
{
	public class Controller : UIViewController
	{
		UIScrollView scrollView;
		UIImageView imageView;

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			// set the background color of the view to white
			View.BackgroundColor = UIColor.White;

			Title = "Scroll View";

			imageView = new UIImageView (UIImage.FromFile ("Images/halloween.jpg"));

			// create our scroll view
			var frame = new CGRect (0, 0, View.Frame.Width, View.Frame.Height - NavigationController.NavigationBar.Frame.Height);
			scrollView = new UIScrollView (frame) {
				ContentSize = imageView.Image.Size,
				MaximumZoomScale = 3f,
				MinimumZoomScale = .1f
			};
			scrollView.AddSubview (imageView);
			View.AddSubview (scrollView);

			scrollView.ViewForZoomingInScrollView += (UIScrollView sv) => {
				return imageView;
			};
		}
	}
}
