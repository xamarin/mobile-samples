namespace Example_Touch
{
	using Example_Touch.Screens.iPhone.Home;
	using Foundation;
	using UIKit;

	[Register("AppDelegate")]
	public class AppDelegate : UIApplicationDelegate
	{
		protected Home_iPhone iPhoneHome;
		protected UINavigationController mainNavController;
		protected UIWindow window;

		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			// create our window
			window = new UIWindow (UIScreen.MainScreen.Bounds);
			window.MakeKeyAndVisible ();

			// instantiate our main navigatin controller and add it's view to the window
			mainNavController = new UINavigationController ();

			iPhoneHome = new Home_iPhone ();
			mainNavController.PushViewController (iPhoneHome, false);

			window.RootViewController = mainNavController;

			return true;
		}
	}
}
