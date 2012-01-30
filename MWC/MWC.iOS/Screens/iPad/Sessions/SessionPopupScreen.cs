using System;
using MonoTouch.UIKit;
using MWC.BL;
using MWC.iOS.UI.Controls.Views;
using System.Drawing;

namespace MWC.iOS.Screens.iPad
{
	/// <summary>
	/// Session popup for use on the HomeScreen (iPad only).
	/// </summary>
	public class SessionPopupScreen : UIViewController
	{
		Session _session;
		SessionView _sessionView;
		
		public SessionPopupScreen (Session session)
		{
			_session = session;
		}
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			_sessionView = new SessionView(_session, this);
			_sessionView.Frame = new System.Drawing.RectangleF(0,0,540,620);
			
			this.View.AddSubview (_sessionView);
		}
		
		public void Dismiss()
		{
			this.DismissViewController (true, () => {});
		}

		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
        {	// only ever used on iPad, so we don't check 
            return true;
        }
	}
}