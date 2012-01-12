using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.Dialog;
using System.Drawing;
using MWC.BL;

namespace MWC.iOS.UI.CustomElements
{
	// Renders a Speaker
	public class SpeakerElement : Element 
	{
		static NSString key = new NSString ("SpeakerElement");
	
		Speaker Speaker;
		string subtitle;
		
		public SpeakerElement (Speaker Speaker) : base (Speaker.Name)
		{
			this.Speaker = Speaker;
			if(String.IsNullOrEmpty(Speaker.Title))
				subtitle = String.Format ("{0}", Speaker.Company);
			else if (String.IsNullOrEmpty(Speaker.Company))
				subtitle = String.Format("{0}", Speaker.Title);
			else
				subtitle = String.Format ("{0}, {1}", Speaker.Title, Speaker.Company);

		}
		
		static int count;
		public override UITableViewCell GetCell (UITableView tv)
		{
			var cell = tv.DequeueReusableCell (key);
			count++;
			if (cell == null)
			{
				cell = new SpeakerCell (UITableViewCellStyle.Subtitle, key, Speaker, Caption, subtitle);
			}
			else
			{
				((SpeakerCell)cell).UpdateCell (Speaker, Caption, subtitle);
			}
			return cell;
		}

		public override void Selected (DialogViewController dvc, UITableView tableView, MonoTouch.Foundation.NSIndexPath path)
		{
			var sds = new MWC.iOS.Screens.iPhone.Speakers.SpeakerDetailsScreen (Speaker.ID);
			dvc.ActivateController (sds);
		}

	}
}