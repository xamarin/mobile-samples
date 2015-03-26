using System;
using System.Drawing;
using Foundation;
using UIKit;
using CoreGraphics;

namespace MWC.iOS {
	public class WebViewControllerBase : UIViewController {
		protected string basedir;
		protected UIWebView webView;
		/// <summary>
		/// Shared Css styles
		/// </summary>
		public string StyleHtmlSnippet {
			get {
				// http://jonraasch.com/blog/css-rounded-corners-in-all-browsers

				return "<style>" +
				"body {background-color:#ffffff; font-size:140%; }"+
				"body,b,i,p,h2{font-family:Helvetica-Light;}" +
				"h1,h2{color:#222222;}" +
				"h1,h2{margin-bottom:0px;}" +
				".footnote{font-size:small;}" +
				".sessionspeaker{color:#333333;font-weight:bold;}" +
				".sessionroom{color:#666666;}" +
				".sessiontime{color:#666666;}" +
				".sessiontag{color:#444444;}" +
				"div.sessionspeaker { -webkit-border-radius:12px; background:white; width:285; color:black; padding:8 10 10 8;  }" +
				"a.sessionspeaker {color:black; text-decoration:none;}"+
				"</style>";

			}
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			if (AppDelegate.IsPhone)
				webView.Frame = new CGRect (0, 0, 320, 420);
			else {
				// something weird happening with View.Frame and SplitViewController...
				if (InterfaceOrientation == UIInterfaceOrientation.LandscapeLeft
					|| InterfaceOrientation == UIInterfaceOrientation.LandscapeRight)
					webView.Frame = new CGRect (0, 0, 1024-321, View.Bounds.Height);
				else
					webView.Frame = new CGRect (0, 0, 768-321, View.Bounds.Height);
				//webView.AutoresizingMask = UIViewAutoresizing.FlexibleDimensions;
			}
		}
		public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();
			
			basedir = NSBundle.MainBundle.BundlePath;
			// no XIB !
			webView = new UIWebView() {
				ScalesPageToFit = false
			};
			LoadHtmlString(FormatText());
            
            // Add the table view as a subview
            View.AddSubview(webView);
		}
		protected virtual string FormatText()
		{ return ""; }

		protected virtual void LoadHtmlString (string s)
		{
			webView.LoadHtmlString(s, new NSUrl(basedir, true));
		}
	}
}