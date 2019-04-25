using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using UIKit;
using System.IO;

using TaskyVBNetStandard;
using CoreGraphics;

namespace Tasky {
	[Foundation.Register("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate {
		// class-level declarations
		UIWindow window;
		UINavigationController navController;
		UITableViewController homeViewController;

		public static AppDelegate Current { get; private set; }
        public TaskManager TaskMgr { get; set; }


		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			Current = this;

			// create a new window instance based on the screen size
			window = new UIWindow ((CGRect)UIScreen.MainScreen.Bounds);
			
			// make the window visible
			window.MakeKeyAndVisible ();
			
			// create our nav controller
			navController = new UINavigationController ();

			// create our home controller based on the device
			homeViewController = new Tasky.Screens.iPhone.Home.PhoneHomeScreen();
			
			// Styling
			UINavigationBar.Appearance.TintColor = UIColor.FromRGB (38, 117 ,255); // nice blue
			UITextAttributes ta = new UITextAttributes();
			ta.Font = UIFont.FromName ("AmericanTypewriter-Bold", (nfloat)0f);
			UINavigationBar.Appearance.SetTitleTextAttributes(ta);
			ta.Font = UIFont.FromName ("AmericanTypewriter", (nfloat)0f);
			UIBarButtonItem.Appearance.SetTitleTextAttributes(ta, UIControlState.Normal);
			

			var sqliteFilename = "TaskDB.xml";
			// we need to put in /Library/ on iOS5.1 to meet Apple's iCloud terms
			// (they don't want non-user-generated data in Documents)
			string documentsPath = Environment.GetFolderPath (Environment.SpecialFolder.Personal); // Documents folder
			string libraryPath = Path.Combine (documentsPath, "../Library/"); // Library folder
            var path = Path.Combine(libraryPath, sqliteFilename);
            
			TaskMgr = new TaskManager(path);

			// push the view controller onto the nav controller and show the window
			navController.PushViewController(homeViewController, false);
			window.RootViewController = navController;
			window.MakeKeyAndVisible ();
			
			return true;
		}
	}
}