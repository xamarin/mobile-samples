using System.Drawing;
using System;
using Foundation;
using UIKit;
using CoreGraphics;

namespace MWC.iOS.Screens.Common.About {
	public partial class AboutXamScreen : UIViewController {
		public AboutXamScreen ()
			: base (AppDelegate.IsPhone ? "AboutXamScreen_iPhone" : "AboutXamScreen_iPad", null)
		{
		}
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			Title = "About Xamarin";
			
			StackOverflowButton.SetBackgroundImage (UIImage.FromFile ("Images/Logos/logo_stackoverflow.png"), UIControlState.Normal);
			LinkedInButton.SetBackgroundImage (UIImage.FromFile ("Images/Logos/logo_linkedin.png"), UIControlState.Normal);
			TwitterButton.SetBackgroundImage (UIImage.FromFile ("Images/Logos/logo_twitter.png"), UIControlState.Normal);
			YouTubeButton.SetBackgroundImage (UIImage.FromFile ("Images/Logos/logo_youtube.png"), UIControlState.Normal);
			FacebookButton.SetBackgroundImage (UIImage.FromFile ("Images/Logos/logo_facebook.png"), UIControlState.Normal);
			BlogRssButton.SetBackgroundImage (UIImage.FromFile ("Images/Logos/logo_rss.png"), UIControlState.Normal);


			StackOverflowButton.TouchUpInside += (sender, e) => {OpenUrl (Constants.AboutUrlStackOverflow);};
			LinkedInButton.TouchUpInside += (sender, e) => {OpenUrl (Constants.AboutUrlLinkedIn);};
			TwitterButton.TouchUpInside += (sender, e) => {OpenUrl (Constants.AboutUrlTwitter);};
			YouTubeButton.TouchUpInside += (sender, e) => {OpenUrl (Constants.AboutUrlYouTube);};
			FacebookButton.TouchUpInside += (sender, e) => {OpenUrl (Constants.AboutUrlFacebook);};
			BlogRssButton.TouchUpInside += (sender, e) => {OpenUrl (Constants.AboutUrlBlogRss);};


			if (AppDelegate.IsPhone) {
				ScrollView.Frame = new CGRect(0, 43, 320, 367);
				ScrollView.ContentSize = new CGSize(320, 610);
	
				XamLogoImageView.Image = UIImage.FromBundle("/Images/About");			
				
				AboutTextView.Frame = new CGRect(
											AboutTextView.Frame.X,
											AboutTextView.Frame.Y,
											320, 
											240);
				
				var Y = AboutTextView.Frame.Y + 240;

				StackOverflowButton.Frame = new CGRect (StackOverflowButton.Frame.X, Y, StackOverflowButton.Frame.Width, StackOverflowButton.Frame.Height);
				LinkedInButton.Frame = new CGRect (LinkedInButton.Frame.X, Y, LinkedInButton.Frame.Width, LinkedInButton.Frame.Height);
				TwitterButton.Frame = new CGRect (TwitterButton.Frame.X, Y, TwitterButton.Frame.Width, TwitterButton.Frame.Height);
				YouTubeButton.Frame = new CGRect (YouTubeButton.Frame.X, Y, YouTubeButton.Frame.Width, YouTubeButton.Frame.Height);
				FacebookButton.Frame = new CGRect (FacebookButton.Frame.X, Y, FacebookButton.Frame.Width, FacebookButton.Frame.Height);
				BlogRssButton.Frame = new CGRect (BlogRssButton.Frame.X, Y, BlogRssButton.Frame.Width, BlogRssButton.Frame.Height);
			} else {
				// IsPad
				ScrollView.Frame = new CGRect(0, 0, 768, 1004);
				ScrollView.ContentSize = new CGSize(768, 1024);

				XamLogoImageView.Image = UIImage.FromBundle("/Images/About-Portrait~iPad");			
				XamLogoImageView.Frame = ScrollView.Frame;
				
				CGSize size = AboutTextView.StringSize (AboutTextView.Text
								, AboutTextView.Font
								, new SizeF (738, 500)
								, UILineBreakMode.WordWrap);

				AboutTextView.Frame = new CGRect(
											15,
											650,
											size.Width, 
											size.Height + 30);
				//AboutTextView.AutoresizingMask = UIViewAutoresizing.FlexibleTopMargin | UIViewAutoresizing.FlexibleWidth;
				
				var Y = AboutTextView.Frame.Y + size.Height + 40;
				
				StackOverflowButton.AutoresizingMask = UIViewAutoresizing.FlexibleTopMargin | UIViewAutoresizing.FlexibleLeftMargin | UIViewAutoresizing.FlexibleRightMargin;
				LinkedInButton.AutoresizingMask = UIViewAutoresizing.FlexibleTopMargin | UIViewAutoresizing.FlexibleLeftMargin | UIViewAutoresizing.FlexibleRightMargin;
				TwitterButton.AutoresizingMask = UIViewAutoresizing.FlexibleTopMargin | UIViewAutoresizing.FlexibleLeftMargin | UIViewAutoresizing.FlexibleRightMargin;
				YouTubeButton.AutoresizingMask = UIViewAutoresizing.FlexibleTopMargin | UIViewAutoresizing.FlexibleLeftMargin | UIViewAutoresizing.FlexibleRightMargin;
				BlogRssButton.AutoresizingMask = UIViewAutoresizing.FlexibleTopMargin | UIViewAutoresizing.FlexibleLeftMargin | UIViewAutoresizing.FlexibleRightMargin;
				FacebookButton.AutoresizingMask = UIViewAutoresizing.FlexibleTopMargin | UIViewAutoresizing.FlexibleLeftMargin | UIViewAutoresizing.FlexibleRightMargin;				

				LayoutButtons(Y);
			}
			AboutTextView.Text = Constants.AboutText; // @"Xamarin was founded in 2011 with the mission to make it fast, easy and fun to build great mobile apps. Xamarin’s products are used by individual developers and companies, including VMware, Target, Rdio, Medtronic and Unity Technologies, to simplify creation and operation of high-performance, cross-platform mobile consumer and corporate applications, targeting phones and tablets running iOS, Android and Windows. For more information, visit http://xamarin.com.";
		}
		
