using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

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
				"body {background-color:#ffffff; }"+
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
		public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();
			
			basedir = NSBundle.MainBundle.BundlePath;
			// no XIB !
			webView = new UIWebView() {
				ScalesPageToFit = false,
			};
			LoadHtmlString(FormatText());
            webView.SizeToFit();
            webView.Frame = new RectangleF (0, 0, View.Frame.Width, View.Frame.Height-93);
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