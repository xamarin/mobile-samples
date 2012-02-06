using MonoTouch.UIKit;
using System.Drawing;
using System;
using MonoTouch.Foundation;
using MWC.iOS.Screens.iPhone.Sessions;

namespace MWC.iOS.Screens.iPad.Sessions {
	public class SessionSplitView : UISplitViewController {
		MonoTouch.Dialog.DialogViewController sessionsList;
		SessionSpeakersMasterDetail sessionDetailsWithSpeakers;
		int showingDay = -1;

		public SessionSplitView ()
		{
			Delegate = new SessionSplitViewDelegate();
			
			sessionsList = new SessionsScreen(this);
			sessionDetailsWithSpeakers = new SessionSpeakersMasterDetail(-1);
			
			this.ViewControllers = new UIViewController[]
				{sessionsList, sessionDetailsWithSpeakers};
		}
		
		/// <summary>
		/// On 'view will appear', if we were showing a particular day last time,
		/// we're going to revert to the entire schedule this time
		/// </summary>
		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			if (showingDay > 0) {
				showingDay = -1;
				sessionsList = new SessionsScreen(this);
				this.ViewControllers = new UIViewController[]
					{sessionsList, sessionDetailsWithSpeakers};
			}
		}

		public void ShowSession (int sessionID)
		{
			sessionDetailsWithSpeakers = this.ViewControllers[1] as SessionSpeakersMasterDetail;
			sessionDetailsWithSpeakers.Update(sessionID);
		}
		public void ShowDay (int day)
		{	
			showingDay = day;
			sessionsList = new MWC.iOS.Screens.Common.Session.SessionDayScheduleScreen("", showingDay, this);
			this.ViewControllers = new UIViewController[]
				{sessionsList, sessionDetailsWithSpeakers};
		}
		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
        {
            return true;
        }
	}

 	public class SessionSplitViewDelegate : UISplitViewControllerDelegate
    {
		public override bool ShouldHideViewController (UISplitViewController svc, UIViewController viewController, UIInterfaceOrientation inOrientation)
		{
			return inOrientation == UIInterfaceOrientation.Portrait
				|| inOrientation == UIInterfaceOrientation.PortraitUpsideDown;
		}

		public override void WillHideViewController (UISplitViewController svc, UIViewController aViewController, UIBarButtonItem barButtonItem, UIPopoverController pc)
		{
			SessionSpeakersMasterDetail dvc = svc.ViewControllers[1] as SessionSpeakersMasterDetail;
			
			if (dvc != null) {
				dvc.AddNavBarButton (barButtonItem);
				dvc.Popover = pc;
			}
		}
		
		public override void WillShowViewController (UISplitViewController svc, UIViewController aViewController, UIBarButtonItem button)
		{
			SessionSpeakersMasterDetail dvc = svc.ViewControllers[1] as SessionSpeakersMasterDetail;
			
			if (dvc != null) {
				dvc.RemoveNavBarButton ();
				dvc.Popover = null;
			}
		}
	}
}