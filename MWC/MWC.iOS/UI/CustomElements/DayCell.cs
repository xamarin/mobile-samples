using System;
using Foundation;
using UIKit;
using MonoTouch.Dialog;
using System.Drawing;
using CoreGraphics;

namespace MWC.iOS.UI.CustomElements {
	/// <remarks>
	/// Although not used for a MT.D Element, placed in namespace with the other cells
	/// </remarks>
	public class DayCell: UITableViewCell  {
		UILabel dayLabel;
		UIImageView imageView;
		static UIImage calendarImageBig = UIImage.FromFile (AppDelegate.ImageCalendarPad);
		static UIImage calendarImageSmall = UIImage.FromFile (AppDelegate.ImageCalendarPhone);

		public DayCell (string caption, DateTime day, NSString cellId) : base (UITableViewCellStyle.Default, cellId)
		{
			SelectionStyle = UITableViewCellSelectionStyle.Blue;
			
			BackgroundColor = AppDelegate.ColorCellBackgroundHome; //UIColor.FromRGB (36, 54, 72);

			dayLabel = new UILabel () {
				TextAlignment = UITextAlignment.Left,
				BackgroundColor = UIColor.FromWhiteAlpha (0f, 0f),
				Font = UIFont.FromName("Helvetica-Bold", AppDelegate.Font16pt),
				TextColor = AppDelegate.ColorTextHome, //UIColor.FromRGB (192, 205, 223),
				Lines = 0
			};
			imageView = new UIImageView();
			
			UpdateCell (caption, day);
			
			ContentView.Add (dayLabel);
			ContentView.Add (imageView);
		}
		
		public void UpdateCell (string caption, DateTime day)
		{
			UIImage image;
			if (AppDelegate.IsPad || AppDelegate.HasRetina) // use the big image
				image = MWC.iOS.UI.CustomElements.CustomBadgeElement.MakeCalendarBadge (calendarImageBig, day.ToString ("MMM").ToUpper (), day.ToString ("dd"));
			else // use the small image
				image = MWC.iOS.UI.CustomElements.CustomBadgeElement.MakeCalendarBadgeSmall (calendarImageSmall, day.ToString ("MMM").ToUpper (), day.ToString ("dd"));
			// either way, on iPad it'll be 60 wide, 
			// on iPhone it'll be 30 wide (but if Retina, a 60-wide image will be stuffed in)

			imageView.Image = image;
			dayLabel.Text = caption;
		}
		
		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();
			
			var dayFrame = ContentView.Bounds;
			dayFrame.Height = 25;
			dayFrame.Width = ContentView.Bounds.Width - 90;
			
			if (AppDelegate.IsPad) {
				dayFrame.X = 8 + 57 + 13;
				dayFrame.Y = 23;
				imageView.Frame = new CGRect(8, 8, 59, 58);
			} else { // IsPhone
				dayFrame.X = 8 + 30 + 13;
				dayFrame.Y = 8;
				imageView.Frame = new CGRect(8, 8, 30, 29);
			}
			dayLabel.Frame = dayFrame;
		}
	}
}