using System;
using System.Drawing;
using UIKit;
using MWC.iOS.Screens.iPhone.Exhibitors;

namespace MWC.iOS.Screens.iPad.Exhibitors
{
	public class ExhibitorSplitView : UISplitViewController
	{
		ExhibitorsScreen _exhibitorsList;
		ExhibitorDetailsScreen _exhibitorDetails;
		
		public ExhibitorSplitView ()
		{
			//View.Bounds = new CGRect(0,0,UIScreen.MainScreen.Bounds.Width,UIScreen.MainScreen.Bounds.Height);
			Delegate = new SplitViewDelegate();
			
			_exhibitorsList = new ExhibitorsScreen(this);
			_exhibitorDetails = new ExhibitorDetailsScreen(-1);
			
			this.ViewControllers = new UIViewController[]
				{_exhibitorsList, _exhibitorDetails};
		}
		
		public void ShowExhibitor (int exhibitorID, UIViewController exhibitorView)
		{
			this.ViewControllers = new UIViewController[]
				{_exhibitorsList, exhibitorView};

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