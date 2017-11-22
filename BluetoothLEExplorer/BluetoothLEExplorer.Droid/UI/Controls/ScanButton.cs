using System;
using Android.Widget;
using Android.App;
using Android.Content;

namespace BluetoothLEExplorer.Droid.UI.Controls
{
	public class ScanButton : Button
	{
		//TODO: add a progress indicator, see: http://stackoverflow.com/questions/5442183/using-the-animated-circle-in-an-imageview-while-loading-stuff
//		protected UILabel _title;
//		protected UIActivityIndicatorView _activity;
		public ScanButtonState State
		{
			get { return this._state; }
		}
		protected ScanButtonState _state = ScanButtonState.Normal;

		public ScanButton (IntPtr javaReference, Android.Runtime.JniHandleOwnership transfer ) : base (javaReference, transfer)
		{
			this.Initialize ();
		}
		public ScanButton (Context context, Android.Util.IAttributeSet attrs) : base (context, attrs)
		{
			this.Initialize ();
		}

		public ScanButton (Context context, Android.Util.IAttributeSet attrs, int defStyle) : base (context, attrs, defStyle)
		{
			this.Initialize ();
		}

		public ScanButton (Context context) : base (context)
		{
//			this._title = new UILabel ();
//			this._activity = new UIActivityIndicatorView (UIActivityIndicatorViewStyle.Gray);
			this.Initialize ();
		}

		protected void Initialize()
		{
			this.Click += (sender, e) => {
				this.ChangeState();
			};

//			this._title.Frame = _initialFrame;
			this.Text = "Scan";
//			this._title.Font = UIFont.SystemFontOfSize (UIFont.ButtonFontSize);
//			this._title.TextColor = UIColor.Blue;
//			this.Add (this._title);
//
//			this._activity.Frame = new RectangleF (35, 0, 40, 40);
//			this._activity.HidesWhenStopped = true;
//			this.Add (this._activity);
		}

		protected void ChangeState()
		{
			if (this._state == ScanButtonState.Normal) {
				this.SetState (ScanButtonState.Scanning);
			} else {
				this.SetState (ScanButtonState.Normal);
			}
		}

		public void SetState(ScanButtonState state)
		{
			((Activity)this.Context).RunOnUiThread  (() => {
				if (this._state == ScanButtonState.Normal) {
					this._state = ScanButtonState.Scanning;
					this.Text = "Stop";
//					this._activity.Hidden = false;
//					this._activity.StartAnimating ();
				} else {
					this._state = ScanButtonState.Normal;
					this.Text = "Scan";
//					this._activity.StopAnimating ();
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

