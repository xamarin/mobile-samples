using System;

using UIKit;
using Foundation;

namespace Location.iOS
{
	[Register("AppDelegate")]
	public class AppDelegate : UIApplicationDelegate
	{
		#region declarations and properties
		
		protected UIWindow window;
		protected MainViewController mainViewController;
		public static LocationManager Manager = null;

		#endregion
	
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			// create our window
			window = new UIWindow (UIScreen.MainScreen.Bounds);
			window.MakeKeyAndVisible ();
			
			// instantiate and load our main screen onto the window
			mainViewController = new MainViewController ();
			window.RootViewController = mainViewController;

			// as soon as the app is done launching, begin generating location updates in the location manager
			Manager = new LocationManager();
			Manager.StartLocationUpdates();

			return true;
		}

		public override void WillEnterForeground (UIApplication application)
		{
			Console.WriteLine ("App will enter foreground");
		}
		
		// Runs when the activation transitions from running in the background to
		// being the foreground application.
		// Also gets hit on app startup
		public override void OnActivated (UIApplication application)
		{
			Console.WriteLine ("App is becoming active");
		}
		
		public override void OnResignActivation (UIApplication application)
		{
			Console.WriteLine ("App moving to inactive state.");
		}
		
		public override void DidEnterBackground (UIApplication application)
		{
			Console.WriteLine ("App entering background state.");
			Console.WriteLine ("Now receiving location updates in the background");
		} 
	}
}
