using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MWC.BL;

namespace MWC.iOS.Screens.iPhone.Speakers
{
	/// <summary>
	/// Displays personal information about the speaker, in a WebView
	/// (for ease of formatting)
	/// </summary>
	public class SpeakerDetailsScreen : WebViewControllerBase
	{
		Speaker _speaker;
		
		public SpeakerDetailsScreen (int speakerID) : base()
		{
			_speaker = BL.Managers.SpeakerManager.GetSpeaker ( speakerID );
			this.Title = _speaker.Name;
		}

		public void Update(Speaker speaker)
		{
			_speaker = speaker;
			LoadHtmlString(FormatText());
		}

		public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();
			webView.Delegate = new WebViewDelegate(this);
		}
		class WebViewDelegate : UIWebViewDelegate
		{
			//private SpeakerDetailsScreen _c;
			public WebViewDelegate (SpeakerDetailsScreen bc)
			{
			//	_c = bc;
			}

//TODO: uncomment when we implement 'links/buttons' in the speaker view (eg. links to sessions)
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
			sb.Append("<h2>"+_speaker.Name+"</h2>"+ Environment.NewLine);
			
			//sb.Append("<i>"+(_speaker.Position??"")+", "+(_speaker.Company??"")+"</i><br/>"+ Environment.NewLine);
			if (!string.IsNullOrEmpty(_speaker.Title))
			{
				sb.Append("<span class='body'>"+_speaker.Title+ "</span><br/>"+ Environment.NewLine);
				
			}
			sb.Append("<br />");
			if (!string.IsNullOrEmpty(_speaker.Company))
			{
				sb.Append("<span class='body'>"+_speaker.Company+ "</span><br/>"+ Environment.NewLine);
				
			}
			
			if (!string.IsNullOrEmpty(_speaker.Bio))
			{
				sb.Append("<span class='body'>"+_speaker.Bio+ "</span><br/>"+ Environment.NewLine);
			}

			if (!string.IsNullOrEmpty(_speaker.ImageUrl))
			{
				sb.Append("<img src='"+_speaker.ImageUrl+ "'/><br/>"+ Environment.NewLine);
			}

			//TODO: add more metadata to speaker view, if available
			//TODO: loop through the sessions for this speaker, to link thru
//			foreach (var session in _speaker.Sessions)
//			{
//				sb.Append("<div class='sessionspeaker'><a href='http://mwc.app/" + session.Code + "' class='sessionspeaker'>" + session.Title + "</a></div><br />");
//			}
			/*
			if (!string.IsNullOrEmpty(_speaker.BlogUrl))
				sb.Append("Blog: <a href='"+_speaker.BlogUrl+"'>"+_speaker.BlogUrl+"</a><br/>"+ Environment.NewLine);
			
			if (!string.IsNullOrEmpty(_speaker.TwitterName))
				sb.Append("Twitter: <a href='http://twitter.com/"+_speaker.TwitterName.Replace("@","")+"'>"+_speaker.TwitterName+"</a><br/>"+ Environment.NewLine);
			*/
			return sb.ToString();
		}
	}
}

