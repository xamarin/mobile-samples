using System;
using System.Collections.Generic;

using Foundation;
using UIKit;

namespace SoMA {
	public partial class MainScreen : UIViewController {
		public MainScreen (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			NavigationController.NavigationBar.Translucent = false;
			Title = "Xamarin.SoMA";
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);

			if (UIDevice.CurrentDevice.CheckSystemVersion(7,0))
				ItemTableView.ContentInset = new UIEdgeInsets (TopLayoutGuide.Length, 0, 0, 0);

			var items = AppDelegate.Database.GetItems ();
			Console.WriteLine ("items: {0}", items.Count);
			ItemTableView.Source = new MainScreenSource(items);
		}

		public override void PrepareForSegue (UIStoryboardSegue segue, NSObject sender)
		{
			if (segue.Identifier == "view") { // set in Storyboard
				var tdvc = segue.DestinationViewController as PhotoScreen;
				if (tdvc != null) {
					var source = ItemTableView.Source as MainScreenSource;
					var rowPath = ItemTableView.IndexPathForSelectedRow;
					tdvc.SetItem(source.GetItem(rowPath.Row));
				}
			}
		}
	}
}