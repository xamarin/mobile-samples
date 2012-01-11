using System;
using System.Drawing;
using MonoTouch.Dialog.Utilities;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MWC.BL;

namespace MWC.iOS.UI.CustomElements
{
	/// <remarks>
	/// Uses ImageLoader from MonoTouch.Dialog
	/// https://github.com/migueldeicaza/MonoTouch.Dialog/blob/master/MonoTouch.Dialog/Utilities/ImageLoader.cs
	/// </remarks>
	public class SpeakerCell : UITableViewCell, IImageUpdated
	{
		static UIFont bigFont = UIFont.FromName("Helvetica-Light", 16f);
		static UIFont smallFont = UIFont.FromName("Helvetica-Light", 10f);
		UILabel bigLabel, smallLabel;
		UIImageView image;

		const int ImageSpace = 44;
		const int Padding = 8;
		
		public SpeakerCell (UITableViewCellStyle style, NSString ident, Speaker Speaker, string big, string small) : base (style, ident)
		{
			SelectionStyle = UITableViewCellSelectionStyle.Blue;
			
			bigLabel = new UILabel () {
				TextAlignment = UITextAlignment.Left,
				BackgroundColor = UIColor.FromWhiteAlpha (0f, 0f)
			};
			smallLabel = new UILabel () {
				TextAlignment = UITextAlignment.Left,
				Font = smallFont,
				TextColor = UIColor.DarkGray,
				BackgroundColor = UIColor.FromWhiteAlpha (0f, 0f)
			};

			image = new UIImageView();

			UpdateCell (Speaker, big, small);
			
			ContentView.Add (bigLabel);
			ContentView.Add (smallLabel);
			ContentView.Add (image);
		}
		
		public void UpdateCell (Speaker speaker, string big, string small)
		{
			bigLabel.Text = big;
			smallLabel.Text = small;
			
			if (speaker.ImageUrl != "http://www.mobileworldcongress.com")
			{
				//Console.WriteLine("INITIAL:" + speaker.ImageUrl);
				var u = new Uri(speaker.ImageUrl);
				image.Image = ImageLoader.DefaultRequestImage(u,this);
			}
		}

		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();
			var full = ContentView.Bounds;
			var bigFrame = full;
			
			bigFrame.X = ImageSpace+Padding+Padding+5;
			bigFrame.Y = 15;
			bigFrame.Height = 23;
			bigFrame.Width -= (ImageSpace+Padding+Padding);
			bigLabel.Frame = bigFrame;
			
			var smallFrame = full;
			smallFrame.X = ImageSpace+Padding+Padding+5;
			smallFrame.Y = 15 + 23;
			smallFrame.Height = 12;
			smallFrame.Width -= (ImageSpace+Padding+Padding);
			smallLabel.Frame = smallFrame;

			image.Frame = new RectangleF(8,8,44,44);
		}
		
		public void UpdatedImage (Uri uri)
		{
			//Console.WriteLine("UPDATED:" + uri.AbsoluteUri);
			image.Image = ImageLoader.DefaultRequestImage(uri, this);
		}
	}	
}