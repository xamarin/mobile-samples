using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using UIKit;
using WindowsAzure.Messaging;


namespace NotificationHubQuickStart
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : UIApplicationDelegate
    {
        private SBNotificationHub Hub { get; set; }

        // class-level declarations
        public override UIWindow Window
        {
            get;
            set;
        }
        // This method is invoked when the application is about to move from active to inactive state.
        // OpenGL applications should use this method to pause.
        public override void OnResignActivation(UIApplication application)
        {
        }
        // This method should be used to release shared resources and it should store the application state.
        // If your application supports background exection this method is called instead of WillTerminate
        // when the user quits.
        public override void DidEnterBackground(UIApplication application)
        {
        }

        /// This method is called as part of the transiton from background to active state.
        public override void WillEnterForeground(UIApplication application)
        {
        }

        /// This method is called when the application is about to terminate. Save data, if needed. 
        public override void WillTerminate(UIApplication application)
        {
        }

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

            Hub = new SBNotificationHub(Constants.ConnectionString, Constants.NotificationHubPath);

            Hub.UnregisterAllAsync (deviceToken, (error) => {
                if (error != null) 
                {
                    Console.WriteLine("Error calling Unregister: {0}", error.ToString());
                    return;
                } 

                NSSet tags = null; // create tags if you want
                Hub.RegisterNativeAsync(deviceToken, tags, (errorCallback) => {
                    if (errorCallback != null)
                        Console.WriteLine("RegisterNativeAsync error: " + errorCallback.ToString());
                });
            });
        }

        public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
        {
            // NOTE: Don't call the base implementation on a Model class
			// see https://docs.microsoft.com/xamarin/ios/app-fundamentals/delegates-protocols-and-events

            /*
            // TODO:: error handling for failed registration (ex: no internet connection)
            var alert = new UIAlertView("Error registering push notifications", error.LocalizedDescription, null, "OK", null);
            alert.Show();
            */
            Console.WriteLine("Failed to register for remote notifications");
        }

        public override void ReceivedRemoteNotification(UIApplication application, NSDictionary userInfo)
        {
            ProcessNotification(userInfo, false);
        }

        void ProcessNotification(NSDictionary options, bool fromFinishedLaunching)
        {
            // Check to see if the dictionary has the aps key.  This is the notification payload you would have sent
            if (null != options && options.ContainsKey(new NSString("aps")))
            {
                //Get the aps dictionary
                NSDictionary aps = options.ObjectForKey(new NSString("aps")) as NSDictionary;

                string alert = string.Empty;

                //Extract the alert text
                //NOTE: If you're using the simple alert by just specifying "  aps:{alert:"alert msg here"}  "
                //      this will work fine.  But if you're using a complex alert with Localization keys, etc., your "alert" object from the aps dictionary
                //      will be another NSDictionary... Basically the json gets dumped right into a NSDictionary, so keep that in mind
                if (aps.ContainsKey(new NSString("alert")))
                    alert = (aps [new NSString("alert")] as NSString).ToString();

                //If this came from the ReceivedRemoteNotification while the app was running,
                // we of course need to manually process things like the sound, badge, and alert.
                if (!fromFinishedLaunching)
                {
                    //Manually show an alert
                    if (!string.IsNullOrEmpty(alert))
                    {
                        UIAlertView avAlert = new UIAlertView("Notification", alert, null, "OK", null);
                        avAlert.Show();
                    }
                }           
            }
        }
    }
}

