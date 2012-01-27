using MonoTouch.Dialog;
using MonoTouch.UIKit;

namespace MWC.iOS.Screens.Common
{
	public class TabBarController : UITabBarController
	{
		UINavigationController _homeNav = null, _speakerNav = null, _sessionNav = null
								, _exhibitorsNav = null, _twitterNav = null, _newsNav = null;
		UIViewController _homeScreen = null;
		DialogViewController _speakersScreen;
		DialogViewController _sessionsScreen;
		DialogViewController _twitterFeedScreen;
		DialogViewController _newsFeedScreen;
		DialogViewController _exhibitorsScreen;
		DialogViewController _favoritesScreen;
		Screens.Common.Map.MapController _mapScreen;
		Screens.Common.About.AboutXamScreen _aboutScreen;

		UISplitViewController _speakersSplitView, _exhibitorsSplitView, _twitterSplitView, _newsSplitView;
		
		public TabBarController ()
		{
		}
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			// home tab
			if (AppDelegate.IsPhone){
				_homeScreen = new Screens.iPhone.Home.HomeScreen();
				_homeScreen.Title = "Schedule";
			}
			else
			{
				_homeScreen = new Screens.iPhone.Home.HomeScreen();
			}
			this._homeNav = new UINavigationController();
			this._homeNav.PushViewController ( this._homeScreen, false );			
			this._homeNav.Title = "Schedule";
			this._homeNav.TabBarItem = new UITabBarItem("Schedule"
										, UIImage.FromBundle("Images/Tabs/schedule.png"), 0);
			

			// speakers tab
			if (AppDelegate.IsPhone){
				this._speakersScreen = new Screens.iPhone.Speakers.SpeakersScreen();			
				this._speakerNav = new UINavigationController();
				this._speakerNav.TabBarItem = new UITabBarItem("Speakers"
											, UIImage.FromBundle("Images/Tabs/speakers.png"), 1);
				this._speakerNav.PushViewController ( this._speakersScreen, false );
			}
			else
			{
				this._speakersSplitView = new MWC.iOS.Screens.iPad.Speakers.SpeakerSplitView();
				this._speakersSplitView.TabBarItem = new UITabBarItem("Speakers"
											, UIImage.FromBundle("Images/Tabs/speakers.png"), 1);
			}

			// sessions
			this._sessionsScreen = new Screens.iPhone.Sessions.SessionsScreen();
			this._sessionNav = new UINavigationController();
			this._sessionNav.TabBarItem = new UITabBarItem("Sessions"
										, UIImage.FromBundle("Images/Tabs/sessions.png"), 2);
			this._sessionNav.PushViewController ( this._sessionsScreen, false );
			
			// maps tab
			//TODO: pass in the actual frame (minus tab bar, status bar crap)
			this._mapScreen = new Screens.Common.Map.MapController(UIScreen.MainScreen.Bounds);
			this._mapScreen.TabBarItem = new UITabBarItem("Map"
										, UIImage.FromBundle("Images/Tabs/maps.png"), 3);
			
			if (AppDelegate.IsPhone)
			{
				// exhibitors
				this._exhibitorsScreen = new Screens.iPhone.Exhibitors.ExhibitorsScreen();
				this._exhibitorsScreen.TabBarItem = new UITabBarItem("Exhibitors"
											, UIImage.FromBundle("Images/Tabs/exhibitors.png"), 4);
				
				// twitter feed
				this._twitterFeedScreen = new MWC.iOS.Screens.iPhone.Twitter.TwitterScreen();
				this._twitterFeedScreen.TabBarItem = new UITabBarItem("Twitter"
											, UIImage.FromBundle("Images/Tabs/twitter.png"), 5);
				
				// news
				this._newsFeedScreen = new MWC.iOS.Screens.Common.News.NewsScreen();
				this._newsFeedScreen.TabBarItem =  new UITabBarItem("News"
											, UIImage.FromBundle("Images/Tabs/rss.png"), 6);
			}
			else 
			{	// iPad
				// exhibitors
				
				this._exhibitorsSplitView = new Screens.iPad.Exhibitors.ExhibitorSplitView();
				this._exhibitorsSplitView.TabBarItem = new UITabBarItem("Exhibitors"
											, UIImage.FromBundle("Images/Tabs/exhibitors.png"), 4);
				
				// twitter feed
				this._twitterSplitView = new Screens.iPad.Twitter.TwitterSplitView();
				this._twitterSplitView.TabBarItem = new UITabBarItem("Twitter"
											, UIImage.FromBundle("Images/Tabs/twitter.png"), 5);

				// news
				this._newsSplitView = new MWC.iOS.Screens.iPad.News.NewsSplitView();
				this._newsSplitView.TabBarItem =  new UITabBarItem("News"
											, UIImage.FromBundle("Images/Tabs/rss.png"), 6);
			}
			// favorites
			this._favoritesScreen = new MWC.iOS.Screens.iPhone.Favorites.FavoritesScreen();
			this._favoritesScreen.TabBarItem =  new UITabBarItem("Favorites"
										, UIImage.FromBundle("Images/Tabs/favorites.png"), 6);

			// about tab
			this._aboutScreen = new Screens.Common.About.AboutXamScreen();
			this._aboutScreen.TabBarItem = new UITabBarItem("About Xamarin"
										, UIImage.FromBundle("Images/Tabs/about.png"), 8);
			
			// create our array of controllers
			var viewControllers = new UIViewController[] {
				this._homeNav,
				(AppDelegate.IsPhone?(UIViewController)this._speakerNav:(UIViewController)this._speakersSplitView),
				this._sessionNav,
				this._mapScreen,
				(AppDelegate.IsPhone?(UIViewController)this._exhibitorsScreen:(UIViewController)this._exhibitorsSplitView),
				(AppDelegate.IsPhone?(UIViewController)this._twitterFeedScreen:(UIViewController)this._twitterSplitView),
				(AppDelegate.IsPhone?(UIViewController)this._newsFeedScreen:(UIViewController)this._newsSplitView),
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

		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
        {
            return true;
        }
	}
}