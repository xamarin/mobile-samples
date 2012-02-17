using MonoTouch.UIKit;
using System.Drawing;
using System;
using System.Linq;
using MonoTouch.Foundation;
using MWC.iOS.Screens.Common;
using MWC.iOS.Screens.Common.Session;
using MWC.iOS.UI.Controls.Views;

namespace MWC.iOS.Screens.iPad.Speakers {
	public class SpeakerSessionMasterDetail : UIViewController {
		UINavigationBar navBar;

		int speakerId;
		MWC.BL.Session session;
		SessionView sessionView;
		SpeakerView speakerView;

		int colWidth1 = 335;
		int colWidth2 = 433;
	
		public UIPopoverController Popover;

		public SpeakerSessionMasterDetail (int speakerID) //, UIViewController speakerView)
		{
			speakerId = speakerID;
			
			navBar = new UINavigationBar(new RectangleF(0,0,768, 44));
			navBar.SetItems(new UINavigationItem[]{new UINavigationItem("Speaker & Session Info")},false);
			
			View.BackgroundColor = UIColor.LightGray;
			View.Frame = new RectangleF(0,0,768,768);

			speakerView = new SpeakerView(-1);
			speakerView.Frame = new RectangleF(0,44,colWidth1,728);
			speakerView.AutoresizingMask = UIViewAutoresizing.FlexibleHeight;

			sessionView = new SessionView(false);
			sessionView.Frame = new RectangleF(colWidth1+1,44,colWidth2,728);
			sessionView.AutoresizingMask = UIViewAutoresizing.FlexibleHeight | UIViewAutoresizing.FlexibleWidth;

			View.AddSubview (speakerView);
			View.AddSubview (sessionView);
			View.AddSubview (navBar);

//			if (_speakerID > 1)
//			{
//				var speaker = BL.Managers.SpeakerManager.GetSpeaker (_speakerID);
//				var _sessions = BL.Managers.SessionManager.GetSessions ();
//				if (_sessions != null) 
//				{	
//					_session = (from session in _sessions
//							where session.SpeakerNames.IndexOf(speaker.Name) >= 0
//							select session).FirstOrDefault();
//				}
//
//				_sessionView = new SessionView(_session);
//				_ssv.Frame = new RectangleF(colWidth1+1,0,colWidth2,728);
//
//				this.View.AddSubview (_ssv);
//			}
		}

		public void Update(int speakerID) //, UIViewController view)
		{
			speakerId = speakerID;
			speakerView.Update (speakerID);

			if (speakerId > 1) {
				var speaker = BL.Managers.SpeakerManager.GetSpeaker (speakerId);
				var sessions = BL.Managers.SessionManager.GetSessions ();
				if (sessions != null) {	
					session = (from s in sessions
							where s.SpeakerNames.IndexOf(speaker.Name) >= 0
							select s).FirstOrDefault();
				}
				sessionView.Update(session);
			}
			speakerView.SetNeedsDisplay();
			

			if (Popover != null) {
				Popover.Dismiss (true);
			}
		}
		
		/// <summary>
		/// Keep favorite-stars in sync with changes made on other screens
		/// </summary>
		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			sessionView.UpdateFavorite ();		
		}
		
		public void AddNavBarButton (UIBarButtonItem button)
		{
			button.Title = "Speakers";
			navBar.TopItem.SetLeftBarButtonItem (button, false);
		}
		
		public void RemoveNavBarButton ()
		{
			navBar.TopItem.SetLeftBarButtonItem (null, false);
		}
	}
}