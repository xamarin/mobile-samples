using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.Dialog;
using System.Drawing;
using MWC.BL;
using MWC.BL.Managers;

namespace MWC.iOS.UI.CustomElements
{
	public class SessionCell : UITableViewCell 
	{
		static UIFont bigFont = UIFont.BoldSystemFontOfSize (16);
		static UIFont midFont = UIFont.BoldSystemFontOfSize (15);
		static UIFont smallFont = UIFont.SystemFontOfSize (14);
		static UIImage favorite = UIImage.FromFile (AppDelegate.ImageNotFavorite);
		static UIImage favorited = UIImage.FromFile (AppDelegate.ImageIsFavorite);
		UILabel _bigLabel, _smallLabel;
		UIButton _button;
		Session _session;
		const int ImageSpace = 32;
		const int Padding = 8;
		
		public SessionCell (UITableViewCellStyle style, NSString ident, Session session, string big, string small) : base (style, ident)
		{
			SelectionStyle = UITableViewCellSelectionStyle.Blue;
			
			_bigLabel = new UILabel () {
				TextAlignment = UITextAlignment.Left,
				BackgroundColor = UIColor.FromWhiteAlpha (0f, 0f)
			};
			_smallLabel = new UILabel () {
				TextAlignment = UITextAlignment.Left,
				Font = smallFont,
				TextColor = UIColor.DarkGray,
				BackgroundColor = UIColor.FromWhiteAlpha (0f, 0f)
			};
			_button = UIButton.FromType (UIButtonType.Custom);
			_button.TouchDown += delegate {
				UpdateImage (ToggleFavorite ());
			};
			UpdateCell (session, big, small);
			
			ContentView.Add (_bigLabel);
			ContentView.Add (_smallLabel);
			ContentView.Add (_button);
		}
		
		public void UpdateCell (Session session, string big, string small)
		{
			this._session = session;
			UpdateImage (FavoritesManager.IsFavorite (session.Title));
			
			_bigLabel.Font = big.Length > 35 ? midFont : bigFont;
			_bigLabel.Text = big;
			
			_smallLabel.Text = small;
		}
		
		void UpdateImage (bool selected)
		{
			if (selected)				
				_button.SetImage (favorited, UIControlState.Normal);
			else
				_button.SetImage (favorite, UIControlState.Normal);
		}
		
		bool ToggleFavorite ()
		{
			if (FavoritesManager.IsFavorite (_session.Title)){
				FavoritesManager.RemoveFavoriteSession (_session.Title);
				return false;
			} else {
				var fav = new Favorite {SessionID = _session.ID, SessionName = _session.Title};
				FavoritesManager.AddFavoriteSession (fav);
				return true;
			}
		}
		
		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();
			var full = ContentView.Bounds;
			var bigFrame = full;
			
			bigFrame.Height = 22;
			bigFrame.X = Padding;
			bigFrame.Width -= ImageSpace+Padding;
			_bigLabel.Frame = bigFrame;
			
			var smallFrame = full;
			smallFrame.Y = 22;
			smallFrame.Height = 21;
			smallFrame.X = Padding;
			smallFrame.Width = bigFrame.Width;
			_smallLabel.Frame = smallFrame;
			
			_button.Frame = new RectangleF (full.Width-ImageSpace, -3, ImageSpace, 48);
		}
	}
}