using System;
using Foundation;
using UIKit;
using System.Drawing;
using CoreGraphics;

namespace Example_StandardControls.Controls
{
	[Register ("ActivityIndicatorAlertView")]
	public class ActivityIndicatorAlertView : UIAlertView
	{
		/// <summary>
		/// our activity indicator
		/// </summary>
		UIActivityIndicatorView activityIndicator;
		/// <summary>
		/// the message label in the window
		/// </summary>
		UILabel lblMessage;

		/// <summary>
		/// The message that appears in the alert above the activity indicator
		/// </summary>
		string message;
		public override string Message {
			get { return message; }
			set { message = value; }
		}

		#region -= constructors =-

		public ActivityIndicatorAlertView (IntPtr handle) : base (handle)
		{
		}

		[Export ("initWithCoder:")]
		public ActivityIndicatorAlertView (NSCoder coder) : base (coder)
		{
		}

		public ActivityIndicatorAlertView ()
		{
		}

		#endregion

		/// <summary>
		/// we use this to resize our alert view. doing it at any other time has
		/// weird effects because of the lifecycle
		/// </summary>
		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();
			Frame = new CGRect (Frame.X, Frame.Y, Frame.Width, 120);
		}

		/// <summary>
		/// this is where we do the meat of creating our alert, which includes adding
		/// controls, etc.
		/// </summary>
		public override void Draw (CGRect rect)
		{
			if (activityIndicator == null) {
				if (!string.IsNullOrEmpty (message)) {
					lblMessage = new UILabel (new CGRect (20, 10, rect.Width - 40, 33));
					lblMessage.BackgroundColor = UIColor.Clear;
					lblMessage.TextColor = UIColor.LightTextColor;
					lblMessage.TextAlignment = UITextAlignment.Center;
					lblMessage.Text = message;
					AddSubview (lblMessage);
				}

				activityIndicator = new UIActivityIndicatorView (UIActivityIndicatorViewStyle.White);
				activityIndicator.Frame = new CGRect ((rect.Width / 2) - (activityIndicator.Frame.Width / 2)
					, 50, activityIndicator.Frame.Width, activityIndicator.Frame.Height);
				AddSubview (activityIndicator);
				activityIndicator.StartAnimating ();
			}
			base.Draw (rect);
		}

		/// <summary>
		/// dismisses the alert view. makes sure to call it on the main UI
		/// thread in case it's called from a worker thread.
		/// </summary>
		public void Hide (bool animated)
		{
			InvokeOnMainThread (() => {
				DismissWithClickedButtonIndex (0, animated);
			});
		}
	}
}