using System;
using UIKit;
using CoreGraphics;

namespace BluetoothLEExplorer.iOS.UI.Controls
{
	public class ScanButton : UIButton
	{
		protected UILabel _title;
		protected UIActivityIndicatorView _activity;
		public ScanButtonState State
		{
			get { return this._state; }
		}
		protected ScanButtonState _state = ScanButtonState.Normal;
		protected CGRect _initialFrame = new CGRect (0, 0, 65, 40);

		public ScanButton () : base ()
		{
			this.Frame = _initialFrame;

			this._title = new UILabel ();
			this._activity = new UIActivityIndicatorView (UIActivityIndicatorViewStyle.Gray);

			this.TouchUpInside += (sender, e) => {
				this.ChangeState();
			};

			this.Initialize ();
		}

		protected void Initialize()
		{
			this._title.Frame = _initialFrame;
			this._title.Text = "Scan";
			this._title.Font = UIFont.SystemFontOfSize (UIFont.ButtonFontSize);
			this._title.TextColor = UIColor.Blue;
			this.Add (this._title);

			this._activity.Frame = new CGRect (35, 0, 40, 40);
			this._activity.HidesWhenStopped = true;
			this.Add (this._activity);
		}

		protected void ChangeState()
		{
			//TODO: SetState already runs on the UI thread, 
			// so get rid of the call here and test.
			InvokeOnMainThread (() => {
				if (this._state == ScanButtonState.Normal) {
					this.SetState (ScanButtonState.Scanning);
				} else {
					this.SetState (ScanButtonState.Normal);
				}
			});
		}

		public void SetState(ScanButtonState state)
		{
			InvokeOnMainThread (() => {
				if (this._state == ScanButtonState.Normal) {
					this._state = ScanButtonState.Scanning;
					this._title.Text = "Stop";
					this._activity.Hidden = false;
					this._activity.StartAnimating ();
				} else {
					this._state = ScanButtonState.Normal;
					this._title.Text = "Scan";
					this._activity.StopAnimating ();
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

