using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MWC.BL;
using MWC.SAL;

namespace MWC.iOS.Screens.iPhone.Twitter
{
	/// <summary>
	/// Displays personal information about the speaker, in a WebView
	/// (for ease of formatting)
	/// </summary>
	public class TweetDetailsScreen : WebViewControllerBase
	{
		Tweet _tweet;
		
		public TweetDetailsScreen (Tweet tweet) : base()
		{
			this._tweet = tweet;
		}

		public void Update(Tweet tweet)
		{
			this._tweet = tweet;
			LoadHtmlString(FormatText());
		}

		public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();
			webView.Delegate = new WebViewDelegate(this);
		}
		class WebViewDelegate : UIWebViewDelegate
		{
			private TweetDetailsScreen _c;
			public WebViewDelegate (TweetDetailsScreen bc)
			{
				_c = bc;
			}
		
			public override bool ShouldStartLoad (UIWebView webView, NSUrlRequest request, UIWebViewNavigationType navigationType)
			{
//				if (navigationType == UIWebViewNavigationType.LinkClicked)
//				{
//					string path = request.Url.Path.Substring(1);
//					if (sessVC == null)
//					{
//						sessVC = new SessionViewController(path);
//					} else {
//						sessVC.Update(path);
//					}
//					//sessVC.Title = s.Title;
//					_c.NavigationController.PushViewController(sessVC, true);
//				}
				return true;
			}
		}
		/// <summary>
		/// Format the tweet for UIWebView
		/// </summary>
		protected override string FormatText()
		{
			StringBuilder sb = new StringBuilder();
			
			sb.Append(StyleHtmlSnippet);
			sb.Append("<h2>@"+_tweet.FormattedAuthor+"</h2>"+ Environment.NewLine);
			
			if (!string.IsNullOrEmpty(_tweet.Title))
			{
				sb.Append("<span class='body'>"+_tweet.Title+ "</span><br/>"+ Environment.NewLine);
				
			}
			sb.Append("<br />");
			if (!string.IsNullOrEmpty(_tweet.FormattedTime))
			{
				sb.Append("<span class='body'>"+_tweet.FormattedTime+ "</span><br/>"+ Environment.NewLine);
				
			}

			return sb.ToString();
		}
	}
}

