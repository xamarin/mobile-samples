using System;
using Foundation;
using UIKit;
using MWC.BL;

namespace MWC.iOS.Screens.Common.News {

	/// <remarks>
	/// Uses UIWebView since we want to format the text display (with HTML)
	/// </remarks>
	public class NewsDetailsScreen  : WebViewControllerBase {
		RSSEntry newsEntry;
		EmptyOverlay emptyOverlay;
		
		public NewsDetailsScreen (RSSEntry entry) : base()
		{
			newsEntry = entry;
			View.BackgroundColor = UIColor.White;
			webView.BackgroundColor = UIColor.White;
		}

		public void Update (RSSEntry entry)
		{
			this.LoadHtmlString(FormatText());
		}
		
		protected override string FormatText()
		{
			var styleString = @"<style>
	body {
		font-family:Helvetica-Light, Helvetica, sans-serif;
	}
	body, tr, td, a, font {
		font-family:Helvetica-Light, Helvetica, sans-serif;
	}
</style>";

			if (AppDelegate.IsPad) {
				// set the font large
				styleString = @"<style>
	body {
		font-family:Helvetica-Light, Helvetica, sans-serif;
	}
	body, tr, td, a, font {
		font-size: large;
		font-family:Helvetica-Light, Helvetica, sans-serif;
	}
</style>";
			}

			return newsEntry==null?"": @"<html>"+styleString+newsEntry.Content+"</html>";
		}
		protected override void LoadHtmlString (string s)
		{
			if (newsEntry == null) return;

			Uri u = new Uri(newsEntry.Url);
			NSUrl baseUrl = new NSUrl("http://" + u.DnsSafeHost);

			webView.LoadHtmlString (s, baseUrl);
		}
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			if (EmptyOverlay.ShowIfRequired(ref emptyOverlay, newsEntry, View, "No news selected", EmptyOverlayType.News)) return;

			webView.ShouldStartLoad = 
			delegate (UIWebView webViewParam, NSUrlRequest request, UIWebViewNavigationType navigationType) {
				// view links in a new 'webbrowser' window like about, session & twitter
				if (navigationType == UIWebViewNavigationType.LinkClicked) {
					if (AppDelegate.IsPhone)
						this.NavigationController.PushViewController (new WebViewController (request), true);
					else
						this.PresentModalViewController (new WebViewController(request), true);
					return false;
				}
				return true;
			};
		}
	}
}