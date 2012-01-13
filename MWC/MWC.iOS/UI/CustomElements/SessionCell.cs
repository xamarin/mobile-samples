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
		static UIFont bigFont = UIFont.FromName ("Helvetica-Light", 16f);
		static UIFont smallFont = UIFont.FromName ("Helvetica-LightOblique", 14);
		static UIImage favorite = UIImage.FromFile (AppDelegate.ImageNotFavorite);
		static UIImage favorited = UIImage.FromFile (AppDelegate.ImageIsFavorite);
		UILabel _titleTextView, _speakerTextView;
		UIButton _button;
		Session _session;
		const int Padding = 13;
		const int _buttonSpace = 24;
		
		public SessionCell (UITableViewCellStyle style, NSString ident, Session session, string big, string small) : base (style, ident)
		{
			SelectionStyle = UITableViewCellSelectionStyle.Blue;
			
			_titleTextView = new UILabel () {
				TextAlignment = UITextAlignment.Left,
				BackgroundColor = UIColor.FromWhiteAlpha (0f, 0f)
			};
			_speakerTextView = new UILabel () {
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
			
			ContentView.Add (_titleTextView);
			ContentView.Add (_speakerTextView);
			ContentView.Add (_button);
		}
		
		public void UpdateCell (Session session, string big, string small)
		{
			this._session = session;
			UpdateImage (FavoritesManager.IsFavorite (session.Title));
			
			_titleTextView.Font = bigFont;
			_titleTextView.Text = big;
			
			_speakerTextView.Text = small;
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
			bigFrame.X = Padding;
			bigFrame.Y = 12; //15 ?
			bigFrame.Height = 23;
			bigFrame.Width -= (Padding + _buttonSpace + 10);
			_titleTextView.Frame = bigFrame;
			
			var smallFrame = full;
			smallFrame.X = Padding;
			smallFrame.Y = 15 + 23;
			smallFrame.Height = 12;
			smallFrame.Width = bigFrame.Width;
			_speakerTextView.Frame = smallFrame;
			
			_button.Frame = new RectangleF (full.Width-_buttonSpace-5
				, 10, _buttonSpace, _buttonSpace);
		}
	}
}