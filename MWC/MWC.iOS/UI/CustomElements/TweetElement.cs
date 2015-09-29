using System;
using Foundation;
using UIKit;
using MonoTouch.Dialog;
using System.Drawing;
using MWC.BL;
using MWC.SAL;

namespace MWC.iOS.UI.CustomElements {
	/// <summary>
	/// Renders a Tweet
	/// </summary>
	/// <remarks>
	/// Originally implemented IElementSizing.GetHeight in this class, however
	/// the variable height was not returned after pull-to-refresh (MT.D bug?)
	/// This was fixed by moving the implementation to TwitterScreenSizingSource
	/// </remarks>
	public class TweetElement : Element  {
		static NSString cellId = new NSString ("TweetElement");
	
		Tweet tweet;
		MWC.iOS.Screens.iPad.Twitter.TwitterSplitView splitView;

		/// <summary>
		/// for iPhone
		/// </summary>
		public TweetElement (Tweet showTweet) : base (showTweet.Author)
		{
			tweet = showTweet;
		}
		/// <summary>
		/// for iPad (SplitViewController)
		/// </summary>
		public TweetElement (Tweet showTweet, MWC.iOS.Screens.iPad.Twitter.TwitterSplitView twitterSplitView) : base (showTweet.Author)
		{
			tweet = showTweet;
			splitView = twitterSplitView;	// could be null, in current implementation
		}

		public override UITableViewCell GetCell (UITableView tv)
		{
			var cell = tv.DequeueReusableCell (cellId);
			if (cell == null)
				cell = new TweetCell (UITableViewCellStyle.Subtitle, cellId, tweet);
			else
				((TweetCell)cell).UpdateCell (tweet);

			return cell;
		}

		public override void Selected (DialogViewController dvc, UITableView tableView, Foundation.NSIndexPath path)
		{
			var tds = new MWC.iOS.Screens.iPhone.Twitter.TweetDetailsScreen (tweet);

			if(dvc.NavigationController != null)
				dvc.NavigationController.NavigationBar.Translucent = false;

			if (splitView != null)
				splitView.ShowTweet(tweet.ID, tds);
			else
				dvc.ActivateController (tds);
		}
	}
}