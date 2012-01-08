using System;
using System.IO;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Threading;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MWC.SAL;
using MWC.BL;

namespace MWC.iOS.Screens.Common.Twitter
{
	[Obsolete("See MT.D implementation in iPhone folder; although this may be re-instated later")]
	public partial class TwitterDetailViewController : UIViewController
	{
		#region Constructors

		// The IntPtr and initWithCoder constructors are required for controllers that need 
		// to be able to be created from a xib rather than from managed code

		public TwitterDetailViewController (Tweet tweet) : base ()
		{
			Tweet = tweet;
		}
		
		public TwitterDetailViewController (IntPtr handle) : base(handle)
		{
			Initialize ();
		}

		[Export("initWithCoder:")]
		public TwitterDetailViewController (NSCoder coder) : base(coder)
		{
			Initialize ();
		}

		public TwitterDetailViewController () : base("TwitterDetailViewController", null)
		{
			Initialize ();
		}

		void Initialize ()
		{
		}
		
		#endregion
		
		public Tweet Tweet {
			get; set;
		}
		
		public override void ViewDidLoad ()
		{
			tweet.Text = Tweet.Title;
			user.Text = Tweet.Author.Substring (0, Tweet.Author.IndexOf (" "));
			date.Text = Tweet.FormattedTime;
				 
			if (!Directory.Exists (string.Format ("{0}/twitter-images", Environment.GetFolderPath (Environment.SpecialFolder.Personal))))
				Directory.CreateDirectory (string.Format ("{0}/twitter-images", Environment.GetFolderPath (Environment.SpecialFolder.Personal)));
				
			string file = string.Format ("{0}/twitter-images/{1}", Environment.GetFolderPath (Environment.SpecialFolder.Personal), user.Text);
			if (File.Exists (file)) {
				var img = UIImage.FromFile (string.Format ("../Documents/twitter-images/{0}", user.Text));
				if(img != null)
				{
					image.Image = TwitterViewCellController.RemoveSharpEdges (img);
				}
			} else {
				image.Image = null;
				ThreadPool.QueueUserWorkItem (delegate {
					WebClient wc = new WebClient ();
					wc.DownloadFile (Tweet.ImageUrl, file);
					this.InvokeOnMainThread (delegate {
						var img = UIImage.FromFile (string.Format ("../Documents/twitter-images/{0}", user.Text));
						if(img != null)
						{
							image.Image = TwitterViewCellController.RemoveSharpEdges (img);
						}
					});
				});
			}
		}		
	}
}

