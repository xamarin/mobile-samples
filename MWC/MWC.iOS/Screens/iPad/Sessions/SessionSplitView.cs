using MonoTouch.UIKit;
using System.Drawing;
using System;
using MonoTouch.Foundation;
using MWC.iOS.Screens.iPhone.Sessions;

namespace MWC.iOS.Screens.iPad.Sessions
{
	public class SessionSplitView : UISplitViewController
	{
		//SessionsScreen _sessionsList;
		MonoTouch.Dialog.DialogViewController _sessionsList;
		SessionSpeakersMasterDetail _sessionDetailsWithSpeakers;
		int _showingDay = -1;

		public SessionSplitView ()
		{
			Delegate = new SessionSplitViewDelegate();
			
			_sessionsList = new SessionsScreen(this);
			_sessionDetailsWithSpeakers = new SessionSpeakersMasterDetail(-1);
			
			this.ViewControllers = new UIViewController[]
				{_sessionsList, _sessionDetailsWithSpeakers};
		}
		
		/// <summary>
		/// On 'view will appear', if we were showing a particular day last time,
		/// we're going to revert to the entire schedule this time
		/// </summary>
		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			if (_showingDay > 0)
			{
				_showingDay = -1;
				_sessionsList = new SessionsScreen(this);
				this.ViewControllers = new UIViewController[]
					{_sessionsList, _sessionDetailsWithSpeakers};
			}
		}

		public void ShowSession (int sessionID)
		{
			_sessionDetailsWithSpeakers = this.ViewControllers[1] as SessionSpeakersMasterDetail;
			_sessionDetailsWithSpeakers.Update(sessionID);
		}
		public void ShowDay (int day)
		{	
			_showingDay = day;
			_sessionsList = new MWC.iOS.Screens.Common.Session.SessionDayScheduleScreen("", _showingDay, this);
			this.ViewControllers = new UIViewController[]
				{_sessionsList, _sessionDetailsWithSpeakers};
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