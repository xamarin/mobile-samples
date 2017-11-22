using System;

using UIKit;
using CoreGraphics;

namespace BluetoothLEExplorer.iOS
{
	public class ScanButton : UIButton
	{
		public enum ScanButtonState
		{
			Normal,
			Scanning
		}

		UILabel title;
		UIActivityIndicatorView activity;
		ScanButtonState state = ScanButtonState.Normal;
		CGRect initialFrame = new CGRect (0, 0, 65, 40);

		public ScanButton ()
		{
			Frame = initialFrame;
			TouchUpInside += (sender, e) => ChangeState ();
			Initialize ();
		}

		void Initialize()
		{
			title = new UILabel {
				Frame = initialFrame,
				Text = "Scan",
				Font = UIFont.SystemFontOfSize (UIFont.ButtonFontSize),
				TextColor = UIColor.Blue,
			};
			Add (title);

			activity = new UIActivityIndicatorView (UIActivityIndicatorViewStyle.Gray) {
				Frame = new CGRect (35, 0, 40, 40),
				HidesWhenStopped = true,
			};
			Add (activity);
		}

		void ChangeState()
		{
			if (state == ScanButtonState.Normal)
				SetState (ScanButtonState.Scanning);
			else
				SetState (ScanButtonState.Normal);
		}

		public void SetState(ScanButtonState state)
		{
			if (state == ScanButtonState.Normal) {
				state = ScanButtonState.Scanning;
				title.Text = "Stop";
				activity.Hidden = false;
				activity.StartAnimating ();
			} else {
				state = ScanButtonState.Normal;
				title.Text = "Scan";
				activity.StopAnimating ();
			}
		}
	}
}