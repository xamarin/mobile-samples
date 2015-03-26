using System;
using System.Drawing;
using MonoTouch.Dialog.Utilities;
using Foundation;
using UIKit;
using MWC.BL;
using CoreGraphics;

namespace MWC.iOS.UI.CustomElements {
	/// <remarks>
	/// Uses ImageLoader from Dialog
	/// https://github.com/migueldeicaza/Dialog/blob/master/Dialog/Utilities/ImageLoader.cs
	/// </remarks>
	public class SpeakerCell : UITableViewCell, IImageUpdated {
		static UIFont bigFont = UIFont.FromName("Helvetica-Light", AppDelegate.Font16pt);
		static UIFont smallFont = UIFont.FromName("Helvetica-LightOblique", AppDelegate.Font10pt);
		UILabel nameLabel, companyLabel;
		UIImageView image;

		const int imageSpace = 44;
		const int padding = 8;
		
		public SpeakerCell (UITableViewCellStyle style, NSString ident, Speaker showSpeaker) : base (style, ident)
		{
			SelectionStyle = UITableViewCellSelectionStyle.Blue;
			
			nameLabel = new UILabel () {
				TextAlignment = UITextAlignment.Left,
				Font = bigFont,
				BackgroundColor = UIColor.FromWhiteAlpha (0f, 0f)
			};
			companyLabel = new UILabel () {
				TextAlignment = UITextAlignment.Left,
				Font = smallFont,
				TextColor = UIColor.DarkGray,
				BackgroundColor = UIColor.FromWhiteAlpha (0f, 0f)
			};

			image = new UIImageView();

			UpdateCell (showSpeaker);
			
			ContentView.Add (nameLabel);
			ContentView.Add (companyLabel);
			ContentView.Add (image);
		}
		
		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();
			var full = ContentView.Bounds;
			var bigFrame = full;
			
			bigFrame.X = imageSpace+padding+padding+5;
			bigFrame.Y = 13; // 15 -> 13
			bigFrame.Height = 23;
			bigFrame.Width -= (imageSpace+padding+padding);
			nameLabel.Frame = bigFrame;
			
			var smallFrame = full;
			smallFrame.X = imageSpace+padding+padding+5;
			smallFrame.Y = 15 + 23;
			smallFrame.Height = 15; // 12 -> 15
			smallFrame.Width -= (imageSpace+padding+padding);
			companyLabel.Frame = smallFrame;

			image.Frame = new CGRect(8,8,44,44);
		}
		
		public void UpdateCell (Speaker speaker)
		{
			nameLabel.Text = speaker.Name;
			string subtitle = "";
			if (String.IsNullOrEmpty (speaker.Title))
				subtitle = String.Format ("{0}", speaker.Company);
			else if (String.IsNullOrEmpty(speaker.Company))
				subtitle = String.Format("{0}", speaker.Title);
			else
				subtitle = String.Format ("{0}, {1}", speaker.Title, speaker.Company);

			companyLabel.Text = subtitle;
			
			if (speaker.ImageUrl != "http://www.mobileworldcongress.com") {
				var u = new Uri(speaker.ImageUrl);
				image.Image = ImageLoader.DefaultRequestImage(u,this);
			}
		}

		public void UpdatedImage (Uri uri)
		{
			image.Image = ImageLoader.DefaultRequestImage(uri, this);
		}
	}	
}