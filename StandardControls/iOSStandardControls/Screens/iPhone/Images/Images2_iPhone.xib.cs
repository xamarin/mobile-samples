using System;
using Foundation;
using UIKit;
using CoreGraphics;

namespace Example_StandardControls.Screens.iPhone.Images
{
	public partial class Images2_iPhone : UIViewController
	{
		UIImageView imageView;
		UIImageView imgSpinningCircle;

		public Images2_iPhone (IntPtr handle)
			: base (handle)
		{
		}

		public Images2_iPhone ()
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			Title = "Images";

			// a simple image
			var img = UIImage.FromBundle ("Images/Icons/50_icon.png");
			imageView = new UIImageView (img) {
				Frame = new CGRect (20, 20, img.CGImage.Width, img.CGImage.Height)
			};
			View.AddSubview (imageView);

			// an animating image
			imgSpinningCircle = new UIImageView {
				Frame = new CGRect (150, 20, 100, 100),
				AnimationRepeatCount = 0,
				AnimationDuration = .5,
				AnimationImages = new UIImage[] {
					UIImage.FromBundle ("Images/Spinning Circle_1.png"),
					UIImage.FromBundle ("Images/Spinning Circle_2.png"),
					UIImage.FromBundle ("Images/Spinning Circle_3.png"),
					UIImage.FromBundle ("Images/Spinning Circle_4.png")
				}
			};

			View.AddSubview (imgSpinningCircle);
			imgSpinningCircle.StartAnimating ();
		}
	}
}