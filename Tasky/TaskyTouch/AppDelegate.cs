using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Tasky
{
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		// class-level declarations
		UIWindow window;
		UINavigationController _navController;
		UITableViewController _homeViewController;

		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			// create a new window instance based on the screen size
			window = new UIWindow (UIScreen.MainScreen.Bounds);
			
			// make the window visible
			window.MakeKeyAndVisible ();
			
			// create our nav controller
			this._navController = new UINavigationController ();
			this._navController.NavigationBar.Translucent = false;

			// create our home controller based on the device
			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone) {
				this._homeViewController = new Tasky.Screens.iPhone.Home.controller_iPhone();
			} else {
//				this._viewController = new Hello_UniversalViewController ("Hello_UniversalViewController_iPad", null);
			}
			
			// push the view controller onto the nav controller and show the window
			this._navController.PushViewController(this._homeViewController, false);
			window.RootViewController = this._navController;
			window.MakeKeyAndVisible ();
			
			return true;
		}
	}
}

