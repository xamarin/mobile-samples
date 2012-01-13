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
		public const string ImageNotFavorite = "Images/favorite.png";
		public const string ImageIsFavorite = "Images/favorited.png";
		public const string ImageCalendarTemplate = "Images/caltemplate.png";

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
			//Parallel.Invoke(() => { BL.Managers.UpdateManager.UpdateAll (); });
			new Thread(new ThreadStart(() => { BL.Managers.UpdateManager.UpdateAll (); })).Start();

			this._tabBar = new Screens.Common.TabBarController();
			
			// gotta love Appearance in iOS5
			// but should we manually set this *everywhere* for iOS4 too??
			UINavigationBar.Appearance.TintColor = UIColor.DarkGray;

			this._window.RootViewController = this._tabBar;
			this._window.MakeKeyAndVisible ();

			return true;
		}
	}
}

