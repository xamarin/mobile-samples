using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MWC.BL;

namespace MWC.iOS.Screens.Common.News
{
	/// <summary>
	/// 
	/// </summary>
	/// <remarks>
	/// Uses UIWebView since we want to format the text display (with HTML)
	/// </remarks>
	public class NewsDetailsScreen  : WebViewControllerBase
	{
		RSSEntry _entry;
		public void Update (RSSEntry entry)
		{
			this.LoadHtmlString(FormatText());
		}
		public NewsDetailsScreen (RSSEntry entry) : base()
		{
			_entry = entry;
		}
		protected override string FormatText()
		{
			return _entry==null?"":"<html>"+_entry.Content+"</_html>";
		}
		protected override void LoadHtmlString (string s)
		{
			if (_entry == null) return;

			Uri u = new Uri(_entry.Url);
			NSUrl baseUrl = new NSUrl("http://" + u.DnsSafeHost);

			webView.LoadHtmlString (s, baseUrl);
		}
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			webView.ShouldStartLoad = delegate (UIWebView webViewParam, NSUrlRequest request, UIWebViewNavigationType navigationType)
			{	// view links in a new 'webbrowser' window like about, session & twitter
				if (navigationType == UIWebViewNavigationType.LinkClicked)
				{
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

