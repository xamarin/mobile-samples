using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using MonoTouch.Dialog.Utilities;
using Foundation;
using UIKit;
using MWC.BL;
using MWC.iOS.UI.Controls.Views;
using CoreGraphics;

namespace MWC.iOS.Screens.iPhone.Sessions {
	public class SessionDetailsScreen : UIViewController, ISessionViewHost {
		int sessionId;
		UIScrollView scrollView;
		SessionView sessionView;
		
		public bool ShouldShowSpeakers { get; set; }

		public SessionDetailsScreen (int sessionID)
		{
			ShouldShowSpeakers = true;	// by default

			sessionId = sessionID;

			View.BackgroundColor = UIColor.White;

			sessionView = new SessionView(this);
			sessionView.Frame = new CGRect(0,0,320,100);
			sessionView.AutoresizingMask = UIViewAutoresizing.FlexibleHeight;

			var scrollViewHeight = View.Frame.Height - 92; // update for 4"
			scrollView = new UIScrollView();
			scrollView.Frame = new CGRect(0,0,320,scrollViewHeight); // was hardcoded 370
			Add (scrollView);
		}
		
		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			sessionView.Update (sessionId, ShouldShowSpeakers);

			scrollView.Add(sessionView);
			scrollView.ContentOffset = new PointF(0,0);
			scrollView.ContentSize = sessionView.Bounds.Size.Height < 370 ? new CGSize(320,370) : sessionView.Bounds.Size;
		}

		public void SelectSpeaker(Speaker speaker)
		{
			var sds = new MWC.iOS.Screens.iPhone.Speakers.SpeakerDetailsScreen (speaker.ID);
			sds.ShouldShowSessions = false;
			sds.Title = "Speaker";
			NavigationController.PushViewController(sds, true);
		}
	}
}