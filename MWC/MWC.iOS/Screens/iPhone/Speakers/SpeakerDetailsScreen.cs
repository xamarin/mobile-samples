using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MWC.BL;
using MonoTouch.Dialog.Utilities;

namespace MWC.iOS.Screens.iPhone.Speakers
{
	/// <summary>
	/// Displays personal information about the speaker
	/// </summary>
	public class SpeakerDetailsScreen : UIViewController, IImageUpdated
	{
		UILabel _nameLabel, _titleLabel, _companyLabel, _bioLabel;
		UIImageView _image;

		Speaker _speaker;
		
		const int ImageSpace = 80;

		public SpeakerDetailsScreen (int speakerID) : base()
		{
			_speaker = BL.Managers.SpeakerManager.GetSpeaker ( speakerID );
			this.View.BackgroundColor = UIColor.White;
			_nameLabel = new UILabel () {
				TextAlignment = UITextAlignment.Left,
				Font = UIFont.FromName ("Helvetica-Light", AppDelegate.Font16pt),
				BackgroundColor = UIColor.FromWhiteAlpha (0f, 0f)
			};
			_titleLabel = new UILabel () {
				TextAlignment = UITextAlignment.Left,
				Font = UIFont.FromName ("Helvetica-LightOblique", AppDelegate.Font10pt),
				TextColor = UIColor.DarkGray,
				BackgroundColor = UIColor.FromWhiteAlpha (0f, 0f)
			};
			_companyLabel = new UILabel () {
				TextAlignment = UITextAlignment.Left,
				Font = UIFont.FromName ("Helvetica-Light", AppDelegate.Font10pt),
				TextColor = UIColor.DarkGray,
				BackgroundColor = UIColor.FromWhiteAlpha (0f, 0f)
			};
			 _bioLabel = new UILabel () {
				TextAlignment = UITextAlignment.Left,
				Font = UIFont.FromName ("Helvetica-Light", AppDelegate.Font10_5pt),
				BackgroundColor = UIColor.FromWhiteAlpha (0f, 0f),
				Lines = 0
			};
			_image = new UIImageView();

			this.View.AddSubview (_nameLabel);
			this.View.AddSubview (_titleLabel);
			this.View.AddSubview (_companyLabel);
			this.View.AddSubview (_bioLabel);
			this.View.AddSubview (_image);

			LayoutSubviews ();

			Update ();			
		}

		void LayoutSubviews ()
		{
			var full = this.View.Bounds;
			var bigFrame = full;
			
			bigFrame.X = ImageSpace+13+17;
			bigFrame.Y = 27; // 15 -> 13
			bigFrame.Height = 26;
			bigFrame.Width -= (ImageSpace+13+17);
			_nameLabel.Frame = bigFrame;
			
			var smallFrame = full;
			smallFrame.X = ImageSpace+13+17;
			smallFrame.Y = 27+26;
			smallFrame.Height = 15; // 12 -> 15
			smallFrame.Width -= (ImageSpace+13+17);
			_titleLabel.Frame = smallFrame;
			
			smallFrame.Y += 17;
			_companyLabel.Frame = smallFrame;

			_image.Frame = new RectangleF(13, 15, 80, 80);
			
			if (!String.IsNullOrEmpty(_speaker.Bio))
			{
				SizeF size = _bioLabel.StringSize (_speaker.Bio
									, _bioLabel.Font
									, new SizeF (290, 500)
									, UILineBreakMode.WordWrap);
				_bioLabel.Frame = new RectangleF(15, 115, 290, size.Height);
			}
			else
			{
				_bioLabel.Frame = new RectangleF(15, 115, 290, 20);
			}
		}

		void Update()
		{
			this._nameLabel.Text = _speaker.Name;
			this._titleLabel.Text = _speaker.Title;
			this._companyLabel.Text = _speaker.Company;

			if (!String.IsNullOrEmpty(_speaker.Bio))
			{
				this._bioLabel.Text = _speaker.Bio;
				this._bioLabel.Font = UIFont.FromName ("Helvetica-Light", AppDelegate.Font10_5pt);
				this._bioLabel.TextColor = UIColor.Black;
			}
			else
			{
				this._bioLabel.Font = UIFont.FromName ("Helvetica-LightOblique", AppDelegate.Font10_5pt);
				this._bioLabel.TextColor = UIColor.Gray;
				this._bioLabel.Text = "No background information available.";
			}
			if (_speaker.ImageUrl != "http://www.mobileworldcongress.com")
			{
				//Console.WriteLine("INITIAL:" + speaker.ImageUrl);
				var u = new Uri(_speaker.ImageUrl);
				_image.Image = ImageLoader.DefaultRequestImage(u,this);
			}
		}

		public void UpdatedImage (Uri uri)
		{
			//Console.WriteLine("UPDATED:" + uri.AbsoluteUri);
			_image.Image = ImageLoader.DefaultRequestImage(uri, this);
		}
		


//		public override void ViewDidLoad ()
//        {
//            base.ViewDidLoad ();
//			webView.Delegate = new WebViewDelegate(this);
//		}
















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
//		protected  string FormatText()	
//		{
//			StringBuilder sb = new StringBuilder();
//			
//			sb.Append(StyleHtmlSnippet);
//			sb.Append("<h2>"+_speaker.Name+"</h2>"+ Environment.NewLine);
//			
//			//sb.Append("<i>"+(_speaker.Position??"")+", "+(_speaker.Company??"")+"</i><br/>"+ Environment.NewLine);
//			if (!string.IsNullOrEmpty(_speaker.Title))
//			{
//				sb.Append("<span class='body'>"+_speaker.Title+ "</span><br/>"+ Environment.NewLine);
//				
//			}
//			sb.Append("<br />");
//			if (!string.IsNullOrEmpty(_speaker.Company))
//			{
//				sb.Append("<span class='body'>"+_speaker.Company+ "</span><br/>"+ Environment.NewLine);
//				
//			}
//			
//			if (!string.IsNullOrEmpty(_speaker.Bio))
//			{
//				sb.Append("<span class='body'>"+_speaker.Bio+ "</span><br/>"+ Environment.NewLine);
//			}
//
//			if (!string.IsNullOrEmpty(_speaker.ImageUrl))
//			{
//				sb.Append("<img src='"+_speaker.ImageUrl+ "'/><br/>"+ Environment.NewLine);
//			}
//
//			//TODO: add more metadata to speaker view, if available
//			//TODO: loop through the sessions for this speaker, to link thru
////			foreach (var session in _speaker.Sessions)
////			{
////				sb.Append("<div class='sessionspeaker'><a href='http://mwc.app/" + session.Code + "' class='sessionspeaker'>" + session.Title + "</a></div><br />");
////			}
//			/*
//			if (!string.IsNullOrEmpty(_speaker.BlogUrl))
//				sb.Append("Blog: <a href='"+_speaker.BlogUrl+"'>"+_speaker.BlogUrl+"</a><br/>"+ Environment.NewLine);
//			
//			if (!string.IsNullOrEmpty(_speaker.TwitterName))
//				sb.Append("Twitter: <a href='http://twitter.com/"+_speaker.TwitterName.Replace("@","")+"'>"+_speaker.TwitterName+"</a><br/>"+ Environment.NewLine);
//			*/
//			return sb.ToString();
//		}
	}
}

