using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace MWC.iOS.Screens.Common.About {
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
			Add (webView);
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			if (AppDelegate.IsPhone)
				webView.Frame = new RectangleF (0, 0, 320, 420);
			else {
				webView.Frame = new RectangleF (0, 0, View.Bounds.Width, View.Bounds.Height);
				
				webView.AutoresizingMask = UIViewAutoresizing.FlexibleDimensions;
			}
			var url = NSUrl.FromFilename("Images/About/index.html");
			var request = new NSUrlRequest(url);
			webView.LoadRequest(request);
		}
	}
}