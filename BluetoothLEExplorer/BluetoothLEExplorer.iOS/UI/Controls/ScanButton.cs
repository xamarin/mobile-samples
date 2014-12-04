using System;
using UIKit;
using CoreGraphics;

namespace BluetoothLEExplorer.iOS.UI.Controls
{
	public class ScanButton : UIButton
	{
		UILabel _title;
		UIActivityIndicatorView _activity;
		ScanButtonState _state = ScanButtonState.Normal;
		CGRect _initialFrame = new CGRect (0, 0, 65, 40);

		public ScanButton () : base ()
		{
			Frame = _initialFrame;

			_title = new UILabel ();
			_activity = new UIActivityIndicatorView (UIActivityIndicatorViewStyle.Gray);

			TouchUpInside += (sender, e) => {
				ChangeState();
			};

			Initialize ();
		}

		protected void Initialize()
		{
			_title.Frame = _initialFrame;
			_title.Text = "Scan";
			_title.Font = UIFont.SystemFontOfSize (UIFont.ButtonFontSize);
			_title.TextColor = UIColor.Blue;
			Add (_title);

			_activity.Frame = new CGRect (35, 0, 40, 40);
			_activity.HidesWhenStopped = true;
			Add (_activity);
		}

		protected void ChangeState()
		{
			//TODO: SetState already runs on the UI thread, 
			// so get rid of the call here and test.
			InvokeOnMainThread (() => {
				if (_state == ScanButtonState.Normal) {
					SetState (ScanButtonState.Scanning);
				} else {
					SetState (ScanButtonState.Normal);
				}
			});
		}

		public void SetState(ScanButtonState state)
		{
			InvokeOnMainThread (() => {
				if (_state == ScanButtonState.Normal) {
					_state = ScanButtonState.Scanning;
					_title.Text = "Stop";
					_activity.Hidden = false;
					_activity.StartAnimating ();
				} else {
					_state = ScanButtonState.Normal;
					_title.Text = "Scan";
					_activity.StopAnimating ();
				}
			});

		}
	
		public enum ScanButtonState
		{
			Normal,
			Scanning
		}
	}
}

