using System;
using System.Drawing;
using System.Text;
using MonoTouch.Dialog.Utilities;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MWC.SAL;

namespace MWC.iOS.Screens.iPhone.Twitter {
	/// <summary>
	/// Displays tweet: name, icon, tweet text
	/// </summary>
	public class TweetDetailsScreen : UIViewController, IImageUpdated {
		UILabel date, user, handle; //, tweetLabel;
		UIImageView image;
		UIWebView webView;

		BL.Tweet tweet;
		
		public TweetDetailsScreen (BL.Tweet showTweet) : base()
		{
			if (showTweet == null) return;

			tweet = showTweet;
	
			View.BackgroundColor = UIColor.White;

			user = new UILabel () {
				TextAlignment = UITextAlignment.Left,
				Font = UIFont.FromName("Helvetica-Light",AppDelegate.Font16pt),
				BackgroundColor = UIColor.FromWhiteAlpha (0f, 0f)
			};
			handle = new UILabel () {
				TextAlignment = UITextAlignment.Left,
				Font = UIFont.FromName("Helvetica-Light",AppDelegate.Font9pt),
				TextColor = UIColor.LightGray,
				BackgroundColor = UIColor.FromWhiteAlpha (0f, 0f)
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
			
			View.AddSubview (user);
			View.AddSubview (handle);
			View.AddSubview (image);
			View.AddSubview (date);
			View.AddSubview (webView);
			
			LayoutSubviews();
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
		
		void LayoutSubviews ()
		{
			image.Frame   = new RectangleF(8,   8,  48, 48);
			user.Frame    = new RectangleF(69, 14, 239, 24);
			handle.Frame  = new RectangleF(69, 39, 239, 14);
			date.Frame    = new RectangleF(69, 55, 80,  15); 
			webView.Frame = new RectangleF(0,  75, 320, 440 - 75);
		}
		
		public void UpdatedImage (Uri uri)
		{
			Console.WriteLine ("UPDATED:" + uri.AbsoluteUri);
			var img = ImageLoader.DefaultRequestImage (uri, this);
			if (img != null)
				image.Image = MWC.iOS.UI.CustomElements.TweetCell.RemoveSharpEdges (img);
		}

		class WebViewDelegate : UIWebViewDelegate {
			private TweetDetailsScreen twitterScreen;
			public WebViewDelegate (TweetDetailsScreen tds)
			{
				twitterScreen = tds;
			}
		
			/// <summary>
			/// Allow links inside tweets to be viewed in a UIWebView
			/// </summary>
			public override bool ShouldStartLoad (UIWebView webView, NSUrlRequest request, UIWebViewNavigationType navigationType)
			{
				if (navigationType == UIWebViewNavigationType.LinkClicked) {
					if (AppDelegate.IsPhone)
						twitterScreen.NavigationController.PushViewController (new WebViewController (request), true);
					else
						twitterScreen.PresentModalViewController (new WebViewController(request), true);
					return false;
				}
				return true;
			}
		}
	}
}