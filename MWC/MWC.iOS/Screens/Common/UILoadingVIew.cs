using System;
using System.Diagnostics;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.ObjCRuntime;
using MonoTouch.UIKit;

namespace MWC.iOS.Screens.Common
{
	public delegate void FinishedFadeOutAndRemove();

	public class UILoadingView : UIView
	{
		UILabel loadingMessageLabel;
		UIImageView overlayBackground;
		UIActivityIndicatorView activityIndicator;
		public FinishedFadeOutAndRemove OnFinishedFadeOutAndRemove;
		
		public UILoadingView (string message)
		{
			Initialize(message);
		}

		public UILoadingView (IntPtr handle) : base(handle)
		{
			Initialize ();
		}
 
		[Export("initWithCoder:")]
		public UILoadingView (NSCoder coder) : base(coder)
		{
			Initialize ();
		}
 
 		void Initialize ()
		{
			Initialize("Loading...");	
		}
		
		void Initialize (string message)
		{
			//using (var pool = new NSAutoreleasePool())
			//{
				SetUpLoadingMessageLabel(message);
				SetUpActivityIndicator();
				SetUpOverlayBackground();
			
				this.AddSubview(overlayBackground);
				this.AddSubview(activityIndicator);
				this.AddSubview(loadingMessageLabel);
			//}
		}

		void SetUpOverlayBackground ()
		{
			overlayBackground = new UIImageView(new RectangleF(0f, 0f, 320f, 460f));
			overlayBackground.BackgroundColor = new UIColor(0f, 0f, 0f, 0.2f); // 0.75f
		}


		void SetUpActivityIndicator ()
		{
			activityIndicator = new UIActivityIndicatorView(new RectangleF(150f, 220f, 20f, 20f));
			activityIndicator.StartAnimating();          
		}


		void SetUpLoadingMessageLabel (string message)
		{
			// Set up loading message - Positioned Above centre in the middle
			loadingMessageLabel = new UILabel(new RectangleF(53f, 139f, 214f, 62f));
			loadingMessageLabel.BackgroundColor = UIColor.Clear;
			loadingMessageLabel.AdjustsFontSizeToFitWidth = true;
			loadingMessageLabel.TextColor = UIColor.White;
			loadingMessageLabel.TextAlignment = UITextAlignment.Center;
			loadingMessageLabel.Lines = 3;
			loadingMessageLabel.Text = message;
			loadingMessageLabel.Font = UIFont.BoldSystemFontOfSize(16f);
			this.AddSubview(loadingMessageLabel);
		}
		
		public override void WillRemoveSubview (UIView uiview)
		{
			activityIndicator.StopAnimating();
		}
		
		public void FadeOutAndRemove()
		{
			InvokeOnMainThread( delegate { 
				Debug.WriteLine ("Fade out loading screen...");
				UIView.BeginAnimations("FadeOutLoadingView");
				UIView.SetAnimationDuration(0.5f);
				UIView.SetAnimationDelegate(this);
				UIView.SetAnimationTransition(UIViewAnimationTransition.None, this, true);
				UIView.SetAnimationDidStopSelector(new Selector("FadeOutLoadingViewDone"));
			    this.Alpha = 0f;
				UIView.CommitAnimations();	
			});

		}
		
		[Export("FadeOutLoadingViewDone")]
		void FadeOutLoadingViewDone()
		{ 
			Debug.WriteLine ("RemoveFromSuperview...");
			this.RemoveFromSuperview();
			this.OnFinishedFadeOutAndRemove();
		}
	}
}