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
		static UIFont bigFont = UIFont.FromName ("Helvetica-Light", AppDelegate.Font16pt);
		static UIFont smallFont = UIFont.FromName ("Helvetica-LightOblique", AppDelegate.Font10pt);
		static UIImage favorite = UIImage.FromFile (AppDelegate.ImageNotFavorite);
		static UIImage favorited = UIImage.FromFile (AppDelegate.ImageIsFavorite);
		UILabel _titleLabel, _speakerLabel;
		UIButton _button;
		Session _session;
		const int _padding = 13;
		const int _buttonSpace = 24;
		
		public SessionCell (UITableViewCellStyle style, NSString ident, Session session, string big, string small) : base (style, ident)
		{
			SelectionStyle = UITableViewCellSelectionStyle.Blue;
			
			_titleLabel = new UILabel () {
				TextAlignment = UITextAlignment.Left,
				BackgroundColor = UIColor.FromWhiteAlpha (0f, 0f)
			};
			_speakerLabel = new UILabel () {
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
			
			ContentView.Add (_titleLabel);
			ContentView.Add (_speakerLabel);
			ContentView.Add (_button);
		}
		
		public void UpdateCell (Session session, string big, string small)
		{
			this._session = session;
			UpdateImage (FavoritesManager.IsFavorite (session.Key));
			
			_titleLabel.Font = bigFont;
			_titleLabel.Text = big;
			
			_speakerLabel.Text = small;
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
			if (FavoritesManager.IsFavorite (_session.Key)){
				FavoritesManager.RemoveFavoriteSession (_session.Key);
				return false;
			} else {
				var fav = new Favorite {SessionID = _session.ID, SessionKey = _session.Key};
				FavoritesManager.AddFavoriteSession (fav);
				return true;
			}
		}
		
		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();
			var full = ContentView.Bounds;
			
			var titleFrame = full;
			titleFrame.X = _padding;
			titleFrame.Y = 12; //15 ?
			titleFrame.Height = 25; // 23 -> 25
			titleFrame.Width -= (_padding + _buttonSpace + 10);
			_titleLabel.Frame = titleFrame;
			
			var companyFrame = full;
			companyFrame.X = _padding;
			companyFrame.Y = 15 + 23;
			companyFrame.Height = 14; // 12 -> 14
			companyFrame.Width = titleFrame.Width;
			_speakerLabel.Frame = companyFrame;
			
			_button.Frame = new RectangleF (full.Width-_buttonSpace-5
				, 10, _buttonSpace, _buttonSpace);
		}
	}
}