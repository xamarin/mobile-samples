using MonoTouch.UIKit;
using System.Drawing;
using System;
using MonoTouch.Foundation;
using MWC.iOS.Screens.iPhone.Exhibitors;

namespace MWC.iOS.Screens.iPad.Exhibitors
{
	public class ExhibitorSplitView : UISplitViewController
	{
		ExhibitorsScreen _exhibitorsScreen;
		
		ExhibitorDetailsScreen _esd;
		
		public ExhibitorSplitView ()
		{
			View.Bounds = new RectangleF(0,0,UIScreen.MainScreen.Bounds.Width,UIScreen.MainScreen.Bounds.Height);
			Delegate = new SplitViewDelegate();
			
			_exhibitorsScreen = new ExhibitorsScreen(this);
			
			_esd = new ExhibitorDetailsScreen(-1);
			
			this.ViewControllers = new UIViewController[]
				{_exhibitorsScreen, _esd};
		}
		
		public void ShowExhibitor (int exhibitorID, UIViewController exhibitorView)
		{
			//_esd = new ExhibitorDetailsScreen(exhibitorID);
			this.ViewControllers = new UIViewController[]
				{_exhibitorsScreen, exhibitorView};

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