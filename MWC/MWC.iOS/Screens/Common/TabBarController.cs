using System;
using MonoTouch.UIKit;

namespace MWC.iOS.Screens.Common
{
	public class TabBarController : UITabBarController
	{
		UIViewController _homeScreen = null;
		UIViewController _speakersScreen;
		Screens.Common.Map.MapController _mapScreen;
		Screens.Common.About.AboutXamScreen _aboutScreen;
		
		public TabBarController ()
		{
		}
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			// home tab
			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone)
				_homeScreen = new Screens.iPhone.Home.HomeScreen();
			//else
			//	this._homeScreen = new Screens.iPad.Home();
			
			// maps tab
			//TODO: pass in the actual frame (minus tab bar, status bar crap)
			_mapScreen = new Screens.Common.Map.MapController(UIScreen.MainScreen.Bounds);
			_mapScreen.TabBarItem = new UITabBarItem();
			_mapScreen.TabBarItem.Title = "Map";
			
			
			// about tab
			this._aboutScreen = new Screens.Common.About.AboutXamScreen();
			this._aboutScreen.TabBarItem = new UITabBarItem();
			this._aboutScreen.TabBarItem.Title = "Xamarin";
				
			
			// create our array of controllers
			var viewControllers = new UIViewController[] {
				this._homeScreen,
				this._mapScreen,
				this._aboutScreen
			};
			
			// attach the view controllers
			this.ViewControllers = viewControllers;
			
			// tell the tab bar which controllers are allowed to customize. if we 
			// don't set this, it assumes all controllers are customizable.
			//CustomizableViewControllers = customizableControllers;
			
			// set our selected item
			SelectedViewController = this._homeScreen;
			
		}
	}
}

