namespace Example_Touch.Screens.iPhone.Home
{
	using System;
	using Example_Touch.Screens.iPhone.GestureRecognizers;
	using Example_Touch.Screens.iPhone.SimpleTouch;
	using Foundation;
	using UIKit;

	public partial class Home_iPhone : UIViewController
	{
		private CustomCheckmarkGestureRecognizer_iPhone customGestureScreen;
		private GestureRecognizers_iPhone gestureScreen;
		private Touches_iPhone touchScreen;
		// The IntPtr and initWithCoder constructors are required for items that need 
		// to be able to be created from a xib rather than from managed code
		public Home_iPhone (IntPtr handle)
            : base(handle)
		{
			Initialize ();
		}

		[Export("initWithCoder:")]
		public Home_iPhone (NSCoder coder)
            : base(coder)
		{
			Initialize ();
		}

		public Home_iPhone ()
            : base("Home_iPhone", null)
		{
			Initialize ();
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			Title = "Working with Touch";

			btnTouch.TouchUpInside += (s, e) => {
				if (touchScreen == null) {
					touchScreen = new Touches_iPhone ();
				}
				NavigationController.PushViewController (touchScreen, true);
			};

			btnGestureRecognizers.TouchUpInside += (s, e) => {
				if (gestureScreen == null) {
					gestureScreen = new GestureRecognizers_iPhone ();
				}
				NavigationController.PushViewController (gestureScreen, true);
			};

			btnCustomGestureRecognizer.TouchUpInside += (s, e) => {
				if (customGestureScreen == null) {
					customGestureScreen = new CustomCheckmarkGestureRecognizer_iPhone ();
				}
				NavigationController.PushViewController (customGestureScreen, true);
			};
		}

		private void Initialize ()
		{
		}
	}
}
