using MonoTouch.UIKit;
using System.Drawing;
using System;
using System.Linq;
using System.Collections.Generic;
using MonoTouch.Foundation;
using MWC.iOS.Screens.Common;
using MWC.iOS.Screens.Common.Session;
using MWC.iOS.UI.Controls.Views;

namespace MWC.iOS.Screens.iPad.Sessions
{
	public class SessionSpeakersMasterDetail : UIViewController
	{
		UINavigationBar _navBar;

		int _sessionID;
		List<MWC.BL.Speaker>  _speakersInSession;
		SessionView _sessionView;
		SpeakerView _speakerView;

		int colWidth1 = 335;
		int colWidth2 = 433;
	
		public UIPopoverController Popover;

		public SessionSpeakersMasterDetail (int sessionID)
		{
			_sessionID = sessionID;
			
			_navBar = new UINavigationBar(new RectangleF(0,0,768, 44));
			_navBar.SetItems(new UINavigationItem[]{new UINavigationItem("Session & Speaker Info")},false);
			
			this.View.BackgroundColor = UIColor.LightGray;
			this.View.Frame = new RectangleF(0,0,768,768);

			_sessionView = new SessionView(null);
			_sessionView.Frame = new RectangleF(0,44,colWidth1,728);
			_sessionView.AutoresizingMask = UIViewAutoresizing.FlexibleHeight;

			_speakerView = new SpeakerView(-1);
			_speakerView.Frame = new RectangleF(colWidth1+1,44,colWidth2,728);
			_speakerView.AutoresizingMask = UIViewAutoresizing.FlexibleHeight | UIViewAutoresizing.FlexibleWidth;

			this.View.AddSubview (_speakerView);
			this.View.AddSubview (_sessionView);
			this.View.AddSubview (_navBar);
		}

		public void Update(int sessionID) 
		{
			_sessionID = sessionID;
			_sessionView.Update (_sessionID);

			if (_sessionID > 1)
			{
				var session = BL.Managers.SessionManager.GetSession (_sessionID);
				var _speakers = BL.Managers.SpeakerManager.GetSpeakers ();
				if (_speakers != null) 
				{	
					_speakersInSession = (from speaker in _speakers
							where session.SpeakerNames.IndexOf(speaker.Name) >= 0
							select speaker).ToList();
				}
				if (_speakersInSession != null && _speakersInSession.Count > 0) 
				{
					_speakerView.Update(_speakersInSession[0].ID);
				}
			}
			
			if (Popover != null)
			{
				Popover.Dismiss (true);
			}
		}
		public void AddNavBarButton (UIBarButtonItem button)
		{
			button.Title = "Sessions";
			_navBar.TopItem.SetLeftBarButtonItem (button, false);
		}
		
		public void RemoveNavBarButton ()
		{
			_navBar.TopItem.SetLeftBarButtonItem (null, false);
		}
	}
}