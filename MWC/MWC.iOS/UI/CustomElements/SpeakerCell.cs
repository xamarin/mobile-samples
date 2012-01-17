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
		static UIFont bigFont = UIFont.FromName("Helvetica-Light", AppDelegate.Font16pt);
		static UIFont smallFont = UIFont.FromName("Helvetica-LightOblique", AppDelegate.Font10pt);
		UILabel _nameLabel, _companyLabel;
		UIImageView image;

		const int ImageSpace = 44;
		const int Padding = 8;
		
		public SpeakerCell (UITableViewCellStyle style, NSString ident, Speaker Speaker) : base (style, ident)
		{
			SelectionStyle = UITableViewCellSelectionStyle.Blue;
			
			_nameLabel = new UILabel () {
				TextAlignment = UITextAlignment.Left,
				Font = bigFont,
				BackgroundColor = UIColor.FromWhiteAlpha (0f, 0f)
			};
			_companyLabel = new UILabel () {
				TextAlignment = UITextAlignment.Left,
				Font = smallFont,
				TextColor = UIColor.DarkGray,
				BackgroundColor = UIColor.FromWhiteAlpha (0f, 0f)
			};

			image = new UIImageView();

			UpdateCell (Speaker);
			
			ContentView.Add (_nameLabel);
			ContentView.Add (_companyLabel);
			ContentView.Add (image);
		}
		
		public void UpdateCell (Speaker speaker)
		{
			_nameLabel.Text = speaker.Name;
			string subtitle = "";
			if(String.IsNullOrEmpty(speaker.Title))
				subtitle = String.Format ("{0}", speaker.Company);
			else if (String.IsNullOrEmpty(speaker.Company))
				subtitle = String.Format("{0}", speaker.Title);
			else
				subtitle = String.Format ("{0}, {1}", speaker.Title, speaker.Company);

			_companyLabel.Text = subtitle;
			
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
			bigFrame.Y = 13; // 15 -> 13
			bigFrame.Height = 23;
			bigFrame.Width -= (ImageSpace+Padding+Padding);
			_nameLabel.Frame = bigFrame;
			
			var smallFrame = full;
			smallFrame.X = ImageSpace+Padding+Padding+5;
			smallFrame.Y = 15 + 23;
			smallFrame.Height = 15; // 12 -> 15
			smallFrame.Width -= (ImageSpace+Padding+Padding);
			_companyLabel.Frame = smallFrame;

			image.Frame = new RectangleF(8,8,44,44);
		}
		
		public void UpdatedImage (Uri uri)
		{
			//Console.WriteLine("UPDATED:" + uri.AbsoluteUri);
			image.Image = ImageLoader.DefaultRequestImage(uri, this);
		}
	}	
}