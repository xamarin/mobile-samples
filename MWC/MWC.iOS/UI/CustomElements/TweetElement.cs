using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.Dialog;
using System.Drawing;
using MWC.BL;
using MWC.SAL;

namespace MWC.iOS.UI.CustomElements
{
	/// <summary>
	/// Renders a Tweet
	/// </summary>
	/// <remarks>
	/// Originally implemented IElementSizing.GetHeight in this class, however
	/// the variable height was not returned after pull-to-refresh (MT.D bug?)
	/// This was fixed by moving the implementation to TwitterScreenSizingSource
	/// </remarks>
	public class TweetElement : Element  
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

		public override void Selected (DialogViewController dvc, UITableView tableView, MonoTouch.Foundation.NSIndexPath path)
		{
			var tds = new MWC.iOS.Screens.iPhone.Twitter.TweetDetailsScreen (Tweet);
			dvc.ActivateController (tds);
		}

	}
}

