using System;
using UIKit;
using System.Collections.Generic;
using Core;
using Foundation;

namespace SoMA
{
	class MainScreenSource : UITableViewSource {
		List<ShareItem> items;
		public MainScreenSource (List<ShareItem> items) 
		{
			this.items = items;
		}
		public ShareItem GetItem (int row) 
		{
			return items [row];
		}
		public override nint RowsInSection (UITableView tableview, nint section)
		{
			return (nint)items.Count;
		}
		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			var cell = tableView.DequeueReusableCell ("sharecell"); // set in Storyboard

			var i = items [indexPath.Row];
			cell.ImageView.Image = UIImage.FromFile (i.ThumbImagePath);
			cell.TextLabel.Text = i.Text;
			cell.DetailTextLabel.Text = i.SocialType;
			return cell;
		}
	}
}