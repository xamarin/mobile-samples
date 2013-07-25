using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace SQLite.iOS.Sample
{
	[Register("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		private readonly SQLiteAsyncConnection db = new SQLiteAsyncConnection (Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Personal), "todo.db"));

		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			db.CreateTablesAsync<List, ListItem>().Wait();

			window = new UIWindow (UIScreen.MainScreen.Bounds);
			window.RootViewController = new UINavigationController (new TaskListsViewController (this.db));
			window.MakeKeyAndVisible();

			return true;
		}

		private UIWindow window;

		private static int busy;
		public static void AddActivity()
		{
			UIApplication.SharedApplication.InvokeOnMainThread (() => {
				if (busy++ < 1)
					UIApplication.SharedApplication.NetworkActivityIndicatorVisible = true;
			});
		}

		public static void FinishActivity()
		{
			UIApplication.SharedApplication.InvokeOnMainThread(() => {
				if (--busy < 1)
					UIApplication.SharedApplication.NetworkActivityIndicatorVisible = false;
			});
		}
	}
}