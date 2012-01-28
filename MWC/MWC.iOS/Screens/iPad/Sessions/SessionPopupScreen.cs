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
		UIButton _button;

		public SessionPopupScreen (Session session)
		{
			_session = session;
		}
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			_sessionView = new SessionView(_session);
			_sessionView.Frame = new System.Drawing.RectangleF(0,0,540,620);
			
			_button = UIButton.FromType (UIButtonType.RoundedRect);
			_button.Frame = new RectangleF(440,10,90,30);
			_button.SetTitle ("Close", UIControlState.Normal);
			_button.TouchUpInside += delegate {
				this.DismissViewController (true, () => {});
			};
			
			this.View.AddSubview (_sessionView);
			this.View.AddSubview (_button);
		}
		
		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
        {
            return true;
        }
	}
}