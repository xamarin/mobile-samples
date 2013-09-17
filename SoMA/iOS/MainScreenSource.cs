using System;
using MonoTouch.UIKit;
using System.Collections.Generic;
using Core;
using MonoTouch.Foundation;

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
		public override int RowsInSection (UITableView tableview, int section)
		{
			return items.Count;
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