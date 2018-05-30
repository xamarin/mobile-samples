using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using UIKit;
using System.Diagnostics;

namespace XamarinTodoQuickStart
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the 
	// User Interface of the application, as well as listening (and optionally responding) to 
	// application events from iOS.
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		// class-level declarations
		public override UIWindow Window {get; set;}
        public string DeviceToken { get; set; }

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            // NOTE: Don't call the base implementation on a Model class
			// see https://docs.microsoft.com/xamarin/ios/app-fundamentals/delegates-protocols-and-events 

            UIRemoteNotificationType notificationTypes = UIRemoteNotificationType.Alert | 
                UIRemoteNotificationType.Badge | UIRemoteNotificationType.Sound;
            UIApplication.SharedApplication.RegisterForRemoteNotificationTypes(notificationTypes); 

            return true;
        }

        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            // NOTE: Don't call the base implementation on a Model class
			// see https://docs.microsoft.com/xamarin/ios/app-fundamentals/delegates-protocols-and-events 

            string trimmedDeviceToken = deviceToken.Description;
            if (!string.IsNullOrWhiteSpace(trimmedDeviceToken))
            {
                trimmedDeviceToken = trimmedDeviceToken.Trim('<');
                trimmedDeviceToken = trimmedDeviceToken.Trim('>');
            }
            DeviceToken = trimmedDeviceToken;
        }

        public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
        {
            // NOTE: Don't call the base implementation on a Model class
			// see https://docs.microsoft.com/xamarin/ios/app-fundamentals/delegates-protocols-and-events 

            // TODO:: error handling for failed registration (ex: no internet connection)
            var alert = new UIAlertView("Error registering push notifications", error.LocalizedDescription, null, "OK", null);
            alert.Show();

            DeviceToken = string.Empty;
        }

        public override void ReceivedRemoteNotification(UIApplication application, NSDictionary userInfo)
        {
            // NOTE: Don't call the base implementation on a Model class
			// see https://docs.microsoft.com/xamarin/ios/app-fundamentals/delegates-protocols-and-events 

            Debug.WriteLine(userInfo.ToString());
            NSObject inAppMessage;

            bool success = userInfo.TryGetValue(new NSString("inAppMessage"), out inAppMessage);

            if (success)
            {
                var alert = new UIAlertView("Got push notification", inAppMessage.ToString(), null, "OK", null);
                alert.Show();
            }
        }
	}
}