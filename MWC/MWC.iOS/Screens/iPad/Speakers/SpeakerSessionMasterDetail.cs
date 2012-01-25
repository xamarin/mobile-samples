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
		UIViewController _speakerDetailsScreen;
		
		int _speakerID, _sessionID;
		MWC.BL.Session _session;

		SpeakerView _sv;
		int colWidth1 = 335;
		int colWidth2 = 368;
	
		public SpeakerSessionMasterDetail (int speakerID, UIViewController speakerView)
		{
			_speakerID = speakerID;

			this.View.BackgroundColor = UIColor.LightGray;

			this.View.Frame = new RectangleF(0,0,768,768);

			SpeakerView _sv = new SpeakerView(speakerID);
			_sv.Frame = new RectangleF(0,0,colWidth1,728);
			
			
			this.View.AddSubview (_sv);
	
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

				SessionView _ssv = new SessionView(_session);
				_ssv.Frame = new RectangleF(colWidth1+1,0,colWidth2,728);

				this.View.AddSubview (_ssv);
			}
		}
	}
}