using System;
using System.Drawing;
using MonoTouch.UIKit;

namespace MWC.iOS {
	public class EmptyOverlay : UIView {
		UILabel emptyLabel;
		public EmptyOverlay (RectangleF frame, string caption) : base (frame)
		{
			// configurable bits
			BackgroundColor = UIColor.LightGray;
			AutoresizingMask = UIViewAutoresizing.FlexibleDimensions;

			float labelHeight = 22;
			float labelWidth = Frame.Width - 20;
			
			// derive the center x and y
			float centerX = Frame.Width / 2;
			float centerY = Frame.Height / 2;
			
			// create and configure the "Loading Data" label
			emptyLabel = new UILabel(new RectangleF (
				centerX - (labelWidth / 2),
				centerY + 5 ,
				labelWidth ,
				labelHeight
				));
			emptyLabel.BackgroundColor = UIColor.Clear;
			emptyLabel.TextColor = UIColor.White;
			emptyLabel.Font = UIFont.FromName ("Helvetica-Light",AppDelegate.Font16pt);
			emptyLabel.Text = caption;
			emptyLabel.TextAlignment = UITextAlignment.Center;

			emptyLabel.AutoresizingMask = UIViewAutoresizing.FlexibleMargins;

			AddSubview (emptyLabel);
		}
		/// <summary>
		/// Static helper to show the 'empty overlay' if a business object is null
		/// </summary>
		/// <returns>
		/// True if it was required, false if not (ie. the business object is NOT NULL)
		/// </returns>
		public static bool ShowIfRequired (ref EmptyOverlay emptyOverlay, object toShow, UIView view, string caption) {
			if (toShow == null) {
				if (emptyOverlay == null) {
					emptyOverlay = new EmptyOverlay(view.Bounds, caption);
					view.AddSubview (emptyOverlay);
				}
				return true;
			} else{
				if (emptyOverlay != null) {
					emptyOverlay.RemoveFromSuperview ();
					emptyOverlay = null;
				}
			}
			return false;
		}
	}
}