using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.Dialog;
using System.Drawing;
using MWC.BL;

namespace MWC.iOS.UI.CustomElements
{
	public class SpeakerCell : UITableViewCell {
		static UIFont bigFont = UIFont.BoldSystemFontOfSize (16);
		static UIFont midFont = UIFont.BoldSystemFontOfSize (15);
		static UIFont smallFont = UIFont.SystemFontOfSize (14);
		UILabel bigLabel, smallLabel;
		Speaker Speaker;
		const int ImageSpace = 32;
		const int Padding = 8;
		
		public SpeakerCell (UITableViewCellStyle style, NSString ident, Speaker Speaker, string big, string small) : base (style, ident)
		{
			SelectionStyle = UITableViewCellSelectionStyle.Blue;
			
			bigLabel = new UILabel () {
				TextAlignment = UITextAlignment.Left,
				BackgroundColor = UIColor.FromWhiteAlpha (0f, 0f)
			};
			smallLabel = new UILabel () {
				TextAlignment = UITextAlignment.Left,
				Font = smallFont,
				TextColor = UIColor.DarkGray,
				BackgroundColor = UIColor.FromWhiteAlpha (0f, 0f)
			};
			UpdateCell (Speaker, big, small);
			
			ContentView.Add (bigLabel);
			ContentView.Add (smallLabel);
		}
		
		public void UpdateCell (Speaker Speaker, string big, string small)
		{
			this.Speaker = Speaker;
			
			bigLabel.Font = big.Length > 35 ? midFont : bigFont;
			bigLabel.Text = big;
			
			smallLabel.Text = small;
		}

		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();
			var full = ContentView.Bounds;
			var bigFrame = full;
			
			bigFrame.Height = 22;
			bigFrame.X = Padding;
			bigFrame.Width -= ImageSpace+Padding;
			bigLabel.Frame = bigFrame;
			
			var smallFrame = full;
			smallFrame.Y = 22;
			smallFrame.Height = 21;
			smallFrame.X = Padding;
			smallFrame.Width = bigFrame.Width;
			smallLabel.Frame = smallFrame;
		}
	}
	
	// Renders a Speaker
	public class SpeakerElement : Element {
		static NSString key = new NSString ("SpeakerElement");
	
		Speaker Speaker;
		string subtitle;
		
		public SpeakerElement (Speaker Speaker) : base (Speaker.Title)
		{
			this.Speaker = Speaker;
			if(String.IsNullOrEmpty(Speaker.Title))
				subtitle = String.Format ("{0}", Speaker.Title);
			else if (String.IsNullOrEmpty(Speaker.Company))
				subtitle = String.Format("{0}", Speaker.Company);
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