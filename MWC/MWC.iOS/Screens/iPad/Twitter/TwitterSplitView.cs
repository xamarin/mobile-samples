using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MWC.iOS.Screens.iPhone.Twitter;

namespace MWC.iOS.Screens.iPad.Twitter
{
	public class TwitterSplitView : UISplitViewController
	{
		TwitterScreen _twitterScreen;
		
		TweetDetailsScreen _tds;
		
		public TwitterSplitView ()
		{
			View.Bounds = new RectangleF(0,0,UIScreen.MainScreen.Bounds.Width,UIScreen.MainScreen.Bounds.Height);
			Delegate = new SplitViewDelegate();
			
			_twitterScreen = new TwitterScreen(this);
			
			_tds = new TweetDetailsScreen(null);
			
			this.ViewControllers = new UIViewController[]
				{_twitterScreen, _tds};
		}
		
		public void ShowTweet (int tweetID, UIViewController tweetView)
		{
			this.ViewControllers = new UIViewController[]
				{_twitterScreen, tweetView};

		}
		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
        {
            return true;
        }
	}

 	public class SplitViewDelegate : UISplitViewControllerDelegate
    {
		// http://useyourloaf.com/blog/2011/10/19/ios-5-split-view-controller-changes.html
		[Export("splitViewController:shouldHideViewController:inOrientation:")]
		public bool ShouldHideViewController (UISplitViewController svc, UIViewController viewController, UIInterfaceOrientation inOrientation)
		{
			return false;
		}
	}
}