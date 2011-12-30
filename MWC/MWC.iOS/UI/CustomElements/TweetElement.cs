using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.Dialog;
using System.Drawing;
using MWC.BL;
using MWC.SAL;

namespace MWC.iOS.UI.CustomElements
{
	// Renders a Tweet
	public class TweetElement : Element,  IElementSizing
	{
		static NSString key = new NSString ("TweetElement");
	
		Tweet Tweet;
		
		public TweetElement (Tweet Tweet) : base (Tweet.Author)
		{
			this.Tweet = Tweet;
		}
		
		static int count;
		public override UITableViewCell GetCell (UITableView tv)
		{
			var cell = tv.DequeueReusableCell (key);
			count++;
			if (cell == null)
			{
				cell = new TweetCell (UITableViewCellStyle.Subtitle, key, Tweet);
			}
			else
			{
				((TweetCell)cell).UpdateCell (Tweet);
			}
			return cell;
		}
		
		public float GetHeight (UITableView tableView, NSIndexPath indexPath)
		{
			SizeF maxSize = new SizeF (tableView.Bounds.Width-40, float.MaxValue);
			
//			if (this.Accessory != UITableViewCellAccessory.None)
//			maxSize.Width -= 20;
			
			var captionFont = UIFont.SystemFontOfSize (14);
			float height = tableView.StringSize (this.Tweet.Title, captionFont, maxSize, UILineBreakMode.WordWrap).Height;
			
			return height + 15 + 10; // 15 above, 10 below
		}

		public override void Selected (DialogViewController dvc, UITableView tableView, MonoTouch.Foundation.NSIndexPath path)
		{
			var tds = new MWC.iOS.Screens.iPhone.Twitter.TweetDetailsScreen (Tweet);
			dvc.ActivateController (tds);
		}

	}
}

