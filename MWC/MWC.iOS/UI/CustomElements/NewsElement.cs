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
		static NSString key = new NSString ("NewsElement");
		UIImage _image;
		RSSEntry _entry;
		
		public NewsElement (RSSEntry entry, UIImage image) : base (entry.Title)
		{
			_entry = entry;
			_image = image;
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
			dvc.ActivateController (sds);
		}
	}
}