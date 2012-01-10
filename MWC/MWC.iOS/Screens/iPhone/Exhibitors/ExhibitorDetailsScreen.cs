using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MWC.BL;

namespace MWC.iOS.Screens.iPhone.Exhibitors
{
	/// <summary>
	/// Displays information about the Exhibitor, in a WebView
	/// (for ease of formatting)
	/// </summary>
	public class ExhibitorDetailsScreen : WebViewControllerBase
	{
		Exhibitor _Exhibitor;
		
		public ExhibitorDetailsScreen (int ExhibitorID) : base()
		{
			_Exhibitor = BL.Managers.ExhibitorManager.GetExhibitor ( ExhibitorID );
			this.Title = _Exhibitor.Name;
		}

		public void Update(Exhibitor Exhibitor)
		{
			_Exhibitor = Exhibitor;
			LoadHtmlString(FormatText());
		}

		public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();
			webView.Delegate = new WebViewDelegate(this);
		}
		class WebViewDelegate : UIWebViewDelegate
		{
			//private ExhibitorDetailsScreen _c;
			public WebViewDelegate (ExhibitorDetailsScreen bc)
			{
			//	_c = bc;
			}

//TODO: uncomment when we implement 'links/buttons' in the Exhibitor view (eg. links to sessions)
//			private SessionViewController sessVC;
		
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
		/// Format the parts-of-speech text for UIWebView
		/// </summary>
		protected override string FormatText()
		{
			StringBuilder sb = new StringBuilder();
			
			sb.Append(StyleHtmlSnippet);
			sb.Append("<h2>"+_Exhibitor.Name+"</h2>"+ Environment.NewLine);
			
			//sb.Append("<i>"+(_Exhibitor.Position??"")+", "+(_Exhibitor.Company??"")+"</i><br/>"+ Environment.NewLine);
			if (!string.IsNullOrEmpty(_Exhibitor.City))
			{
				sb.Append("<span class='body'>"+_Exhibitor.City+ "</span><br/>"+ Environment.NewLine);
				
			}
			sb.Append("<br />");
			if (!string.IsNullOrEmpty(_Exhibitor.Country))
			{
				sb.Append("<span class='body'>"+_Exhibitor.Country+ "</span><br/>"+ Environment.NewLine);
				
			}

			sb.Append("<img src='Images/Temp/Logo_Anritsu.png' align='right' border='0'/>"+ Environment.NewLine);

			//TODO: add more metadata to Exhibitor view, if available
			/*
			if (!string.IsNullOrEmpty(_Exhibitor.BlogUrl))
				sb.Append("Blog: <a href='"+_Exhibitor.BlogUrl+"'>"+_Exhibitor.BlogUrl+"</a><br/>"+ Environment.NewLine);
			
			if (!string.IsNullOrEmpty(_Exhibitor.TwitterName))
				sb.Append("Twitter: <a href='http://twitter.com/"+_Exhibitor.TwitterName.Replace("@","")+"'>"+_Exhibitor.TwitterName+"</a><br/>"+ Environment.NewLine);
			*/
			return sb.ToString();
		}
	}
}

