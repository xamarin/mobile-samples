using System;
using UIKit;
using System.Drawing;
using CoreGraphics;

namespace Example_StandardControls.Screens.iPhone.ScrollView
{
	public class Controller : UIViewController
	{
		UIScrollView scrollView;
		UIImageView imageView;
		#region -= constructors =-
		public Controller () : base()
		{
		}
		#endregion
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			// set the background color of the view to white
			View.BackgroundColor = UIColor.White;
			
			Title = "Scroll View";

			// create our scroll view
			scrollView = new UIScrollView (
				new CGRect (0, 0, View.Frame.Width, View.Frame.Height - NavigationController.NavigationBar.Frame.Height));
			View.AddSubview (scrollView);
			
			// create our image view
			imageView = new UIImageView (UIImage.FromFile ("Images/halloween.jpg"));
			scrollView.ContentSize = imageView.Image.Size;
			scrollView.MaximumZoomScale = 3f;
			scrollView.MinimumZoomScale = .1f;
			scrollView.AddSubview (imageView);
			
			scrollView.ViewForZoomingInScrollView += (UIScrollView sv) => {
				return imageView; };
		}
	}
}
