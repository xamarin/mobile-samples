using System;
using Foundation;
using UIKit;
using MonoTouch.Dialog;
using System.Drawing;
using MWC.BL;
using MWC.BL.Managers;
using CoreGraphics;

namespace MWC.iOS.UI.CustomElements {
	/// <summary>
	/// Originally used Dialog BadgeElement but created 
	/// this custom cell to fix layout issues I was having
	/// </summary>
	public class NewsCell : UITableViewCell  {
		static UIFont bigFont = UIFont.FromName ("Helvetica-Light", AppDelegate.Font16pt);
		
		UILabel captionLabel;
		UIImageView imageView;
		
		public NewsCell (UITableViewCellStyle style, NSString ident, string caption, UIImage image) : base (style, ident)
		{
			SelectionStyle = UITableViewCellSelectionStyle.Blue;
			
			captionLabel = new UILabel () {
				TextAlignment = UITextAlignment.Left,
				BackgroundColor = UIColor.FromWhiteAlpha (0f, 0f),
				Font = bigFont,
				Lines = 0
			};
			imageView = new UIImageView();
			imageView.Image = image;

			UpdateCell (caption, image);
			
			ContentView.Add (captionLabel);
			ContentView.Add (imageView);
		}
		
		public void UpdateCell (string caption, UIImage image)
		{
			captionLabel.Text = caption;
			imageView.Image = image;
		}
		
		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();
			var full = ContentView.Bounds;
			
			// this sizing code repreated in TwitterScreenSizingSource.GetHeightForRow()
			CGSize size = captionLabel.StringSize (captionLabel.Text
								, bigFont
								, new CGSize (full.Width - 90, 400)
								, UILineBreakMode.WordWrap);

			var captionFrame = full;
			captionFrame.X = 8 + 57 + 13;
			captionFrame.Y = 10;
			captionFrame.Height = size.Height;
			captionFrame.Width = full.Width - 90; //230;
			captionLabel.Frame = captionFrame;
			
			imageView.Frame = new CGRect(8, 12, 57, 57);
		}
	}
}