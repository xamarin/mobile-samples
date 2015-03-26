using System;
using Foundation;
using UIKit;
using MonoTouch.Dialog;
using System.Drawing;
using MWC.BL;

namespace MWC.iOS.UI.CustomElements {
	/// <summary>
	/// Originally used Dialog BadgeElement but created 
	/// this custom element to fix layout issues I was having
	/// </summary>
	public class NewsElement : Element {
		MWC.iOS.Screens.iPad.News.NewsSplitView splitView;
		static NSString key = new NSString ("NewsElement");
		UIImage image;
		RSSEntry entry;
		
		public NewsElement (RSSEntry showEntry, UIImage showImage) : base (showEntry.Title)
		{
			entry = showEntry;
			image = showImage;
		}

		/// <summary>
		/// for iPad (SplitViewController)
		/// </summary>
		public NewsElement (RSSEntry showEntry, UIImage showImage, MWC.iOS.Screens.iPad.News.NewsSplitView newsSplitView) : base (showEntry.Title)
		{
			entry = showEntry;
			image = showImage;
			splitView = newsSplitView;	// could be null, in current implementation
		}

		public override UITableViewCell GetCell (UITableView tv)
		{
			var cell = tv.DequeueReusableCell (key);
			
			if (cell == null) {
				cell = new NewsCell (UITableViewCellStyle.Default, key, entry.Title, image);
			} else {
				((NewsCell)cell).UpdateCell (entry.Title, image);
			}
			return cell;
		}

		public override void Selected (DialogViewController dvc, UITableView tableView, Foundation.NSIndexPath path)
		{
			var sds = new MWC.iOS.Screens.Common.News.NewsDetailsScreen(entry);
			
			if (splitView != null)
				splitView.ShowNews(entry.ID, sds);
			else
				dvc.ActivateController (sds);
		}
	}
}