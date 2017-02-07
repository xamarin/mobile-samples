using System;
using System.IO;
using Foundation;
using SQLite;
using TaskyNetStandard.Core;
using UIKit;

namespace TaskyNetStandard.iOS 
{

	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate 
	{
		// class-level declarations
		UIWindow window;
		UINavigationController navController;
		UITableViewController homeViewController;

		public static AppDelegate Current { get; private set; }
		public TodoItemManager TodoManager { get; set; }
		SQLiteConnection conn;

		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			Current = this;

			// create a new window instance based on the screen size
			window = new UIWindow (UIScreen.MainScreen.Bounds);
			
			// make the window visible
			window.MakeKeyAndVisible ();


			// Create the database file
			var sqliteFilename = "TodoItemDB.db3";
			// we need to put in /Library/ on iOS5.1 to meet Apple's iCloud terms
			// (they don't want non-user-generated data in Documents)
			string documentsPath = Environment.GetFolderPath (Environment.SpecialFolder.Personal); // Documents folder
			string libraryPath = Path.Combine (documentsPath, "..", "Library"); // Library folder
			var path = Path.Combine(libraryPath, sqliteFilename);
			conn = new SQLiteConnection(path);
			TodoManager = new TodoItemManager(conn);


			// create our nav controller
			navController = new UINavigationController ();

			// create our Todo list screen
			homeViewController = new HomeScreen ();

			// push the view controller onto the nav controller and show the window
			navController.PushViewController(homeViewController, false);
			window.RootViewController = navController;
			window.MakeKeyAndVisible ();
			
			return true;
		}
	}
}