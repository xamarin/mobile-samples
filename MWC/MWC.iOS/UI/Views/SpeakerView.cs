using System;
using Foundation;
using UIKit;
using MonoTouch.Dialog;
using MonoTouch.Dialog.Utilities;
using System.Drawing;
using MWC.BL;
using CoreGraphics;

namespace MWC.iOS.UI.Controls.Views {
	/// <summary>
	/// Used in:
	///  iPad   * SessionSpeakersMasterDetail
	///         * SpeakerSessionsMasterDetail
	///  NOT used on iPhone ~ see Common.iPhone.SpeakersScreen which dups some of this
	/// </summary>
	public class SpeakerView : UIView, IImageUpdated {
		UILabel nameLabel, titleLabel, companyLabel;
		UITextView bioTextView;
		UIImageView image;
		
		int y = 0;
		int speakerId;
		Speaker showSpeaker;
		EmptyOverlay emptyOverlay;

		const int ImageSpace = 80;		
		
		public SpeakerView (int speakerID)
		{
			speakerId = speakerID;

			BackgroundColor = UIColor.White;
			
			nameLabel = new UILabel () {
				TextAlignment = UITextAlignment.Left,
				Font = UIFont.FromName ("Helvetica-Light", AppDelegate.Font16pt),
				BackgroundColor = UIColor.FromWhiteAlpha (0f, 0f)
			};
			titleLabel = new UILabel () {
				TextAlignment = UITextAlignment.Left,
				Font = UIFont.FromName ("Helvetica-LightOblique", AppDelegate.Font10pt),
				TextColor = UIColor.DarkGray,
				BackgroundColor = UIColor.FromWhiteAlpha (0f, 0f)
			};
			companyLabel = new UILabel () {
				TextAlignment = UITextAlignment.Left,
				Font = UIFont.FromName ("Helvetica-Light", AppDelegate.Font10pt),
				TextColor = UIColor.DarkGray,
				BackgroundColor = UIColor.FromWhiteAlpha (0f, 0f)
			};
			 bioTextView = new UITextView () {
				TextAlignment = UITextAlignment.Left,
				Font = UIFont.FromName ("Helvetica-Light", AppDelegate.Font10_5pt),
				BackgroundColor = UIColor.FromWhiteAlpha (0f, 0f),
				ScrollEnabled = true,
				Editable = false
			};
			image = new UIImageView();

			AddSubview (nameLabel);
			AddSubview (titleLabel);
			AddSubview (companyLabel);
			AddSubview (bioTextView);
			AddSubview (image);	
		}

		public override void LayoutSubviews ()
		{
			if (EmptyOverlay.ShowIfRequired (ref emptyOverlay, showSpeaker, this, "No speaker info", EmptyOverlayType.Speaker)) return;

			var full = Bounds;
			var bigFrame = full;
			
			bigFrame.X = ImageSpace+13+17;
			bigFrame.Y = y + 27; // 15 -> 13
			bigFrame.Height = 26;
			bigFrame.Width -= (ImageSpace+13+17);
			nameLabel.Frame = bigFrame;
			
			var smallFrame = full;
			smallFrame.X = ImageSpace+13+17;
			smallFrame.Y = y + 27+26;
			smallFrame.Height = 15; // 12 -> 15
			smallFrame.Width -= (ImageSpace+13+17);
			titleLabel.Frame = smallFrame;
			
			smallFrame.Y += y + 17;
			companyLabel.Frame = smallFrame;

			image.Frame = new CGRect(13, y + 15, 80, 80);

			if (!String.IsNullOrEmpty(showSpeaker.Bio)) {
				if (AppDelegate.IsPhone) {
					// for now, hardcode iPhone dimensions to reduce regressions
					CGSize size = bioTextView.StringSize (showSpeaker.Bio
										, bioTextView.Font
										, new SizeF (310, 580)
										, UILineBreakMode.WordWrap);
					bioTextView.Frame = new CGRect(5, y + 115, 310, size.Height);
				} else {
					var f = new CGSize (full.Width - 13 * 2, full.Height - (image.Frame.Y + 80 + 20));
//					SizeF size = bioTextView.StringSize (showSpeaker.Bio
//										, bioTextView.Font
//										, f
//										, UILineBreakMode.WordWrap);
					bioTextView.Frame = new CGRect(5, image.Frame.Y + 80 + 10
										, f.Width
										, f.Height);
				}
			} else {
				bioTextView.Frame = new CGRect(5, y + 115, 310, 30);
			}
		}
		
		// for masterdetail
		public void Update(int speakerID)
		{
			speakerId = speakerID;
			showSpeaker = BL.Managers.SpeakerManager.GetSpeaker (speakerId);
			Update ();
			LayoutSubviews ();
		}

		public void Clear()
		{
			showSpeaker = null;
			nameLabel.Text = "";
			titleLabel.Text = "";
			companyLabel.Text = "";
			bioTextView.Text = "";
			image.Image = null;
			LayoutSubviews (); // show the grey 'no speaker' message
		}

		void Update()
		{
			if (showSpeaker == null) {nameLabel.Text ="not found"; return;}
			
			nameLabel.Text = showSpeaker.Name;
			titleLabel.Text = showSpeaker.Title;
			companyLabel.Text = showSpeaker.Company;

			if (!String.IsNullOrEmpty(showSpeaker.Bio)) {
				bioTextView.Text = showSpeaker.Bio;
				bioTextView.Font = UIFont.FromName ("Helvetica-Light", AppDelegate.Font10_5pt);
				bioTextView.TextColor = UIColor.Black;
			} else {
				bioTextView.Font = UIFont.FromName ("Helvetica-LightOblique", AppDelegate.Font10_5pt);
				bioTextView.TextColor = UIColor.Gray;
				bioTextView.Text = "No background information available.";
			}
			if (showSpeaker.ImageUrl != "http://www.mobileworldcongress.com") {
				var u = new Uri(showSpeaker.ImageUrl);
				image.Image = ImageLoader.DefaultRequestImage(u, this);
			}
		}

		public void UpdatedImage (Uri uri)
		{
			image.Image = ImageLoader.DefaultRequestImage(uri, this);
		}
	}
}