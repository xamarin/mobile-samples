using Foundation;
using UIKit;
using CocosSharp;

namespace ActionProject
{
	[Register ("AppDelegate")]
	public class AppDelegate : UIApplicationDelegate
	{
		public override void FinishedLaunching (UIApplication application)
		{
			var ccApp = new CCApplication {
				ApplicationDelegate = new GameAppDelegate ()
			};

			ccApp.StartGame ();
		}

		static void Main (string[] args)
		{
			UIApplication.Main (args, null, "AppDelegate");
		}
	}
}


