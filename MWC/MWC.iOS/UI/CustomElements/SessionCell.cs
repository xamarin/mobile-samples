using System;
using Foundation;
using UIKit;
using MonoTouch.Dialog;
using System.Drawing;
using MWC.BL;
using MWC.BL.Managers;
using CoreGraphics;

namespace MWC.iOS.UI.CustomElements {
	public class SessionCell : UITableViewCell {
		static UIFont bigFont = UIFont.FromName ("Helvetica-Light", AppDelegate.Font16pt);
		static UIFont smallFont = UIFont.FromName ("Helvetica-LightOblique", AppDelegate.Font10pt);
		static UIImage favorite = UIImage.FromFile (AppDelegate.ImageNotFavorite);
		static UIImage favorited = UIImage.FromFile (AppDelegate.ImageIsFavorite);
		static UIImage building = UIImage.FromFile (AppDelegate.ImageLocation);
		UILabel titleLabel, speakerLabel;
		UIButton button;
		UIImageView locationImageView;
		Session session;
		const int padding = 13;
		const int buttonSpace = 45; //24;
		
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
			locationImageView = new UIImageView();
			locationImageView.Image = building;

			button = UIButton.FromType (UIButtonType.Custom);
			button.TouchDown += delegate {
				UpdateImage (ToggleFavorite ());
				if (AppDelegate.IsPad) {
					NSObject o = new NSObject();
					NSDictionary progInfo = NSDictionary.FromObjectAndKey(o, new NSString("FavUpdate"));

					NSNotificationCenter.DefaultCenter.PostNotificationName(
						"NotificationFavoriteUpdated", o, progInfo);
				}
			};
			UpdateCell (showSession, big, small);
			
			ContentView.Add (titleLabel);
			ContentView.Add (speakerLabel);
			ContentView.Add (button);
			ContentView.Add (locationImageView);
		}
		
		/// <summary>
		/// Update colors/fonts for use on the HomeScreen only
		/// </summary>
		public void StyleForHome ()
		{
			BackgroundColor = AppDelegate.ColorCellBackgroundHome;
			titleLabel.TextColor = AppDelegate.ColorTextHome;
			titleLabel.Font = UIFont.FromName ("Helvetica-Bold", AppDelegate.Font16pt);
			speakerLabel.TextColor = AppDelegate.ColorTextHome;
			speakerLabel.Font = UIFont.FromName ("Helvetica", AppDelegate.Font10_5pt);
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

		/// <summary>
		/// Used in ViewWillAppear (SessionsScreen, SessionDayScheduleScreen) 
		/// to sync favorite-stars that have changed in other views
		/// </summary>		
		public void UpdateFavorite() {
			UpdateImage (FavoritesManager.IsFavorite (session.Key)) ;
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
			titleFrame.Width -= (padding + buttonSpace); // +10

			CGSize size = titleLabel.StringSize (titleLabel.Text
						, titleLabel.Font
						, new CGSize(titleFrame.Width, 400));
			if (size.Height > 27) {
				titleAdjustment = 27;
				titleFrame.Height = titleFrame.Height + titleAdjustment; //size.Height;
				titleLabel.Lines = 2;
			}
			else titleLabel.Lines = 1;

			titleLabel.Frame = titleFrame;
			
			var locationImagePadding = 18 + 5;
			if (speakerLabel.Text == "") {
				locationImagePadding = 0;
				locationImageView.Alpha = 0f;
			} else 
				locationImageView.Alpha = 1f;

			var companyFrame = full;
			companyFrame.X = padding + locationImagePadding;	// image is 
			var bottomRowY = 15 + 23 + titleAdjustment;
			companyFrame.Y = bottomRowY;
			companyFrame.Height = 14; // 12 -> 14
			companyFrame.Width = titleFrame.Width - locationImagePadding;
			speakerLabel.Frame = companyFrame;
			
			locationImageView.Frame = new CGRect (padding, bottomRowY, 18, 16);

			button.Frame = new CGRect (full.Width-buttonSpace //-5
				, titleAdjustment / 2, buttonSpace, buttonSpace); // 10 + 
		}
	}
}