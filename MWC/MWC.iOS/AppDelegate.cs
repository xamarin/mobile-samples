using System;
using System.Globalization;
using System.IO;
using System.Threading;
using Foundation;
using UIKit;
using ObjCRuntime;

namespace MWC.iOS {
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate {
		public const string ImageNotFavorite = "Images/star-grey45.png";
		public const string ImageIsFavorite  = "Images/star-gold45.png";
		public const string ImageCalendarPad = "Images/calendar~ipad.png";
		public const string ImageCalendarPhone = "Images/calendar~iphone.png";
		public const string ImageLocation    = "Images/building.png";

		public const string ImageEmptyExhibitors = "Images/Empty/exhibitors.png";
		public const string ImageEmptyNews     = "Images/Empty/news.png";
		public const string ImageEmptySession  = "Images/Empty/session.png";
		public const string ImageEmptySpeaker  = "Images/Empty/speaker.png";
		public const string ImageEmptyTwitter  = "Images/Empty/twitter.png";
		
		public const float Font16pt = 22f;
		public const float Font10_5pt = 14f;
		public const float Font10pt = 13f;
		public const float Font9pt = 12f;
		public const float Font7_5pt = 10f;

		public static readonly UIColor ColorNavBarTint = UIColor.FromRGB (55, 87 ,118);
		public static readonly UIColor ColorTextHome = UIColor.FromRGB (192, 205, 223);
		public static readonly UIColor ColorHeadingHome = UIColor.FromRGB (150, 210, 254);
		public static readonly UIColor ColorCellBackgroundHome = UIColor.FromRGB (36, 54, 72);
		public static readonly UIColor ColorTextLink = UIColor.FromRGB (9, 9, 238);		

		public const string PrefsEarliestUpdate = "EarliestUpdate";
		
		public static readonly NSString NotificationWillChangeStatusBarOrientation = new NSString("UIApplicationWillChangeStatusBarOrientationNotification");
		public static readonly NSString NotificationDidChangeStatusBarOrientation = new NSString("UIApplicationDidChangeStatusBarOrientationNotification");		
		public static readonly NSString NotificationOrientationDidChange = new NSString("UIDeviceOrientationDidChangeNotification");
		public static readonly NSString NotificationFavoriteUpdated = new NSString("NotificationFavoriteUpdated");
		// class-level declarations
		UIWindow window;
		Screens.Common.TabBarController tabBar;
		


		public static bool IsPhone {
			get {
				return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone;
			}
		}
		public static bool IsPad {
			get {
				return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad;
			}
		}
		public static bool HasRetina {
			get {
				if (UIKit.UIScreen.MainScreen.RespondsToSelector(new Selector("scale")))
					return (UIKit.UIScreen.MainScreen.Scale == 2.0);
				else
					return false;
			}
		}

		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			// create a new window instance based on the screen size
			window = new UIWindow (UIScreen.MainScreen.Bounds);
		
			BL.Managers.UpdateManager.UpdateFinished += HandleFinishedUpdate;
			


//NOTE: this is a quick response to Apple's disapproval of the sqlite living in the /Documents/ folder
// in the previous versions of the app. A quick way to preserve the favorites when the sqlite is moved
// to /Library/
//HACK: not a good idea in the FinishedLaunching method, but it will do for now...
//HACK: we need to do this before triggering the static ctor on MwcDatabase!
var docsPath = Environment.GetFolderPath (Environment.SpecialFolder.Personal);
string oldDBLocation = Path.Combine(docsPath, "MwcDB.db3");	 // in Document					
string newDBLocation = oldDBLocation.Replace("Documents/MwcDB.db3", "Library/MwcDB.db3");
try {	
	ConsoleD.WriteLine ("oldDbLocation="+oldDBLocation);
	if (File.Exists(oldDBLocation)) { // won't normally be there in new installs
		File.Delete(newDBLocation);  // static ctor will have created it
		File.Move(oldDBLocation, newDBLocation);
		ConsoleD.WriteLine ("Moved " + oldDBLocation + " to " + newDBLocation);
	} else ConsoleD.WriteLine ("oldDBLocation didn't exist?");
} catch (Exception ex) {
	ConsoleD.WriteLine ("Well, we tried! Couldn't save the old favorites..." + ex.Message);
	File.Delete(oldDBLocation);
}




			// start updating all data in the background
			// by calling this asynchronously, we must check to see if it's finished
			// everytime we want to use/display data.
			new Thread(new ThreadStart(() => {
				var prefs = NSUserDefaults.StandardUserDefaults;

				bool hasSeedData = BL.Managers.UpdateManager.HasDataAlready;
				ConsoleD.WriteLine ("hasSeedData="+hasSeedData);
				if (!hasSeedData) {
					// only happens when the database is empty (or wasn't there); use local file update
					ConsoleD.WriteLine ("Load seed data");
					var appdir = NSBundle.MainBundle.ResourcePath;
					var seedDataFile = appdir + "/Images/SeedData.xml";
					string xml = System.IO.File.ReadAllText (seedDataFile);
					BL.Managers.UpdateManager.UpdateFromFile(xml);

					ConsoleD.WriteLine("Database lives at: "+MWC.DL.MwcDatabase.DatabaseFilePath);
					// We SHOULDN'T skip backup because we are saving the Favorites in the same sqlite
					// database as the sessions are stored. A more iCloud-friendly design would be 
					// to keep the user-data separate from the server-generated data...
					NSFileManager.SetSkipBackupAttribute (MWC.DL.MwcDatabase.DatabaseFilePath, true);
				} else {
					// if there's already data in the database, do/attempt server update
					//ConsoleD.WriteLine("SkipBackup: "+NSFileManager.GetSkipBackupAttribute (MWC.DL.MwcDatabase.DatabaseFilePath));

					var earliestUpdateString = prefs.StringForKey(PrefsEarliestUpdate);
					DateTime earliestUpdateTime = DateTime.MinValue;
					if (!String.IsNullOrEmpty(earliestUpdateString)) {
						CultureInfo provider = CultureInfo.InvariantCulture;

						if (DateTime.TryParse (earliestUpdateString
								, provider
								, System.Globalization.DateTimeStyles.None
								, out earliestUpdateTime)) {
							ConsoleD.WriteLine ("Earliest update time: " + earliestUpdateTime);
						}
					}
					if (earliestUpdateTime < DateTime.Now) {
						// we're past the earliest update time, so update!
						if (Reachability.IsHostReachable (Constants.ConferenceDataBaseUrl)) {
							ConsoleD.WriteLine ("Reachability okay, update conference from server"); 
							BL.Managers.UpdateManager.UpdateConference ();
						} else {
							// no network
							ConsoleD.WriteLine ("No network, can't update data for now");
						}
					} else ConsoleD.WriteLine ("Too soon to update " + DateTime.Now);
				}
			})).Start();

			tabBar = new Screens.Common.TabBarController ();
			
			// couldn't do RespondsToSelector() on static 'Appearance' property)
			var majorVersionString = UIDevice.CurrentDevice.SystemVersion.Substring (0,1);
			var majorVersion = Convert.ToInt16(majorVersionString);
			if (majorVersion >= 5) { // gotta love Appearance in iOS5
				UINavigationBar.Appearance.TintColor = ColorNavBarTint;			
			}
			window.RootViewController = tabBar;
			window.MakeKeyAndVisible ();

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
			if (args != null) {
				// if we fail, we'll try again in an hour
				var earliestUpdate = DateTime.Now.AddHours(1);
			
				if (args.Success)  {
					if (args.UpdateType == UpdateType.SeedData) {
						// SeedData is already out-of-date
						earliestUpdate = DateTime.Now; 
					} else  {
						// having succeeded, we won't try again for another day
						earliestUpdate = DateTime.Now.AddDays(1);
					}
					if (args.UpdateType == UpdateType.Conference) {
						// now get the exhibitors, but don't really care if it fails
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
		
		/// <summary>
		/// When we receive a memory warning, clear the MT.D image cache
		/// </summary>
		public override void ReceiveMemoryWarning (UIApplication application)
		{
			ConsoleD.WriteLine("==== Received Memory Warning ====");
			MonoTouch.Dialog.Utilities.ImageLoader.Purge();
		}
	}
}