using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Threading;
using System.Threading.Tasks;
using MonoTouch.ObjCRuntime;

namespace MWC.iOS
{
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		public const string ImageNotFavorite = "Images/favorite.png";
		public const string ImageIsFavorite = "Images/favorited.png";
		public const string ImageCalendarTemplate = "Images/caltemplate.png";
		
		public const float Font16pt = 22f;
		public const float Font10_5pt = 14f;
		public const float Font10pt = 13f;
		public const float Font9pt = 12f;
		public const float Font7_5pt = 10f;

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
			new Thread(new ThreadStart(() =>
			{
				var prefs = NSUserDefaults.StandardUserDefaults;
				bool hasSeedData = prefs.BoolForKey("SeedDataLoaded");
				if (!hasSeedData)
				{	// only happens once
					Console.WriteLine ("Load seed data");
					var appdir = NSBundle.MainBundle.ResourcePath;
					var seedDataFile = appdir + "/Images/SeedData.xml";
					string xml = System.IO.File.ReadAllText (seedDataFile);
					BL.Managers.UpdateManager.UpdateFinished += HandleFinishedLaunching;
					BL.Managers.UpdateManager.UpdateFromFile(xml);
				}
				else
				{
					if (Reachability.IsHostReachable (Constants.ConferenceDataBaseUrl))
					{
						Console.WriteLine ("Reachability okay, update from server"); 
						BL.Managers.UpdateManager.UpdateAll ();
					}
					else
					{	// no network
						Console.WriteLine ("No network, can't update data for now");
					}
				}
			})).Start();

			this._tabBar = new Screens.Common.TabBarController();
			
			// gotta love Appearance in iOS5
			// but should we manually set this *everywhere* for iOS4 too??
			UINavigationBar.Appearance.TintColor = UIColor.DarkGray;

			this._window.RootViewController = this._tabBar;
			this._window.MakeKeyAndVisible ();

			return true;
		}
		
		public override void WillTerminate (UIApplication application)
		{
			BL.Managers.UpdateManager.UpdateFinished -= HandleFinishedLaunching;
		}

		void HandleFinishedLaunching (object sender, EventArgs e)
		{	// assume success for now
			var prefs = NSUserDefaults.StandardUserDefaults;
			prefs.SetBool (true, "SeedDataLoaded");
		}
	}
}