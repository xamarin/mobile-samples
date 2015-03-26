using UIKit;
using System.Drawing;
using System;
using Foundation;
using MWC.iOS.Screens.iPhone.Sessions;
using MonoTouch.Dialog;

namespace MWC.iOS.Screens.iPad.Sessions {
	public class SessionSplitView : IntelligentSplitViewController {
		DialogViewController sessionsList;
		SessionSpeakersMasterDetail sessionDetailsWithSpeakers;
		//int day = -1;
		bool showingDay = false;

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
			ConsoleD.WriteLine ("viewappear showingDay = " + showingDay);
			if (!showingDay) {
				var sl = ViewControllers[0] as SessionsScreen;
				sl.ShowAll();
				sessionsList = sl;
			}
			showingDay = false;
		}
		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);
		}

		public void ShowSession (int sessionID)
		{
			sessionDetailsWithSpeakers = this.ViewControllers[1] as SessionSpeakersMasterDetail;
			sessionDetailsWithSpeakers.SelectSpeaker(sessionID);
		}
		public void ShowDay (int day)
		{	
			//this.day = day;
			showingDay = true;
			var sl = ViewControllers[0] as SessionsScreen;
			sl.FitlerByDay(day);
			sessionsList = sl;

			sessionDetailsWithSpeakers = this.ViewControllers[1] as SessionSpeakersMasterDetail;
			sessionDetailsWithSpeakers.SelectSpeaker(-1); // blank out for a new day

		}

		[Obsolete]
		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
        {
            return true;
        }
		public bool IsPortrait {
			get {
				return InterfaceOrientation == UIInterfaceOrientation.Portrait 
					|| InterfaceOrientation == UIInterfaceOrientation.PortraitUpsideDown;
			}
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
			} else ConsoleD.WriteLine ("SessionSplitViewController dvc == null (hide)");
		}
		
		public override void WillShowViewController (UISplitViewController svc, UIViewController aViewController, UIBarButtonItem button)
		{
			SessionSpeakersMasterDetail dvc = svc.ViewControllers[1] as SessionSpeakersMasterDetail;
			
			if (dvc != null) {
				dvc.RemoveNavBarButton ();
				dvc.Popover = null;
			} else ConsoleD.WriteLine ("SessionSplitViewController dvc == null (show)");
		}
	}
}