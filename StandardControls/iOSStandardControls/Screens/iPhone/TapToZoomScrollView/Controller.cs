using System;
using UIKit;
using System.Drawing;
using CoreGraphics;
using Example_StandardControls.Controls;

namespace Example_StandardControls.Screens.iPhone.TapToZoomScrollView
{
	public class Controller : UIViewController
	{
		TapZoomScrollView scrollView;
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
			this.View.BackgroundColor = UIColor.White;
			
			// create our scroll view
			scrollView = new TapZoomScrollView (new CGRect (0, 0, this.View.Frame.Width, this.View.Frame.Height - this.NavigationController.NavigationBar.Frame.Height));
			this.View.AddSubview (scrollView);
			
			// create our image view
			imageView = new UIImageView (UIImage.FromFile ("Images/halloween.jpg"));
			scrollView.ContentSize = imageView.Image.Size;
			scrollView.MaximumZoomScale = 3f;
			scrollView.MinimumZoomScale = .1f;
			scrollView.AddSubview (imageView);
			
			// when the scroll view wants to zoom, it asks for the view to zoom, so 
			// in this case, we tell it that we want it to zoom the image view
			scrollView.ViewForZoomingInScrollView += delegate(UIScrollView sv) {
				return imageView;
			};
		}
	}
}
