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
			
			_ssmd = new SpeakerSessionMasterDetail(0, _speakersScreen);
			
			this.ViewControllers = new UIViewController[]
				{_speakersScreen, _ssmd};
		}
		
		public void ShowSpeaker (int speakerID, UIViewController speakerView)
		{
			_ssmd = new SpeakerSessionMasterDetail(speakerID, speakerView);
			this.ViewControllers = new UIViewController[]
				{_speakersScreen, _ssmd};

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