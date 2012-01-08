using System;
using System.Drawing;
using MonoTouch.UIKit;

namespace MWC.iOS.UI.Controls
{
	/// <summary>
	/// A simple modal overlay that shows an activity spinner and loading message. To use, 
	/// instantiate with a RectangleF frame, and add to super view. When finished, call Hide().
	/// </summary>
	public class LoadingOverlay : UIView
	{
		// control declarations
		UIActivityIndicatorView _activitySpinner;
		UILabel _loadingLabel;
		
		public LoadingOverlay ( RectangleF frame ) : base ( frame )
		{
			// configurable bits
			this.BackgroundColor = UIColor.Black;
			this.Alpha = 0.75f;
			float labelHeight = 22;
			float labelWidth = this.Frame.Width - 20;
			
			// derive the center x and y
			float centerX = this.Frame.Width / 2;
			float centerY = this.Frame.Height / 2;
			
			// create the activity spinner, center it horizontall and put it 5 points above center x
			this._activitySpinner = new UIActivityIndicatorView(UIActivityIndicatorViewStyle.WhiteLarge);
			this._activitySpinner.Frame = new RectangleF ( 
				centerX - (this._activitySpinner.Frame.Width / 2) ,
				centerY - this._activitySpinner.Frame.Height - 5 ,
				this._activitySpinner.Frame.Width ,
				this._activitySpinner.Frame.Height );
			this.AddSubview ( this._activitySpinner );
			this._activitySpinner.StartAnimating ();
			
			// create and configure the "Loading Data" label
			this._loadingLabel = new UILabel( new RectangleF (
				centerX - (labelWidth / 2),
				centerY + 5 ,
				labelWidth ,
				labelHeight
				));
			this._loadingLabel.BackgroundColor = UIColor.Clear;
			this._loadingLabel.TextColor = UIColor.White;
			this._loadingLabel.Text = "Loading Data...";
			this._loadingLabel.TextAlignment = UITextAlignment.Center;
			this.AddSubview ( this._loadingLabel );
		}
		
		/// <summary>
		/// Fades out the control and then removes it from the super view
		/// </summary>
		public void Hide ()
		{
			UIView.Animate ( 
				0.5, // duration
				() => { this.Alpha = 0; }, 
				() => { this.RemoveFromSuperview(); }
			);
		}
	}
}

