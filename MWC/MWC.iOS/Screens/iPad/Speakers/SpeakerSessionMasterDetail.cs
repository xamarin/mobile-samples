using MonoTouch.UIKit;
using System.Drawing;
using System;
using System.Linq;
using MonoTouch.Foundation;
using MWC.iOS.Screens.Common;
using MWC.iOS.Screens.Common.Session;
using MWC.iOS.UI.Controls.Views;

namespace MWC.iOS.Screens.iPad.Speakers
{
	public class SpeakerSessionMasterDetail : UIViewController
	{
		UINavigationBar _navBar;
		UIViewController _speakerDetailsScreen;

		int _speakerID, _sessionID;
		MWC.BL.Session _session;
		SessionView _ssv;
		SpeakerView _sv;

		int colWidth1 = 335;
		int colWidth2 = 368;
	
		public UIPopoverController Popover;

		public SpeakerSessionMasterDetail (int speakerID) //, UIViewController speakerView)
		{
			_speakerID = speakerID;
			
			_navBar = new UINavigationBar(new RectangleF(0,0,768, 44));
			_navBar.SetItems(new UINavigationItem[]{new UINavigationItem("Speaker & Session Info")},false);
			
			this.View.BackgroundColor = UIColor.LightGray;
			this.View.Frame = new RectangleF(0,0,768,768);

			_sv = new SpeakerView(-1);
			_sv.Frame = new RectangleF(0,44,colWidth1,728);
			
			_ssv = new SessionView(null);
			_ssv.Frame = new RectangleF(colWidth1+1,44,colWidth2,728);
			
			this.View.AddSubview (_sv);
			this.View.AddSubview (_ssv);
			this.View.AddSubview (_navBar);

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
//				_ssv = new SessionView(_session);
//				_ssv.Frame = new RectangleF(colWidth1+1,0,colWidth2,728);
//
//				this.View.AddSubview (_ssv);
//			}
		}

		public void Update(int speakerID) //, UIViewController view)
		{
			_speakerID = speakerID;
			_sv.Update (speakerID);

			if (_speakerID > 1)
			{
				var speaker = BL.Managers.SpeakerManager.GetSpeaker (_speakerID);
				var _sessions = BL.Managers.SessionManager.GetSessions ();
				if (_sessions != null) 
				{	
					_session = (from session in _sessions
							where session.SpeakerNames.IndexOf(speaker.Name) >= 0
							select session).FirstOrDefault();
				}
				_ssv.Update(_session);
			}
			_sv.SetNeedsDisplay();
			

			if (Popover != null)
			{
				Popover.Dismiss (true);
			}
		}
		public void AddNavBarButton (UIBarButtonItem button)
		{
			button.Title = "Speakers";
			_navBar.TopItem.SetLeftBarButtonItem (button, false);
		}
		
		public void RemoveNavBarButton ()
		{
			_navBar.TopItem.SetLeftBarButtonItem (null, false);
		}
	}
}