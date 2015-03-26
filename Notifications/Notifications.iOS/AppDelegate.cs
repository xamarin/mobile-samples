namespace Notifications
{
	using Foundation;
	using UIKit;

	[Register("AppDelegate")]
	public class AppDelegate : UIApplicationDelegate
	{
		private MyViewController viewController;
		private UIWindow window;

		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			window = new UIWindow (UIScreen.MainScreen.Bounds);

			viewController = new MyViewController ();
			window.RootViewController = viewController;

			window.MakeKeyAndVisible ();

			// check for a notification
			if (options != null) {
				// check for a local notification
				if (options.ContainsKey (UIApplication.LaunchOptionsLocalNotificationKey)) {
					var localNotification = options [UIApplication.LaunchOptionsLocalNotificationKey] as UILocalNotification;
					if (localNotification != null) {
						new UIAlertView (localNotification.AlertAction, localNotification.AlertBody, null, "OK", null).Show ();
						// reset our badge
						UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;
					}
				}
			}

			return true;
		}

		public override void ReceivedLocalNotification (UIApplication application, UILocalNotification notification)
		{
			// show an alert
			new UIAlertView (notification.AlertAction, notification.AlertBody, null, "OK", null).Show ();

			// reset our badge
			UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;
		}
	}
}
