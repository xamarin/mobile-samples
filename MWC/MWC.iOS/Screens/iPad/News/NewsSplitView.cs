using System;
using System.Drawing;
using Foundation;
using UIKit;
using MWC.iOS.Screens.Common.News;
using CoreGraphics;

namespace MWC.iOS.Screens.iPad.News
{
	public class NewsSplitView : UISplitViewController
	{
		NewsScreen newsList;
		
		NewsDetailsScreen newsDetailView;
		
		public NewsSplitView ()
		{
			View.Bounds = new CGRect(0,0,UIScreen.MainScreen.Bounds.Width,UIScreen.MainScreen.Bounds.Height);
			Delegate = new SplitViewDelegate();
			
			newsList = new NewsScreen(this);
			
			newsDetailView = new NewsDetailsScreen(null);
			
			this.ViewControllers = new UIViewController[]
				{newsList, newsDetailView};
		}
		
		public void ShowNews (int newsID, UIViewController newsView)
		{
			this.ViewControllers = new UIViewController[]
				{newsList, newsView};

		}

		[Obsolete]
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