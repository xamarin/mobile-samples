using System;
using System.Text;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MWC.SAL;

namespace MWC.iOS.Screens.iPhone.Twitter
{
	/// <summary>
	/// Displays tweet: name, icon, tweet text
	/// (in a WebView for ease of formatting)
	/// </summary>
	public class TweetDetailsScreen : WebViewControllerBase
	{
		BL.Tweet _tweet;
		
		public TweetDetailsScreen (BL.Tweet tweet) : base()
		{
			this._tweet = tweet;
		}

		public void Update(BL.Tweet tweet)
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
		
			/// <summary>
			/// Allow links inside tweets to be viewed in a UIWebView
			/// </summary>
			public override bool ShouldStartLoad (UIWebView webView, NSUrlRequest request, UIWebViewNavigationType navigationType)
			{
				if (navigationType == UIWebViewNavigationType.LinkClicked)
				{
					_c.NavigationController.PushViewController (new WebViewController (request), true);
					return false;
				}
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

			sb.Append(string.Format ("<img src='../Documents/twitter-images/{0}' align='left' />", _tweet.FormattedAuthor));
			sb.Append("<h2>@"+_tweet.FormattedAuthor+"</h2>"+ Environment.NewLine);
			
			if (!string.IsNullOrEmpty(_tweet.RealName))
			{
				sb.Append("<span class='body'>"+_tweet.RealName+ "</span><br/>"+ Environment.NewLine);
				
			}

			if (!string.IsNullOrEmpty(_tweet.Content))
			{
				sb.Append("<span class='body'>"+_tweet.Content+ "</span><br/>"+ Environment.NewLine);
				
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