using System;
using System.Diagnostics;
using System.Drawing;
using Foundation;
using ObjCRuntime;
using UIKit;
using CoreGraphics;

namespace MWC.iOS.Screens.Common {
	public delegate void FinishedFadeOutAndRemove ();

	public class UILoadingView : UIView {
		UILabel loadingMessageLabel;
		UIImageView overlayBackground;
		UIActivityIndicatorView activityIndicator;

		string message;
		bool initialized;

		public FinishedFadeOutAndRemove OnFinishedFadeOutAndRemove;
		
		public UILoadingView (string message, CGRect bounds)
		{
			this.message = message;
			Initialize(message, bounds);
		}

		public UILoadingView (IntPtr handle) : base (handle)
		{
			message = "Loading...";
		}
 
		[Export("initWithCoder:")]
		public UILoadingView (NSCoder coder) : base (coder)
		{
			message = "Loading...";
		}
 
		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();
			var b = Superview.Bounds; // defaults to 
			if (!initialized)
				Initialize (message, b);
		}

		void Initialize (string message, CGRect bounds)
		{
			ConsoleD.WriteLine ("UILoadingView.Initialize " + bounds);
			SetUpLoadingMessageLabel (message);
			SetUpActivityIndicator ();
			SetUpOverlayBackground (bounds);
		
			AutoresizingMask = UIViewAutoresizing.FlexibleDimensions;

			AddSubview (overlayBackground);
			AddSubview (activityIndicator);
			AddSubview (loadingMessageLabel);

			initialized = true;
		}

		void SetUpOverlayBackground (CGRect bounds)
		{
			overlayBackground = new UIImageView (bounds);
			overlayBackground.BackgroundColor = new UIColor (0f, 0f, 0f, 0.2f); // 0.75f
			//overlayBackground.BackgroundColor = UIColor.Blue;
			if (AppDelegate.IsPad)
				overlayBackground.AutoresizingMask = UIViewAutoresizing.FlexibleDimensions;
		}


		void SetUpActivityIndicator ()
		{
			activityIndicator = new UIActivityIndicatorView (new CGRect (150f, 220f, 20f, 20f));
			if (AppDelegate.IsPad)
				activityIndicator.AutoresizingMask = UIViewAutoresizing.FlexibleMargins;
			activityIndicator.StartAnimating ();
		}
		
		void SetUpLoadingMessageLabel (string message)
		{
			// Set up loading message - Positioned Above centre in the middle
			loadingMessageLabel = new UILabel (new CGRect(53f, 139f, 214f, 62f));
			loadingMessageLabel.BackgroundColor = UIColor.Clear;
			loadingMessageLabel.AdjustsFontSizeToFitWidth = true;
			loadingMessageLabel.TextColor = UIColor.White;
			loadingMessageLabel.TextAlignment = UITextAlignment.Center;
			loadingMessageLabel.Lines = 3;
			loadingMessageLabel.Text = message;
			loadingMessageLabel.Font = UIFont.BoldSystemFontOfSize (16f);
			if (AppDelegate.IsPad)
				loadingMessageLabel.AutoresizingMask = UIViewAutoresizing.FlexibleMargins;
			AddSubview (loadingMessageLabel);
		}
		
		public override void WillRemoveSubview (UIView uiview)
		{
			if (activityIndicator != null)
				activityIndicator.StopAnimating ();
		}
		
		public void FadeOutAndRemove()
		{
			InvokeOnMainThread( delegate { 
				Debug.WriteLine ("Fade out loading screen...");
				UIView.BeginAnimations ("FadeOutLoadingView");
				UIView.SetAnimationDuration (0.5f);
				UIView.SetAnimationDelegate (this);
				UIView.SetAnimationTransition (UIViewAnimationTransition.None, this, true);
				UIView.SetAnimationDidStopSelector (new Selector ("FadeOutLoadingViewDone"));
			    Alpha = 0f;
				UIView.CommitAnimations();	
			});

		}
		
		[Export("FadeOutLoadingViewDone")]
		void FadeOutLoadingViewDone()
		{ 
			Debug.WriteLine ("RemoveFromSuperview...");
			RemoveFromSuperview ();
			OnFinishedFadeOutAndRemove ();
		}
	}
}