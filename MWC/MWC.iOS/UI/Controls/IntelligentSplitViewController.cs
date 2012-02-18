//
// IntelligentSplitViewController.m
// From TexLege by Gregory S. Combs
//
// Released under the Creative Commons Attribution 3.0 Unported License
// Please see the included license page for more information.
// https://github.com/grgcombs/IntelligentSplitViewController/blob/master/CreativeCommonsLicense.html
//
// In a nutshell, you can use this, just attribute this to me in your "thank you" notes or about box.
//
using MonoTouch.UIKit;
using System.Drawing;
using System;
using MonoTouch.Foundation;
using MonoTouch.ObjCRuntime;
using MWC.iOS.Screens.iPhone.Speakers;

namespace MWC.iOS {
	public class IntelligentSplitViewController : UISplitViewController {

		NSObject ObserverWillRotate;
		NSObject ObserverDidRotate;

		public IntelligentSplitViewController ()
		{
			ObserverWillRotate = NSNotificationCenter.DefaultCenter.AddObserver(
					"UIApplicationWillChangeStatusBarOrientationNotification", OnWillRotate);			
			ObserverDidRotate = NSNotificationCenter.DefaultCenter.AddObserver(
					"UIApplicationDidChangeStatusBarOrientationNotification", OnDidRotate);			
		}
		~IntelligentSplitViewController () {
			NSNotificationCenter.DefaultCenter.RemoveObserver(ObserverWillRotate);
			NSNotificationCenter.DefaultCenter.RemoveObserver(ObserverDidRotate);
		}
		protected void OnWillRotate (NSNotification notification)
		{
			if (!IsViewLoaded) return;
			if (notification == null) return;

			var o1 = notification.UserInfo.ValueForKey(new NSString("UIApplicationStatusBarOrientationUserInfoKey"));
			int o2 = Convert.ToInt32(o1.ToString ());
			UIInterfaceOrientation toOrientation =(UIInterfaceOrientation) o2;
			var notModal = !(TabBarController.ModalViewController == null);
			var isSelectedTab = (TabBarController.SelectedViewController == this);

			//Console.WriteLine ("toOrientation:"+toOrientation);
			//Console.WriteLine ("isSelectedTab:"+isSelectedTab);

			var duration = UIApplication.SharedApplication.StatusBarOrientationAnimationDuration;

			if (!isSelectedTab || !notModal) {
				base.WillRotate (toOrientation, duration);
				
				UIViewController master = ViewControllers[0];
				var theDelegate = Delegate;
				
				//YOU_DONT_FEEL_QUEAZY_ABOUT_THIS_BECAUSE_IT_PASSES_THE_APP_STORE
				UIBarButtonItem button = base.ValueForKey (new NSString("_barButtonItem")) as UIBarButtonItem;
				
				
				if (toOrientation == UIInterfaceOrientation.Portrait
				|| toOrientation == UIInterfaceOrientation.PortraitUpsideDown) {
					if (theDelegate != null && theDelegate.RespondsToSelector(new Selector("splitViewController:willHideViewController:withBarButtonItem:forPopoverController:"))) {
						try {
							UIPopoverController popover = base.ValueForKey(new NSString("_hiddenPopoverController")) as UIPopoverController;
							theDelegate.WillHideViewController(this, master, button, popover);
						} catch (Exception e) {
							Console.WriteLine ("There was a nasty error while notifyng splitviewcontrollers of a portrait orientation change: " + e.Message);
						}
					}
		
				} else {
					if (theDelegate != null && theDelegate.RespondsToSelector(new Selector("splitViewController:willShowViewController:invalidatingBarButtonItem:"))) {
						try {
							theDelegate.WillShowViewController (this, master, button);
						} catch (Exception e) {
							Console.WriteLine ("There was a nasty error while notifyng splitviewcontrollers of a landscape orientation change: " + e.Message);
						}
					}
				}
	
			}
		}

		protected void OnDidRotate (NSNotification notification)
		{
			if (!IsViewLoaded) return;
			if (notification == null) return;

			var o1 = notification.UserInfo.ValueForKey(new NSString("UIApplicationStatusBarOrientationUserInfoKey"));
			int o2 = Convert.ToInt32(o1.ToString ());
			UIInterfaceOrientation toOrientation =(UIInterfaceOrientation) o2;
			var notModal = !(TabBarController.ModalViewController == null);
			var isSelectedTab = (TabBarController.SelectedViewController == this);
			if (!isSelectedTab || !notModal) {
				base.DidRotate(toOrientation);
			}
		}
	}
}

