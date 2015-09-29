using System;
using System.Drawing;
using MonoTouch.Dialog.Utilities;
using Foundation;
using UIKit;
using CoreGraphics;

namespace MWC.iOS.Screens.iPhone.Twitter {
	/// <summary>
	/// Displays tweet: name, icon, tweet text
	/// </summary>
	public class TweetDetailsScreen : UIViewController, IImageUpdated {
		UILabel date, user;
		UnderlineLabel handle;
		UIButton handleButton;
		UIImageView image;
		UIWebView webView;
		EmptyOverlay emptyOverlay;
		BL.Tweet tweet;
		
		public TweetDetailsScreen (BL.Tweet showTweet) : base()
		{
			tweet = showTweet;
	
			View.BackgroundColor = UIColor.White;

			user = new UILabel () {
				TextAlignment = UITextAlignment.Left,
				Font = UIFont.FromName("Helvetica-Light",AppDelegate.Font16pt),
				BackgroundColor = UIColor.FromWhiteAlpha (0f, 0f)
			};
			handle = new UnderlineLabel () {
				TextAlignment = UITextAlignment.Left,
				Font = UIFont.FromName("Helvetica-Light",AppDelegate.Font9pt),
				TextColor = AppDelegate.ColorTextLink,
				
				BackgroundColor = UIColor.FromWhiteAlpha (0f, 0f)
			};
			handleButton = UIButton.FromType (UIButtonType.Custom);
			handleButton.TouchUpInside += (sender, e) => {
				var url = new NSUrl(tweet.AuthorUrl);
				var request = new NSUrlRequest(url);
				if (AppDelegate.IsPhone)
					NavigationController.PushViewController (new WebViewController (request), true);
				else
					PresentModalViewController (new WebViewController(request), true);
			};
			date = new UILabel () {
				TextAlignment = UITextAlignment.Left,
				Font = UIFont.FromName("Helvetica-Light",AppDelegate.Font9pt),
				TextColor = UIColor.DarkGray,
				BackgroundColor = UIColor.FromWhiteAlpha (0f, 0f)
			};

			image = new UIImageView();
			
			webView = new UIWebView();
			webView.Delegate = new WebViewDelegate(this);
			try { // iOS5 only
				webView.ScrollView.ScrollEnabled = false; 
				webView.ScrollView.Bounces = false;
			} catch {}

			View.AddSubview (user);
			View.AddSubview (handle);
			View.AddSubview (handleButton);
			View.AddSubview (image);
			View.AddSubview (date);
			View.AddSubview (webView);
			
			LayoutSubviews();
			if (tweet != null)
				Update ();
		}

		public void Update()
		{
			handle.Text = tweet.FormattedAuthor;
			user.Text = tweet.RealName;
			date.Text = tweet.FormattedTime;

			var u = new Uri(this.tweet.ImageUrl);
			var img = ImageLoader.DefaultRequestImage(u,this);
			if(img != null)
				image.Image = MWC.iOS.UI.CustomElements.TweetCell.RemoveSharpEdges (img);

			var css = "<style>" +
				"body {background-color:#ffffff; }" +
				"body,b,i,p,h2 {font-family:Helvetica-Light;}" +
				"</style>";
			
			webView.LoadHtmlString(css + tweet.Content, new NSUrl(NSBundle.MainBundle.BundlePath, true));
		}
		
		public void UpdatedImage (Uri uri)
		{
			ConsoleD.WriteLine ("UPDATED:" + uri.AbsoluteUri);
			var img = ImageLoader.DefaultRequestImage (uri, this);
			if (img != null)
				image.Image = MWC.iOS.UI.CustomElements.TweetCell.RemoveSharpEdges (img);
		}

		void LayoutSubviews ()
		{
			if (EmptyOverlay.ShowIfRequired (ref emptyOverlay, tweet, this.View
						, "No tweet selected", EmptyOverlayType.Twitter)) 
				return;
			
			image.Frame   = new CGRect(8,   8,  48, 48);
			user.Frame    = new CGRect(69, 14, 239, 24);
			handle.Frame  = new CGRect(69, 39, 239, 20); //14
			handleButton.Frame = new CGRect (69, 14, 239, 40); // over the two display fields
			date.Frame    = new CGRect(69, 55, 80,  15); 
			webView.Frame = new CGRect(0,  75, 320, 440 - 75);
		}
		
		class WebViewDelegate : UIWebViewDelegate {
			private TweetDetailsScreen tweetScreen;
			public WebViewDelegate (TweetDetailsScreen tds)
			{
				tweetScreen = tds;
			}
		
			/// <summary>
			/// Allow links inside tweets to be viewed in a UIWebView
			/// </summary>
			public override bool ShouldStartLoad (UIWebView webView, NSUrlRequest request, UIWebViewNavigationType navigationType)
			{
				if (navigationType == UIWebViewNavigationType.LinkClicked) {
					if (AppDelegate.IsPhone)
						tweetScreen.NavigationController.PushViewController (new WebViewController (request), true);
					else
						tweetScreen.PresentModalViewController (new WebViewController(request), true);
					return false;
				}
				return true;
			}
		}
	}
}