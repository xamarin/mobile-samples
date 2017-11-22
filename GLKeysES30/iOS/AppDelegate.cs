using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using UIKit;

namespace GLKeysES30
{
	/// <summary>
	/// The UIApplicationDelegate for the application. This class is responsible for launching the 
	/// User Interface of the application, as well as listening (and optionally responding) to 
	/// application events from iOS.
	/// </summary>
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		// class-level declarations
		UIWindow window;
		OpenGLViewController viewController;
		// This method is invoked when the application has loaded its UI and is ready to run
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			// create a new window instance based on the screen size
			window = new UIWindow (UIScreen.MainScreen.Bounds);

			// load the appropriate UI, depending on whether the app is running on an iPhone or iPad
			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone) {
				viewController = new OpenGLViewController ("OpenGLViewController_iPhone", null);
			} else {
				viewController = new OpenGLViewController ("OpenGLViewController_iPad", null);
			}
			window.RootViewController = viewController;

			// make the window visible
			window.MakeKeyAndVisible ();
			
			return true;
		}
	}
}
