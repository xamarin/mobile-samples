using System;
using System.Drawing;
using MonoTouch.Dialog.Utilities;
using Foundation;
using UIKit;
using CoreGraphics;

namespace MWC.iOS.UI.CustomElements {
	public class TweetCell : UITableViewCell, IImageUpdated {
		UILabel date, user, handle, tweetLabel;
		UIImageView image;

		BL.Tweet tweet;
		const int ImageSpace = 32;
		const int Padding = 8;
		
		public TweetCell (UITableViewCellStyle style, NSString ident, BL.Tweet Tweet) : base (style, ident)
		{
			SelectionStyle = UITableViewCellSelectionStyle.Blue;
			
			user = new UILabel () {
				TextAlignment = UITextAlignment.Left,
				Font = UIFont.FromName("Helvetica-Light",AppDelegate.Font16pt),
				BackgroundColor = UIColor.FromWhiteAlpha (0f, 0f)
			};
			handle = new UILabel () {
				TextAlignment = UITextAlignment.Left,
				Font = UIFont.FromName("Helvetica-Light",AppDelegate.Font9pt),
				TextColor = UIColor.LightGray,
				BackgroundColor = UIColor.FromWhiteAlpha (0f, 0f)
			};
			date = new UILabel () {
				TextAlignment = UITextAlignment.Right,
				Font = UIFont.FromName("Helvetica-Light",AppDelegate.Font9pt),
				TextColor = UIColor.DarkGray,
				BackgroundColor = UIColor.FromWhiteAlpha (0f, 0f)
			};
			tweetLabel = new UILabel () {
				TextAlignment = UITextAlignment.Left,
				Font = UIFont.FromName("Helvetica-Light",AppDelegate.Font10_5pt),
				BackgroundColor = UIColor.FromWhiteAlpha (0f, 0f),
				LineBreakMode = UILineBreakMode.WordWrap,
				Lines = 0
			};
			image = new UIImageView();
			
			UpdateCell (Tweet);
			
			
			ContentView.Add (user);
			ContentView.Add (handle);
			ContentView.Add (tweetLabel);
			ContentView.Add (image);
			ContentView.Add (date);
		}
		
		public void UpdateCell (BL.Tweet showTweet)
		{
			tweet = showTweet;
			
			handle.Text = tweet.FormattedAuthor;
			user.Text = tweet.RealName;
			date.Text = tweet.FormattedTime;
			tweetLabel.Text = tweet.Title;
			
			var u = new Uri (tweet.ImageUrl);
			var img = ImageLoader.DefaultRequestImage (u,this);
			if(img != null)
				image.Image = RemoveSharpEdges (img);
		}
		
		public void UpdatedImage (Uri uri)
		{
			ConsoleD.WriteLine("UPDATED:" + uri.AbsoluteUri);
			var img = ImageLoader.DefaultRequestImage(uri, this);
			if(img != null)
				image.Image = RemoveSharpEdges (img);
		}

		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();
			// this sizing code repreated in TwitterScreenSizingSource.GetHeightForRow()
			CGSize size = tweetLabel.StringSize (tweet.Title
								, tweetLabel.Font
								, new SizeF (239, 120)
								, UILineBreakMode.WordWrap);
			
			image.Frame  = new CGRect(8,   8,  48, 48);
			user.Frame   = new CGRect(69, 14, 239, 24);
			handle.Frame = new CGRect(69, 39, 239, 14); // 35 -> 39
			tweetLabel.Frame  = new CGRect(69, 57, 239, size.Height);
			date.Frame   = new CGRect(230,2,80,15); // 8 -> 2
		}


		// Prevent accidents by rounding the edges of the image
		internal static UIImage RemoveSharpEdges (UIImage image)
		{
			UIGraphics.BeginImageContext (new SizeF (48, 48));
			var c = UIGraphics.GetCurrentContext ();
			
			c.BeginPath ();
			c.MoveTo (48, 24);
			c.AddArcToPoint (48, 48, 24, 48, 4);
			c.AddArcToPoint (0, 48, 0, 24, 4);
			c.AddArcToPoint (0, 0, 24, 0, 4);
			c.AddArcToPoint (48, 0, 48, 24, 4);
			c.ClosePath ();
			c.Clip ();
			
			image.Draw (new PointF (0, 0));
			var converted = UIGraphics.GetImageFromCurrentImageContext ();
			UIGraphics.EndImageContext ();
			return converted;
		}
	}	
}