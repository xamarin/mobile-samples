using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.Dialog;
using System.Drawing;
using MWC.BL;
using MWC.BL.Managers;

namespace MWC.iOS.UI.CustomElements
{
	/// <summary>
	/// Originally used MonoTouch.Dialog BadgeElement but created 
	/// this custom cell to fix layout issues I was having
	/// </summary>
	public class NewsCell : UITableViewCell 
	{
		static UIFont bigFont = UIFont.FromName ("Helvetica-Light", AppDelegate.Font16pt);
		
		UILabel _captionLabel;
		UIImageView _image;
		
		public NewsCell (UITableViewCellStyle style, NSString ident, string caption, UIImage image) : base (style, ident)
		{
			SelectionStyle = UITableViewCellSelectionStyle.Blue;
			
			_captionLabel = new UILabel () {
				TextAlignment = UITextAlignment.Left,
				BackgroundColor = UIColor.FromWhiteAlpha (0f, 0f),
				Font = bigFont,
				Lines = 0
			};
			_image = new UIImageView();

			UpdateCell (caption, image);
			
			ContentView.Add (_captionLabel);
			ContentView.Add (_image);
		}
		
		public void UpdateCell (string caption, UIImage image)
		{
			_captionLabel.Text = caption;
			_image.Image = image;
		}
		
		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();
			var full = ContentView.Bounds;
			
			// this sizing code repreated in TwitterScreenSizingSource.GetHeightForRow()
			SizeF size = _captionLabel.StringSize (_captionLabel.Text
								, bigFont
								, new SizeF (full.Width - 90, 400)
								, UILineBreakMode.WordWrap);

			var captionFrame = full;
			captionFrame.X = 8 + 57 + 13;
			captionFrame.Y = 10;
			captionFrame.Height = size.Height;
			captionFrame.Width = full.Width - 90; //230;
			_captionLabel.Frame = captionFrame;
			
			_image.Frame = new RectangleF(8, 12, 57, 57);
		}
	}
}