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
		
		public SpeakerElement (Speaker Speaker) : base (Speaker.Name)
		{
			this.Speaker = Speaker;
		}
		
		static int count;
		public override UITableViewCell GetCell (UITableView tv)
		{
			var cell = tv.DequeueReusableCell (key);
			count++;
			if (cell == null)
			{
				cell = new SpeakerCell (UITableViewCellStyle.Subtitle, key, Speaker);
			}
			else
			{
				((SpeakerCell)cell).UpdateCell (Speaker);
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