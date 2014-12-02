using System;
using System.Drawing;
using UIKit;
using CoreGraphics;

namespace MWC.iOS {
	public enum EmptyOverlayType {
		  Exhibitor
		, News
		, Session
		, Speaker
		, Twitter
	}

	public class EmptyOverlay : UIView {
		UILabel emptyLabel;
		UIImageView emptyImageView;

		public EmptyOverlay (CGRect frame, string caption, EmptyOverlayType type) : base (frame)
		{
			// configurable bits
			BackgroundColor = UIColor.LightGray;
			AutoresizingMask = UIViewAutoresizing.FlexibleDimensions;
			var imageFilename = "";
			switch (type) {
			case EmptyOverlayType.Exhibitor: imageFilename = AppDelegate.ImageEmptyExhibitors; break;
			case EmptyOverlayType.News:      imageFilename = AppDelegate.ImageEmptyNews;    break;
			case EmptyOverlayType.Session:   imageFilename = AppDelegate.ImageEmptySession; break;
			case EmptyOverlayType.Speaker:   imageFilename = AppDelegate.ImageEmptySpeaker; break;
			case EmptyOverlayType.Twitter:   imageFilename = AppDelegate.ImageEmptyTwitter; break;
			default: imageFilename = AppDelegate.ImageEmptySession; break;
			}
			var img = UIImage.FromFile (imageFilename);

			float labelHeight = 22;
			nfloat labelWidth = Frame.Width - 20;
			
			// derive the center x and y
			nfloat centerX = Frame.Width / 2;
			nfloat centerY = Frame.Height / 2;
			
			emptyImageView = new UIImageView (new CGRect(
				centerX - (img.Size.Width / 2),
				centerY - img.Size.Height - 25,
				img.Size.Width ,
				img.Size.Height
			));
			emptyImageView.Image = img;
			emptyImageView.AutoresizingMask = UIViewAutoresizing.FlexibleMargins;


			// create and configure the "Loading Data" label
			emptyLabel = new UILabel(new CGRect (
				centerX - (labelWidth / 2),
				centerY + 25,
				labelWidth ,
				labelHeight
				));
			emptyLabel.BackgroundColor = UIColor.Clear;
			emptyLabel.TextColor = UIColor.FromRGB(136, 136, 136); //UIColor.White;
			emptyLabel.Font = UIFont.FromName ("Helvetica-Light",AppDelegate.Font16pt);
			emptyLabel.Text = caption;
			emptyLabel.TextAlignment = UITextAlignment.Center;
			emptyLabel.AutoresizingMask = UIViewAutoresizing.FlexibleMargins;
			
			AddSubview (emptyImageView);
			AddSubview (emptyLabel);
		}
		/// <summary>
		/// Static helper to show the 'empty overlay' if a business object is null
		/// </summary>
		/// <returns>
		/// True if it was required, false if not (ie. the business object is NOT NULL)
		/// </returns>
		public static bool ShowIfRequired (ref EmptyOverlay emptyOverlay
						, object toShow
						, UIView view
						, string caption
						, EmptyOverlayType type) {
			if (toShow == null) {
				if (emptyOverlay == null) {
					emptyOverlay = new EmptyOverlay(view.Bounds, caption, type);
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