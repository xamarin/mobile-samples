using MonoTouch.Dialog;
using MonoTouch.UIKit;

namespace MWC.iOS.Screens.Common
{
	public class TabBarController : UITabBarController
	{
		UINavigationController _homeNav = null, _speakerNav = null, _sessionNav = null;
		UIViewController _homeScreen = null;
		DialogViewController _speakersScreen;
		DialogViewController _sessionsScreen;
		DialogViewController _twitterFeedScreen;
		DialogViewController _newsFeedScreen;
		DialogViewController _exhibitorsScreen;
		DialogViewController _favoritesScreen;
		Screens.Common.Map.MapController _mapScreen;
		Screens.Common.About.AboutXamScreen _aboutScreen;
		
		public TabBarController ()
		{
		}
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			// home tab
			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone){
				_homeScreen = new Screens.iPhone.Home.HomeScreen();
				_homeScreen.Title = "Schedule";
			}
			//else
			//	this._homeScreen = new Screens.iPad.Home();
			this._homeNav = new UINavigationController();
			this._homeNav.PushViewController ( this._homeScreen, false );			
			this._homeNav.Title = "Schedule";
			this._homeNav.TabBarItem = new UITabBarItem("Schedule"
										, UIImage.FromBundle("Images/Tabs/schedule.png"), 0);
			

			// speakers tab
			this._speakersScreen = new Screens.iPhone.Speakers.SpeakersScreen();			
			this._speakerNav = new UINavigationController();
			this._speakerNav.TabBarItem = new UITabBarItem("Speakers"
										, UIImage.FromBundle("Images/Tabs/speakers.png"), 1);
			this._speakerNav.PushViewController ( this._speakersScreen, false );
			
			// sessions
			this._sessionsScreen = new Screens.iPhone.Sessions.SessionsScreen();
			this._sessionNav = new UINavigationController();
			this._sessionNav.TabBarItem = new UITabBarItem("Sessions"
										, UIImage.FromBundle("Images/Tabs/sessions.png"), 2);
			this._sessionNav.PushViewController ( this._sessionsScreen, false );
			
			// maps tab
			//TODO: pass in the actual frame (minus tab bar, status bar crap)
			this._mapScreen = new Screens.Common.Map.MapController(UIScreen.MainScreen.Bounds);
			this._mapScreen.TabBarItem = new UITabBarItem("Maps"
										, UIImage.FromBundle("Images/Tabs/maps.png"), 3);
			
			// twitter feed
			this._twitterFeedScreen = new MWC.iOS.Screens.iPhone.Twitter.TwitterScreen();
			this._twitterFeedScreen.TabBarItem = new UITabBarItem();
			this._twitterFeedScreen.TabBarItem.Title = "Twitter";
			
			// exhibitors
			this._exhibitorsScreen = new Screens.iPhone.Exhibitors.ExhibitorsScreen();
			this._exhibitorsScreen.TabBarItem = new UITabBarItem();
			this._exhibitorsScreen.TabBarItem.Title = "Exhibitors";

			// news
			this._newsFeedScreen = new MWC.iOS.Screens.Common.News.NewsScreen();
			this._newsFeedScreen.TabBarItem = new UITabBarItem();
			this._newsFeedScreen.TabBarItem.Title = "News";
			
			// favorites
			this._favoritesScreen = new MWC.iOS.Screens.iPhone.Favorites.FavoritesScreen();
			this._favoritesScreen.TabBarItem = new UITabBarItem();
			this._favoritesScreen.TabBarItem.Title = "Favorites";

			// about tab
			this._aboutScreen = new Screens.Common.About.AboutXamScreen();
			this._aboutScreen.TabBarItem = new UITabBarItem();
			this._aboutScreen.TabBarItem.Title = "Xamarin";
			
			// create our array of controllers
			var viewControllers = new UIViewController[] {
				this._homeNav,
				this._speakerNav,
				this._sessionNav,
				this._mapScreen,
				this._exhibitorsScreen,
				this._twitterFeedScreen,
				this._newsFeedScreen,
				this._favoritesScreen,
				this._aboutScreen
			};
			
			// attach the view controllers
			this.ViewControllers = viewControllers;
			
			// tell the tab bar which controllers are allowed to customize. 
			// if we don't set this, it assumes all controllers are customizable. 
			// if we set this to empty array, NO controllers are customizable.
			CustomizableViewControllers = new UIViewController[] {};
			
			// set our selected item
			SelectedViewController = this._homeNav;
		}
	}
}