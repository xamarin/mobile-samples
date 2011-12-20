using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace MWC.iOS
{
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		// class-level declarations
		UIWindow _window;
		Screens.Common.TabBarController _tabBar;
		
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			// create a new window instance based on the screen size
			this._window = new UIWindow (UIScreen.MainScreen.Bounds);
			
			this._tabBar = new Screens.Common.TabBarController();
			
			this._window.RootViewController = this._tabBar;
			this._window.MakeKeyAndVisible ();
			
			// TODO: update in the background, otherwise we might not start up in time [CD]
			BL.Managers.UpdateManager.UpdateAll ();

			return true;
		}
	}
}

