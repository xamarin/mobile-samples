using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.Dialog;
using System.Drawing;
using MWC.BL;
using MWC.BL.Managers;

namespace MWC.iOS.UI.CustomElements {
	public class SessionCell : UITableViewCell {
		static UIFont bigFont = UIFont.FromName ("Helvetica-Light", AppDelegate.Font16pt);
		static UIFont smallFont = UIFont.FromName ("Helvetica-LightOblique", AppDelegate.Font10pt);
		static UIImage favorite = UIImage.FromFile (AppDelegate.ImageNotFavorite);
		static UIImage favorited = UIImage.FromFile (AppDelegate.ImageIsFavorite);
		UILabel titleLabel, speakerLabel;
		UIButton button;
		Session session;
		const int padding = 13;
		const int buttonSpace = 24;
		
		public SessionCell (UITableViewCellStyle style, NSString ident, Session showSession, string big, string small) : base (style, ident)
		{
			SelectionStyle = UITableViewCellSelectionStyle.Blue;
			
			titleLabel = new UILabel () {
				TextAlignment = UITextAlignment.Left,
				BackgroundColor = UIColor.FromWhiteAlpha (0f, 0f)
			};
			speakerLabel = new UILabel () {
				TextAlignment = UITextAlignment.Left,
				Font = smallFont,
				TextColor = UIColor.DarkGray,
				BackgroundColor = UIColor.FromWhiteAlpha (0f, 0f)
			};
			button = UIButton.FromType (UIButtonType.Custom);
			button.TouchDown += delegate {
				UpdateImage (ToggleFavorite ());
			};
			UpdateCell (showSession, big, small);
			
			ContentView.Add (titleLabel);
			ContentView.Add (speakerLabel);
			ContentView.Add (button);
		}
		
		public void UpdateCell (Session showSession, string big, string small)
		{
			session = showSession;
			UpdateImage (FavoritesManager.IsFavorite (session.Key));
			
			titleLabel.Font = bigFont;
			titleLabel.Text = big;
			
			speakerLabel.Text = small;
		}
		
		void UpdateImage (bool selected)
		{
			if (selected)				
				button.SetImage (favorited, UIControlState.Normal);
			else
				button.SetImage (favorite, UIControlState.Normal);
		}
		
		bool ToggleFavorite ()
		{
			if (FavoritesManager.IsFavorite (session.Key)) {
				FavoritesManager.RemoveFavoriteSession (session.Key);
				return false;
			} else {
				var fav = new Favorite {SessionID = session.ID, SessionKey = session.Key};
				FavoritesManager.AddFavoriteSession (fav);
				return true;
			}
		}
		
		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();
			var full = ContentView.Bounds;
			var titleAdjustment = 0;			
			var titleFrame = full; 

			titleFrame.X = padding;
			titleFrame.Y = 12; //15 ?
			titleFrame.Height = 25;
			titleFrame.Width -= (padding + buttonSpace + 10);

			SizeF size = titleLabel.StringSize (titleLabel.Text
						, this.titleLabel.Font
						, new SizeF(titleFrame.Width, 400));
			if (size.Height > 27) {
				titleAdjustment = 27;
				titleFrame.Height = titleFrame.Height + titleAdjustment; //size.Height;
				titleLabel.Lines = 2;
			}
			else titleLabel.Lines = 1;

			titleLabel.Frame = titleFrame;
			
			var companyFrame = full;
			companyFrame.X = padding;
			companyFrame.Y = 15 + 23 + titleAdjustment;
			companyFrame.Height = 14; // 12 -> 14
			companyFrame.Width = titleFrame.Width;
			speakerLabel.Frame = companyFrame;
			
			button.Frame = new RectangleF (full.Width-buttonSpace-5
				, 10 + titleAdjustment / 2, buttonSpace, buttonSpace);
		}
	}
}