		public bool IsPortrait {
			get {
				return InterfaceOrientation == UIInterfaceOrientation.Portrait 
					|| InterfaceOrientation == UIInterfaceOrientation.PortraitUpsideDown;
			}
		}
	
		protected void OnDeviceRotated(NSNotification notification)
		{
			if (AppDelegate.IsPad) {
				CGSize size;
				if(IsPortrait) {
					XamLogoImageView.Image = UIImage.FromBundle("/Images/About-Portrait~iPad");
					ScrollView.ContentSize = new CGSize(768, 1004);

					 size = AboutTextView.StringSize (AboutTextView.Text
									, AboutTextView.Font
									, new SizeF (738, 500)
									, UILineBreakMode.WordWrap);
	
					AboutTextView.Frame = new CGRect(
												15,
												650,
												size.Width, 
												size.Height + 30);
				} else {
					// IsLandscape
					XamLogoImageView.Image = UIImage.FromBundle("/Images/About-Landscape~iPad");
					ScrollView.ContentSize = new CGSize(1024, 748);

					size = AboutTextView.StringSize (AboutTextView.Text
									, AboutTextView.Font
									, new SizeF (1004, 500)
									, UILineBreakMode.WordWrap);
	
					AboutTextView.Frame = new CGRect(
												15,
												500,
												size.Width, 
												size.Height + 30);

				
				}
				var Y = AboutTextView.Frame.Y + size.Height + 30;
				LayoutButtons(Y);
			}
		}

		void OpenUrl (string url)
		{
			var nsurl = NSUrl.FromString(url);
			UIApplication.SharedApplication.OpenUrl (nsurl);
		}

		
		void LayoutButtons (nfloat Y) 
		{
			StackOverflowButton.Frame = new CGRect (StackOverflowButton.Frame.X, Y, StackOverflowButton.Frame.Width, StackOverflowButton.Frame.Height);
			LinkedInButton.Frame = new CGRect (LinkedInButton.Frame.X, Y, LinkedInButton.Frame.Width, LinkedInButton.Frame.Height);
			TwitterButton.Frame = new CGRect (TwitterButton.Frame.X, Y, TwitterButton.Frame.Width, TwitterButton.Frame.Height);
			YouTubeButton.Frame = new CGRect (YouTubeButton.Frame.X, Y, YouTubeButton.Frame.Width, YouTubeButton.Frame.Height);
			FacebookButton.Frame = new CGRect (FacebookButton.Frame.X, Y, FacebookButton.Frame.Width, FacebookButton.Frame.Height);
			BlogRssButton.Frame = new CGRect (BlogRssButton.Frame.X, Y, BlogRssButton.Frame.Width, BlogRssButton.Frame.Height);

		}
		NSObject ObserverRotation;

		/// <summary>
		/// Is called when the view is about to appear on the screen. We use method to hide the 
		/// navigation bar.
		/// </summary>
		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			OnDeviceRotated(null);
			ObserverRotation = NSNotificationCenter.DefaultCenter.AddObserver(
					AppDelegate.NotificationOrientationDidChange, OnDeviceRotated);
			UIDevice.CurrentDevice.BeginGeneratingDeviceOrientationNotifications();
		}
		
		/// <summary>
		/// Is called when the another view will appear and one will be hidden. We use method
		/// to show the navigation bar again.
		/// </summary>
		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);
			UIDevice.CurrentDevice.EndGeneratingDeviceOrientationNotifications();
			NSNotificationCenter.DefaultCenter.RemoveObserver(ObserverRotation);
		}
	}
}