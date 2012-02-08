using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using MonoTouch.UIKit;
using MWC.iOS.UI.Controls.Views;

namespace MWC.iOS.Screens.iPad.Sessions {
	public class SessionSpeakersMasterDetail : UIViewController, ISessionViewHost {

		UINavigationBar navBar;
		int sessionId;
		List<MWC.BL.Speaker> speakersInSession;
		SessionView sessionView;
		SpeakerView speakerView;

		int colWidth1 = 335;
		int colWidth2 = 433;
	
		public UIPopoverController Popover;

		public SessionSpeakersMasterDetail (int sessionID)
		{
			sessionId = sessionID;
			
			navBar = new UINavigationBar(new RectangleF(0,0,768, 44));
			navBar.SetItems(new UINavigationItem[]{new UINavigationItem("Session & Speaker Info")},false);
			
			View.BackgroundColor = UIColor.LightGray;
			View.Frame = new RectangleF(0,0,768,768);

			sessionView = new SessionView(this);
			sessionView.Frame = new RectangleF(0,44,colWidth1,728);
			sessionView.AutoresizingMask = UIViewAutoresizing.FlexibleHeight;

			speakerView = new SpeakerView(-1);
			speakerView.Frame = new RectangleF(colWidth1+1,44,colWidth2,728);
			speakerView.AutoresizingMask = UIViewAutoresizing.FlexibleHeight | UIViewAutoresizing.FlexibleWidth;

			View.AddSubview (speakerView);
			View.AddSubview (sessionView);
			View.AddSubview (navBar);
		}

		public void SelectSpeaker(int sessionID) 
		{
			sessionId = sessionID;
			
			if (sessionId > 1) {
				sessionView.Update (sessionId, true);
				
				var session = BL.Managers.SessionManager.GetSession (sessionId);

				speakersInSession = session.Speakers;
				if (speakersInSession != null && speakersInSession.Count > 0) {
					speakerView.Update (speakersInSession[0].ID);
				} else {	// no speaker (!?)
					speakerView.Clear();
				}
			}
			
			if (Popover != null) {
				Popover.Dismiss (true);
			}
		}

		public void SelectSpeaker (BL.Speaker speaker) {
			speakerView.Update (speaker.ID);
		}

		public void AddNavBarButton (UIBarButtonItem button)
		{
			button.Title = "Sessions";
			navBar.TopItem.SetLeftBarButtonItem (button, false);
		}
		
		public void RemoveNavBarButton ()
		{
			navBar.TopItem.SetLeftBarButtonItem (null, false);
		}
	}
}