using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Threading;
using System.Threading.Tasks;
using MonoTouch.ObjCRuntime;
using System.Globalization;

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

		const string prefsSeedDataKey = "SeedDataLoaded";
		public const string PrefsEarliestUpdate = "EarliestUpdate";

		// class-level declarations
		UIWindow _window;
		Screens.Common.TabBarController _tabBar;
		
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			// create a new window instance based on the screen size
			this._window = new UIWindow (UIScreen.MainScreen.Bounds);
		
			BL.Managers.UpdateManager.UpdateFinished += HandleFinishedUpdate;

			// start updating all data in the background
			// by calling this asynchronously, we must check to see if it's finished
			// everytime we want to use/display data.
			//Parallel.Invoke(() => { BL.Managers.UpdateManager.UpdateAll (); });
			new Thread(new ThreadStart(() =>
			{
				var prefs = NSUserDefaults.StandardUserDefaults;
				bool hasSeedData = prefs.BoolForKey(prefsSeedDataKey);
				if (!hasSeedData)
				{	// only happens once
					Console.WriteLine ("Load seed data");
					var appdir = NSBundle.MainBundle.ResourcePath;
					var seedDataFile = appdir + "/Images/SeedData.xml";
					string xml = System.IO.File.ReadAllText (seedDataFile);
					BL.Managers.UpdateManager.UpdateFromFile(xml);
				}
				else
				{
					var earliestUpdateString = prefs.StringForKey(PrefsEarliestUpdate);
					DateTime earliestUpdateTime = DateTime.MinValue;
					if (!String.IsNullOrEmpty(earliestUpdateString))
					{
						CultureInfo provider = CultureInfo.InvariantCulture;

						if (DateTime.TryParse (earliestUpdateString
								, provider
								, System.Globalization.DateTimeStyles.None
								, out earliestUpdateTime))
						{
							Console.WriteLine ("Earliest update time: " + earliestUpdateTime);
						}
					}
					if (earliestUpdateTime < DateTime.Now)
					{	// we're past the earliest update time, so update!
						if (Reachability.IsHostReachable (Constants.ConferenceDataBaseUrl))
						{
							Console.WriteLine ("Reachability okay, update conference from server"); 
							BL.Managers.UpdateManager.UpdateConference ();
						}
						else
						{	// no network
							Console.WriteLine ("No network, can't update data for now");
						}
					} else Console.WriteLine ("Too soon to update " + DateTime.Now);
				}
			})).Start();

			this._tabBar = new Screens.Common.TabBarController();
			
			// gotta love Appearance in iOS5
			// but should we manually set this *everywhere* for iOS4 too??
			UINavigationBar.Appearance.TintColor = UIColor.DarkGray;
			//UIToolbar.Appearance.TintColor = UIColor.DarkGray;			

			this._window.RootViewController = this._tabBar;
			this._window.MakeKeyAndVisible ();

			return true;
		}
		
		public override void WillTerminate (UIApplication application)
		{
			BL.Managers.UpdateManager.UpdateFinished -= HandleFinishedUpdate;
		}
		
		/// <summary>
		/// When updates finished, save the time so we don't check again
		/// too soon.
		/// </summary>
		void HandleFinishedUpdate (object sender, EventArgs ea)
		{
			var prefs = NSUserDefaults.StandardUserDefaults;
			var args = ea as UpdateFinishedEventArgs;
			if (args != null)
			{
				// if we fail, we'll try again in an hour
				var earliestUpdate = DateTime.Now.AddHours(1);
			
				if (args.Success) 
				{
					prefs.SetBool (true, prefsSeedDataKey);
					
					if (args.UpdateType == UpdateType.SeedData)
					{	// SeedData is already out-of-date
						earliestUpdate = DateTime.Now; 
					}
					else 
					{	// having succeeded, we won't try again for another day
						earliestUpdate = DateTime.Now.AddDays(1);
					}
					if (args.UpdateType == UpdateType.Conference)
					{	// now get the exhibitors, but don't really care if it fails
						BL.Managers.UpdateManager.UpdateExhibitors();
					}
				}
				
#if DEBUG
				earliestUpdate = DateTime.Now; // for testing, ALWAYS update :)
#endif	
				CultureInfo provider = CultureInfo.InvariantCulture;
				var earliestUpdateString = earliestUpdate.ToString(provider);					
				prefs.SetString (earliestUpdateString, PrefsEarliestUpdate);
			}
			prefs.Synchronize ();
		}
	}
}