using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Foundation;
using UIKit;
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

		public override UITableViewCell GetCell (UITableView tableView, Foundation.NSIndexPath indexPath)
		{
			var speaker = speakers[indexPath.Row];
			var cell = tableView.DequeueReusableCell(cellId);
			if(cell == null) 
				cell = new UITableViewCell (UITableViewCellStyle.Default, cellId);
			
			cell.TextLabel.Text = speaker.Name;
			return cell;
		}

		public override nfloat GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
		{
			return 40f;
		}

		public override nint RowsInSection (UITableView tableview, nint section)
		{
			return speakers.Count;
		}
		
		public override string TitleForHeader (UITableView tableView, nint section)
		{
			return "Speakers";
		}	

		public override void RowSelected (UITableView tableView, Foundation.NSIndexPath indexPath)
		{
			var speaker = speakers[indexPath.Row];
			view.SelectSpeaker(speaker);
			if (AppDelegate.IsPhone) tableView.DeselectRow (indexPath, true);
		}
	}
}
