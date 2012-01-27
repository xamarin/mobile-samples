using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.Dialog;
using System.Drawing;
using MWC.BL;

namespace MWC.iOS.UI.CustomElements
{
	/// <summary>
	/// Originally used MonoTouch.Dialog BadgeElement but created 
	/// this custom element to fix layout issues I was having
	/// </summary>
	public class NewsElement : Element 
	{
		MWC.iOS.Screens.iPad.News.NewsSplitView _splitView;
		static NSString key = new NSString ("NewsElement");
		UIImage _image;
		RSSEntry _entry;
		
		public NewsElement (RSSEntry entry, UIImage image) : base (entry.Title)
		{
			_entry = entry;
			_image = image;
		}

		/// <summary>
		/// for iPad (SplitViewController)
		/// </summary>
		public NewsElement (RSSEntry entry, UIImage image, MWC.iOS.Screens.iPad.News.NewsSplitView splitView) : base (entry.Title)
		{
			_entry = entry;
			_image = image;
			_splitView = splitView;	// could be null, in current implementation
		}

		
		public override UITableViewCell GetCell (UITableView tv)
		{
			var cell = tv.DequeueReusableCell (key);
			
			if (cell == null)
			{
				cell = new NewsCell (UITableViewCellStyle.Default, key, _entry.Title, _image);
			}
			else
			{
				((NewsCell)cell).UpdateCell (_entry.Title, _image);
			}
			return cell;
		}

		public override void Selected (DialogViewController dvc, UITableView tableView, MonoTouch.Foundation.NSIndexPath path)
		{
			var sds = new MWC.iOS.Screens.Common.News.NewsDetailsScreen(_entry.Title, _entry.Content); 
			
			if (_splitView != null)
				_splitView.ShowNews(_entry.ID, sds);
			else
				dvc.ActivateController (sds);

		}
	}
}