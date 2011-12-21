using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Threading;
using System.Threading.Tasks;

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
			
			// start updating all data in the background
			// by calling this asynchronously, we must check to see if it's finished
			// everytime we want to use/display data.
			Parallel.Invoke(() => { BL.Managers.UpdateManager.UpdateAll (); });

			this._tabBar = new Screens.Common.TabBarController();
			
			this._window.RootViewController = this._tabBar;
			this._window.MakeKeyAndVisible ();

			return true;
		}
	}
}

