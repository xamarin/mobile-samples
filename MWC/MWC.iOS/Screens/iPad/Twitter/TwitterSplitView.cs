using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MWC.iOS.Screens.iPhone.Twitter;

namespace MWC.iOS.Screens.iPad.Twitter {
	public class TwitterSplitView : UISplitViewController {
		TwitterScreen twitterList;
		
		TweetDetailsScreen tweetScreen;
		
		public TwitterSplitView ()
		{
			View.Bounds = new RectangleF(0,0,UIScreen.MainScreen.Bounds.Width,UIScreen.MainScreen.Bounds.Height);
			Delegate = new SplitViewDelegate();
			
			twitterList = new TwitterScreen(this);
			
			tweetScreen = new TweetDetailsScreen(null);
			
			ViewControllers = new UIViewController[]
				{twitterList, tweetScreen};
		}
		
		public void ShowTweet (int tweetID, UIViewController tweetView)
		{
			ViewControllers = new UIViewController[]
				{twitterList, tweetView};

		}
		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
        {
            return true;
        }
	}

 	public class SplitViewDelegate : UISplitViewControllerDelegate
    {
		public override bool ShouldHideViewController (UISplitViewController svc, UIViewController viewController, UIInterfaceOrientation inOrientation)
		{
			return false;
		}
	}
}