using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MWC.iOS.Screens.Common.News;

namespace MWC.iOS.Screens.iPad.News
{
	public class NewsSplitView : UISplitViewController
	{
		NewsScreen _newsScreen;
		
		NewsDetailsScreen _nds;
		
		public NewsSplitView ()
		{
			View.Bounds = new RectangleF(0,0,UIScreen.MainScreen.Bounds.Width,UIScreen.MainScreen.Bounds.Height);
			Delegate = new SplitViewDelegate();
			
			_newsScreen = new NewsScreen(this);
			
			_nds = new NewsDetailsScreen("","");
			
			this.ViewControllers = new UIViewController[]
				{_newsScreen, _nds};
		}
		
		public void ShowNews (int newsID, UIViewController newsView)
		{
			this.ViewControllers = new UIViewController[]
				{_newsScreen, newsView};

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