using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MWC.BL;

namespace MWC.iOS {
	public class SpeakersTableSource : UITableViewSource {
		IList<Speaker> speakers;
		MWC.iOS.UI.Controls.Views.SessionView view;
		static NSString cellId = new NSString("SpeakerCell");

		public SpeakersTableSource (List<Speaker> speakers, MWC.iOS.UI.Controls.Views.SessionView view)
		{
			this.speakers = speakers;
			this.view = view;
		}

		public override UITableViewCell GetCell (UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
		{
			var speaker = speakers[indexPath.Row];
			var cell = tableView.DequeueReusableCell(cellId);
			if(cell == null) 
				cell = new UITableViewCell (UITableViewCellStyle.Default, cellId);
			
			cell.TextLabel.Text = speaker.Name;
			return cell;
		}

		public override float GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
		{
			return 40f;
		}

		public override int RowsInSection (UITableView tableview, int section)
		{
			return speakers.Count;
		}
		
		public override string TitleForHeader (UITableView tableView, int section)
		{
			return "Speakers";
		}

		public override int NumberOfSections (UITableView tableView)
		{
			return 1;
		}	
		
//		public override UIView GetViewForHeader (UITableView tableView, int section)
//		{
//			if (AppDelegate.IsPhone) return null;
//			var headerText = groupedFavorites[section].Timeslot;
//			if (section == 0) headerText = "Favorites for " + headerText;
//			return DaysTableSource.BuildSectionHeaderView(headerText);
//		}

		public override void RowSelected (UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
		{
			var speaker = speakers[indexPath.Row];
			view.SelectSpeaker(speaker);
			//tableView.DeselectRow (indexPath, true);
		}
	}
}
