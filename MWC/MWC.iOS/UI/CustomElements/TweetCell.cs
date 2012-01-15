using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Threading;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MWC.SAL;

namespace MWC.iOS.UI.CustomElements
{
	public class TweetCell : UITableViewCell
	{
		UILabel date, user, handle, tweetLabel;
		UIImageView image;

		BL.Tweet Tweet;
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
		
		public void UpdateCell (BL.Tweet tweet)
		{
			this.Tweet = tweet;
			
			handle.Text = this.Tweet.FormattedAuthor;
			user.Text = this.Tweet.RealName;
			date.Text = this.Tweet.FormattedTime;
			tweetLabel.Text = this.Tweet.Title;
			
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
					this.InvokeOnMainThread (delegate {
						try {
							WebClient wc = new WebClient ();
							//TODO: fix file-access bug here - is try-catch okay?
							wc.DownloadFile (this.Tweet.ImageUrl, file);
							var img = UIImage.FromFile (string.Format ("../Documents/twitter-images/{0}", user.Text));
							if(img != null)
								image.Image = RemoveSharpEdges (img);
						} catch {}
					});
				});
			}
		}

		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();
			// this sizing code repreated in TwitterScreenSizingSource.GetHeightForRow()
			SizeF size = tweetLabel.StringSize (this.Tweet.Title
								, tweetLabel.Font
								, new SizeF (239, 120)
								, UILineBreakMode.WordWrap);
			
			image.Frame  = new RectangleF(8,   8,  48, 48);
			user.Frame   = new RectangleF(69, 14, 239, 24);
			handle.Frame = new RectangleF(69, 39, 239, 14); // 35 -> 39
			tweetLabel.Frame  = new RectangleF(69, 57, 239, size.Height);
			
			date.Frame   = new RectangleF(230,2,80,15); // 8 -> 2
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