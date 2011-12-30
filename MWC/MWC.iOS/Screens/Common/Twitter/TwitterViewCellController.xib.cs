
using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MWC.SAL;
using System.Drawing;

namespace MWC.iOS.Screens.Common.Twitter
{
	[Obsolete("See MT.D implementation in iPhone folder; although this may be re-instated later")]
	public partial class TwitterViewCellController : UIViewController
	{
		#region Constructors

		// The IntPtr and initWithCoder constructors are required for controllers that need 
		// to be able to be created from a xib rather than from managed code

		public TwitterViewCellController (string nib) : base (nib, null)
		{
		}
		
		public TwitterViewCellController (IntPtr handle) : base(handle)
		{
			Initialize ();
		}

		[Export("initWithCoder:")]
		public TwitterViewCellController (NSCoder coder) : base(coder)
		{
			Initialize ();
		}
		
		#endregion

		public TwitterViewCellController () : base("TwitterViewCellController", null)
		{
			Initialize ();
		}

		void Initialize ()
		{
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
		
		public Tweet Tweet {
			set {
				// 263x65 is the size of the tweet label, we do this to vertically align it
				// If you change the xib you need to change it here
				SizeF size = tweet.StringSize (value.Title, tweet.Font, new SizeF (263, 65), UILineBreakMode.WordWrap);
				
				tweet.Text = value.Title;
				tweet.Frame = new RectangleF (tweet.Frame.X, tweet.Frame.Y, tweet.Frame.Width, size.Height);
				
				user.Text = value.Author.Substring (0, value.Author.IndexOf (" "));
				date.Text = value.FormattedTime;
 
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
						wc.DownloadFile (value.ImageUrl, file);
						this.InvokeOnMainThread (delegate {
							var img = UIImage.FromFile (string.Format ("../Documents/twitter-images/{0}", user.Text));
							if(img != null)
								image.Image = RemoveSharpEdges (img);
						});
					});
				}
			}
		}
		
		public UITableViewCell Cell {
			get { return cell; }
		}
	}
}

