using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using UIKit;
using Microsoft.WindowsAzure.MobileServices;

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

		public static Func<NSUrl, bool> ResumeWithURL;

		public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
        {
            return ResumeWithURL != null && ResumeWithURL(url);
        }
	}
}