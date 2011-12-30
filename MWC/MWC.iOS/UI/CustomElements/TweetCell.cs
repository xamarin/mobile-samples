using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.Dialog;
using System.Drawing;
using MWC.BL;
using MWC.SAL;
using System.IO;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace MWC.iOS.UI.CustomElements
{
	public class TweetCell : UITableViewCell
	{
		UILabel date, user, tweet;
		UIImageView image;

		static UIFont smallFont = UIFont.SystemFontOfSize (14);
		
		Tweet Tweet;
		const int ImageSpace = 32;
		const int Padding = 8;
		
		public TweetCell (UITableViewCellStyle style, NSString ident, Tweet Tweet) : base (style, ident)
		{
			SelectionStyle = UITableViewCellSelectionStyle.Blue;
			
			date = new UILabel () {
				TextAlignment = UITextAlignment.Right,
				Font = smallFont,
				TextColor = UIColor.DarkGray,
				BackgroundColor = UIColor.FromWhiteAlpha (0f, 0f)
			};
			user = new UILabel () {
				TextAlignment = UITextAlignment.Left,
				Font = smallFont,
				BackgroundColor = UIColor.FromWhiteAlpha (0f, 0f)
			};
			tweet = new UILabel () {
				TextAlignment = UITextAlignment.Left,
				Font = smallFont,
				BackgroundColor = UIColor.FromWhiteAlpha (0f, 0f),
				LineBreakMode = UILineBreakMode.WordWrap,
				Lines = 0
			};
			image = new UIImageView();

			UpdateCell (Tweet);
			
			ContentView.Add (date);
			ContentView.Add (user);
			ContentView.Add (tweet);
		}
		
		public void UpdateCell (Tweet Tweet)
		{
			this.Tweet = Tweet;
			
			user.Text = Tweet.FormattedAuthor;
			date.Text = Tweet.FormattedTime;
			tweet.Text = Tweet.Title;
			
			if (!Directory.Exists (string.Format ("{0}/twitter-images", Environment.GetFolderPath (Environment.SpecialFolder.Personal))))
					Directory.CreateDirectory (string.Format ("{0}/twitter-images", Environment.GetFolderPath (Environment.SpecialFolder.Personal)));
			
			string file = string.Format ("{0}/twitter-images/{1}", Environment.GetFolderPath (Environment.SpecialFolder.Personal), user.Text);
			if (File.Exists (file)) {
				var img = UIImage.FromFile (string.Format ("../Documents/twitter-images/{0}", user.Text));
				if(img != null)
					image.Image = RemoveSharpEdges (img);
			} else {
				image.Image = null;
				ThreadPool.QueueUserWorkItem (delegate {
					WebClient wc = new WebClient ();
					wc.DownloadFile (this.Tweet.ImageUrl, file);
					this.InvokeOnMainThread (delegate {
						var img = UIImage.FromFile (string.Format ("../Documents/twitter-images/{0}", user.Text));
						if(img != null)
							image.Image = RemoveSharpEdges (img);
					});
				});
			}

		}

		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();
			//var full = ContentView.Bounds;

SizeF size = tweet.StringSize (this.Tweet.Title, tweet.Font, 
new SizeF (263, 65), UILineBreakMode.WordWrap);

			user.Frame = new RectangleF(50,0,135,17);
			date.Frame = new RectangleF(193,0,120,17);
			tweet.Frame = new RectangleF(50,15,263,size.Height);
			image.Frame = new RectangleF(7,6,40,39);
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

