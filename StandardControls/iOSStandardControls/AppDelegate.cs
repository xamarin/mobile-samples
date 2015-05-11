using System;
using UIKit;
using Foundation;

namespace Example_StandardControls
{
	[Register ("AppDelegate")]
	public class AppDelegate : UIApplicationDelegate
	{
		UIWindow window;
		UINavigationController mainNavController;
		Example_StandardControls.Screens.iPhone.Home.HomeNavController iPhoneHome;
		Example_StandardControls.Screens.iPad.Home.HomeNavController iPadHome;

		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			// create our window
			window = new UIWindow (UIScreen.MainScreen.Bounds);
			window.MakeKeyAndVisible ();

			// instantiate our main navigatin controller and add it's view to the window
			mainNavController = new UINavigationController ();
			mainNavController.NavigationBar.Translucent = false;

			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone) {
				iPhoneHome = new Example_StandardControls.Screens.iPhone.Home.HomeNavController ();
				mainNavController.PushViewController (iPhoneHome, false);
			} else {
				iPadHome = new Example_StandardControls.Screens.iPad.Home.HomeNavController ();
				mainNavController.PushViewController (iPadHome, false);
			}

			window.RootViewController = mainNavController;

			return true;
		}
	}
}
