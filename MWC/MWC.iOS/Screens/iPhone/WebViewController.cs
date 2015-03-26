using System;
using System.Drawing;
using UIKit;
using Foundation;
using CoreGraphics;

namespace MWC.iOS {
	/// <summary>
	/// Display web content with back/forward buttons, plus refresh and close
	/// </summary>
	public class WebViewController : UIViewController {
		UIWebView webView;
		UIToolbar navBar;
		NSUrlRequest url;
		UIBarButtonItem [] items;
		
		public WebViewController (NSUrlRequest url) : base ()
		{
			this.url = url;
		}
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			navBar = new UIToolbar ();
			if (AppDelegate.IsPhone)
				navBar.Frame = new CGRect (0, View.Frame.Height-40, View.Frame.Width, 40);
			else
				navBar.Frame = new CGRect (0, View.Frame.Height-40, View.Frame.Width, 40);
			navBar.TintColor = UIColor.DarkGray;			

			items = new UIBarButtonItem [] {
				new UIBarButtonItem ("Back", UIBarButtonItemStyle.Bordered, (o, e) => { webView.GoBack (); }),
				new UIBarButtonItem ("Forward", UIBarButtonItemStyle.Bordered, (o, e) => { webView.GoForward (); }),
				new UIBarButtonItem (UIBarButtonSystemItem.FlexibleSpace, null),
				new UIBarButtonItem (UIBarButtonSystemItem.Refresh, (o, e) => { webView.Reload (); }),
				new UIBarButtonItem (UIBarButtonSystemItem.Stop, (o, e) => { 
					webView.StopLoading (); 
					
					// Phone: NavigationController, Pad: Modal
					if (NavigationController == null)
						DismissViewController (true, ()=> {});
					else
						NavigationController.PopViewController (true);
				})
			};

			navBar.Items = items;
			
			webView = new UIWebView ();
			if (AppDelegate.IsPhone)
				webView.Frame = new CGRect (0, 0, View.Frame.Width, View.Frame.Height-40);
			else
				webView.Frame = new CGRect (0, 0, View.Frame.Width, View.Frame.Height-40);

			webView.LoadStarted += delegate {
				UIApplication.SharedApplication.NetworkActivityIndicatorVisible = true;
				navBar.Items[0].Enabled = webView.CanGoBack;
				navBar.Items[1].Enabled = webView.CanGoForward;
			};
			webView.LoadFinished += delegate {
				UIApplication.SharedApplication.NetworkActivityIndicatorVisible = false;
				navBar.Items[0].Enabled = webView.CanGoBack;
				navBar.Items[1].Enabled = webView.CanGoForward;
			};
			
			webView.ScalesPageToFit = true;
			webView.SizeToFit ();
			webView.LoadRequest (url);
			
			navBar.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleTopMargin;
			webView.AutoresizingMask = UIViewAutoresizing.FlexibleDimensions;

			View.AddSubview (webView);
			View.AddSubview (navBar);
		}

		[Obsolete]
		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			if (AppDelegate.IsPad)
	            return true;
			else
				return toInterfaceOrientation == UIInterfaceOrientation.Portrait;
		}
	}
}