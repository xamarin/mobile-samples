using System;
using MonoTouch.UIKit;
using MonoTouch.Dialog;

namespace MWC.iOS.Screens.Common
{
	public class TabBarController : UITabBarController
	{
		UINavigationController _homeNav = null, _speakerNav = null;
		UIViewController _homeScreen = null;
		//Screens.Common.Speakers.SpeakersScreen _speakersScreen;
		Screens.iPhone.Speakers.SpeakersScreen _speakersScreen;
		DialogViewController _sessionsScreen;
		DialogViewController _twitterFeedScreen;
		DialogViewController _newsFeedScreen;
		DialogViewController _exhibitorsScreen;
		Screens.Common.Map.MapController _mapScreen;
		Screens.Common.About.AboutXamScreen _aboutScreen;
		
		public TabBarController ()
		{
		}
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			// home nav
			this._homeNav = new UINavigationController();
			
			// home tab
			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone)
				_homeScreen = new Screens.iPhone.Home.HomeScreen();
			//else
			//	this._homeScreen = new Screens.iPad.Home();
			this._homeScreen.TabBarItem = new UITabBarItem();
			this._homeScreen.TabBarItem.Title = "Schedule";
			
			this._homeNav.PushViewController ( this._homeScreen, false );
			this._homeNav.TabBarItem = new UITabBarItem();
			this._homeNav.TabBarItem.Title = "Schedule";
			
			// speakers tab
			this._speakersScreen = new Screens.iPhone.Speakers.SpeakersScreen();			
			this._speakerNav = new UINavigationController();
			this._speakerNav.TabBarItem = new UITabBarItem();
			this._speakerNav.TabBarItem.Title = "Speakers";
			this._speakerNav.PushViewController ( this._speakersScreen, false );
			
			// sessions
			this._sessionsScreen = new DialogViewController(new RootElement("Sessions"));
			this._sessionsScreen.TabBarItem = new UITabBarItem();
			this._sessionsScreen.TabBarItem.Title = "Sessions";
			
			// maps tab
			//TODO: pass in the actual frame (minus tab bar, status bar crap)
			this._mapScreen = new Screens.Common.Map.MapController(UIScreen.MainScreen.Bounds);
			this._mapScreen.TabBarItem = new UITabBarItem();
			this._mapScreen.TabBarItem.Title = "Map";
			
			// twitter feed
			this._twitterFeedScreen = new DialogViewController(new RootElement("Twitter Feed"));
			this._twitterFeedScreen.TabBarItem = new UITabBarItem();
			this._twitterFeedScreen.TabBarItem.Title = "Twitter";
			
			// news
			this._exhibitorsScreen = new Screens.iPhone.Exhibitors.ExhibitorsScreen();
			this._exhibitorsScreen.TabBarItem = new UITabBarItem();
			this._exhibitorsScreen.TabBarItem.Title = "Exhibitors";

			// news
			this._newsFeedScreen = new DialogViewController(new RootElement("News"));
			this._newsFeedScreen.TabBarItem = new UITabBarItem();
			this._newsFeedScreen.TabBarItem.Title = "News";

			// about tab
			this._aboutScreen = new Screens.Common.About.AboutXamScreen();
			this._aboutScreen.TabBarItem = new UITabBarItem();
			this._aboutScreen.TabBarItem.Title = "Xamarin";
			
			// create our array of controllers
			var viewControllers = new UIViewController[] {
				this._homeNav,
				this._speakerNav,
				this._sessionsScreen,
				this._mapScreen,
				this._exhibitorsScreen,
				this._twitterFeedScreen,
				this._newsFeedScreen,
				this._aboutScreen
			};
			
			// attach the view controllers
			this.ViewControllers = viewControllers;
			
			// tell the tab bar which controllers are allowed to customize. if we 
			// don't set this, it assumes all controllers are customizable.
			//CustomizableViewControllers = customizableControllers;
			
			// set our selected item
			SelectedViewController = this._homeNav;
			
		}
	}
}

