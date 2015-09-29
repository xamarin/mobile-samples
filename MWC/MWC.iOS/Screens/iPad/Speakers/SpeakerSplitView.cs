using UIKit;
using System.Drawing;
using System;
using Foundation;
using ObjCRuntime;
using MWC.iOS.Screens.iPhone.Speakers;

namespace MWC.iOS.Screens.iPad.Speakers {
	public class SpeakerSplitView : IntelligentSplitViewController {
		SpeakersScreen speakersList;
		SpeakerSessionMasterDetail speakerDetailWithSession;
		
		public SpeakerSplitView ()
		{
			Delegate = new SpeakerSplitViewDelegate();
			
			speakersList = new SpeakersScreen(this);
			speakerDetailWithSession = new SpeakerSessionMasterDetail(-1);
			
			ViewControllers = new UIViewController[]
				{speakersList, speakerDetailWithSession};
		}

		public void ShowSpeaker (int speakerID)
		{
			speakerDetailWithSession = this.ViewControllers[1] as SpeakerSessionMasterDetail;
			speakerDetailWithSession.Update(speakerID);
		}

		[Obsolete]
		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
        {
            return true;
        }
	}

 	public class SpeakerSplitViewDelegate : UISplitViewControllerDelegate
    {
		public override bool ShouldHideViewController (UISplitViewController svc, UIViewController viewController, UIInterfaceOrientation inOrientation)
		{
			return inOrientation == UIInterfaceOrientation.Portrait
				|| inOrientation == UIInterfaceOrientation.PortraitUpsideDown;
		}

		public override void WillHideViewController (UISplitViewController svc, UIViewController aViewController, UIBarButtonItem barButtonItem, UIPopoverController pc)
		{
			SpeakerSessionMasterDetail dvc = svc.ViewControllers[1] as SpeakerSessionMasterDetail;
			
			if (dvc != null) {
				dvc.AddNavBarButton (barButtonItem);
				dvc.Popover = pc;
			}
		}
		
		public override void WillShowViewController (UISplitViewController svc, UIViewController aViewController, UIBarButtonItem button)
		{
			SpeakerSessionMasterDetail dvc = svc.ViewControllers[1] as SpeakerSessionMasterDetail;
			
			if (dvc != null) {
				dvc.RemoveNavBarButton ();
				dvc.Popover = null;
			}
		}
	}
}