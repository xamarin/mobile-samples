using Foundation;
using UIKit;

namespace MonoGameTvOs
{
	[Register("AppDelegate")]
	public class Application : UIApplicationDelegate
	{
		Game1 game;

		public override bool FinishedLaunching (UIApplication application, NSDictionary launchOptions)
		{
			game = new Game1 ();
			game.Run ();



			return true;
		}

		// This is the main entry point of the application.
		static void Main (string [] args)
		{
			// if you want to use a different Application Delegate class from "AppDelegate"
			// you can specify it here.
			UIApplication.Main (args, null, "AppDelegate");
		}
	}
}
