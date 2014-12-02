using System;
using System.Drawing;
using Foundation;
using UIKit;
using CoreGraphics;

namespace MWC.iOS.Screens.Common.About {
	/// <summary>
	/// This screen REPLACES the old XIB version
	/// </summary>
	public class AboutXamarinScreen : UIViewController {
		protected string basedir;
		UIWebView webView;

		public AboutXamarinScreen ()
		{
			Title = "About Xamarin";
		}
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			webView = new UIWebView();

			webView.ShouldStartLoad = 
			delegate (UIWebView webViewParam, NSUrlRequest request, UIWebViewNavigationType navigationType) {
				// view links in a new 'webbrowser' window like about, session & twitter
				if (navigationType == UIWebViewNavigationType.LinkClicked) {
					UIApplication.SharedApplication.OpenUrl (request.Url);
					return false;
				}
				return true;
			};

			Add (webView);
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			webView.Frame = new CGRect (0, 0, View.Bounds.Width, View.Bounds.Height);
			webView.AutoresizingMask = UIViewAutoresizing.FlexibleDimensions;
			
			NSUrl url = null;
			
			if(AppDelegate.IsPad)
				url = NSUrl.FromFilename("Images/About/iPad/index.html");
			else
				url = NSUrl.FromFilename("Images/About/iPhone/index.html");
			var request = new NSUrlRequest(url);
			webView.LoadRequest(request);
		}
	}
}