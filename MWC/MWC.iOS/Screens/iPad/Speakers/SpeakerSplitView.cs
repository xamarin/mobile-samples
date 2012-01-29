using MonoTouch.UIKit;
using System.Drawing;
using System;
using MonoTouch.Foundation;
using MWC.iOS.Screens.iPhone.Speakers;

namespace MWC.iOS.Screens.iPad.Speakers
{
	public class SpeakerSplitView : UISplitViewController
	{
		SpeakersScreen _speakersScreen;
		
		SpeakerSessionMasterDetail _ssmd;
		
		public SpeakerSplitView ()
		{
			View.Bounds = new RectangleF(0,0,UIScreen.MainScreen.Bounds.Width,UIScreen.MainScreen.Bounds.Height);
			Delegate = new SplitViewDelegate();
			
			_speakersScreen = new SpeakersScreen(this);
			
			_ssmd = new SpeakerSessionMasterDetail(-1);//, _speakersScreen);
			
			this.ViewControllers = new UIViewController[]
				{_speakersScreen, _ssmd};
		}
		
		public void ShowSpeaker (int speakerID) //, UIViewController speakerView)
		{
//			_ssmd = new SpeakerSessionMasterDetail(speakerID, speakerView);
//			this.ViewControllers = new UIViewController[]
//				{_speakersScreen, _ssmd};
			_ssmd = this.ViewControllers[1] as SpeakerSessionMasterDetail;
			_ssmd.Update(speakerID);
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