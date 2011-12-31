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
		public const string NewsBaseUrl = "news.google.com"; // for Reachability test
		public const string NewsUrl = "http://news.google.com/news?q=mobile%20world%20congress&output=rss";
		public const string TwitterUrl = "http://search.twitter.com/search.atom?q=%40mobileworldlive&show-user=true&rpp=20";

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
			
			this._window.RootViewController = this._tabBar;
			this._window.MakeKeyAndVisible ();

			return true;
		}
	}
}

