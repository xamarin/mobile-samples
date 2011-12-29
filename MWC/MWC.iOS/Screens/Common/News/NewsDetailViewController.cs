using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace MWC.iOS.Screens.Common.News
{
	/// <summary>
	/// 
	/// </summary>
	/// <remarks>
	/// Uses UIWebView since we want to format the text display (with HTML)
	/// </remarks>
	public class BlogEntryViewController  : WebViewControllerBase
	{
		string _title, _html;
		public void Update (string title, string html)
		{
			_html = html;
			_title = title;
			this.LoadHtmlString(FormatText());
		}
		public BlogEntryViewController (string title, string html) : base()
		{
			_title = title;
			_html = html;
		}
		protected override string FormatText()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(StyleHtmlSnippet);
			sb.Append("<h2>" + _title + "</h2>");
			sb.Append(_html);
			sb.Append("<br/>");
			return sb.ToString();
		}
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			webView.ShouldStartLoad = delegate (UIWebView webViewParam, NSUrlRequest request, UIWebViewNavigationType navigationType)
			{	// view links in a new 'webbrowser' window like about, session & twitter
				if (navigationType == UIWebViewNavigationType.LinkClicked)
				{
					this.NavigationController.PushViewController (new WebViewController (request), true);
					return false;
			}
				return true;
			};
		}
	}
}

