using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.Dialog;
using MonoTouch.Dialog.Utilities;
using System.Drawing;
using MWC.BL;
using System.Linq;
using MWC.iOS.Screens.iPad;
using MWC.BL.Managers;

//TODO: favorites, speakerList

namespace MWC.iOS.UI.Controls.Views
{
	/// <summary>
	/// from SessionDetailsScreen    TODO: merge/re-use
	/// </summary>
	public class SessionView : UIView
	{
		UILabel _titleLabel, _timeLabel, _locationLabel;
		UITextView _descriptionTextView;
		UIToolbar _toolbar;
		UIButton _button;
		SessionPopupScreen _hostScreen;
		bool _isPopup = false;

		int y = 0;

		MWC.BL.Session _session;

		//int width = 368;
		const int _buttonSpace = 24;
		static UIImage favorite = UIImage.FromFile (AppDelegate.ImageNotFavorite);
		static UIImage favorited = UIImage.FromFile (AppDelegate.ImageIsFavorite);

		public SessionView (MWC.BL.Session session) : this(session, null)
		{
		}
		public SessionView (MWC.BL.Session session, SessionPopupScreen host)
		{
			_hostScreen = host;
			_isPopup = (_hostScreen != null);
			_session = session;

			this.BackgroundColor = UIColor.White;
			
			if (AppDelegate.IsPad)
			{
				_toolbar = new UIToolbar();//new RectangleF(0, 0, this.Bounds.Width, 40));
				_toolbar.TintColor = UIColor.DarkGray;
				if (_isPopup)
				{
					_toolbar.Items = new UIBarButtonItem[]{
						new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
						new UIBarButtonItem("Session Info", UIBarButtonItemStyle.Plain, null),
						new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
						new UIBarButtonItem("Close", UIBarButtonItemStyle.Done
							, (o,e)=>
								{
									_hostScreen.Dismiss();
								}
						)};
					this.AddSubview (_toolbar);
					y = 40;
				}
			}
			
			_titleLabel = new UILabel () {
				TextAlignment = UITextAlignment.Left,
				Font = UIFont.FromName ("Helvetica-Light", AppDelegate.Font16pt),
				BackgroundColor = UIColor.FromWhiteAlpha (0f, 0f),
				Lines = 0
			};
			_timeLabel = new UILabel () {
				TextAlignment = UITextAlignment.Left,
				Font = UIFont.FromName ("Helvetica-LightOblique", AppDelegate.Font10pt),
				TextColor = UIColor.DarkGray,
				BackgroundColor = UIColor.FromWhiteAlpha (0f, 0f)
			};
			_locationLabel = new UILabel () {
				TextAlignment = UITextAlignment.Left,
				Font = UIFont.FromName ("Helvetica-Light", AppDelegate.Font10pt),
				TextColor = UIColor.DarkGray,
				BackgroundColor = UIColor.FromWhiteAlpha (0f, 0f)
			};

			_descriptionTextView = new UITextView () {
				TextAlignment = UITextAlignment.Left,
				Font = UIFont.FromName ("Helvetica-Light", AppDelegate.Font10_5pt),
				BackgroundColor = UIColor.FromWhiteAlpha (0f, 0f),
				ScrollEnabled = true,
				Editable = false
			};
			_button = UIButton.FromType (UIButtonType.Custom);
			_button.TouchDown += delegate {
				UpdateImage (ToggleFavorite ());
			};
			
			this.AddSubview (_titleLabel);
			this.AddSubview (_timeLabel);
			this.AddSubview (_locationLabel);			
			this.AddSubview (_descriptionTextView);
			this.AddSubview (_button);
		}

		public override void LayoutSubviews ()
		{
			if (_session == null) return;

			Update ();

			var full = this.Bounds;

			if (AppDelegate.IsPhone)
			{	// for now, hardcode iPhone dimensions to reduce regressions
				int topMargin = 10;
				SizeF titleSize = this._titleLabel.StringSize (this._session.Title
								, UIFont.FromName ("Helvetica-Light", AppDelegate.Font16pt)
								, new SizeF (245, 400), UILineBreakMode.WordWrap);
				var bigFrame = full;
				bigFrame.X = 13;
				bigFrame.Y = y + topMargin; // 15 -> 13
				bigFrame.Height = titleSize.Height; //26
				bigFrame.Width -= (13+17);
				_titleLabel.Frame = bigFrame;
				
				var smallFrame = full;
				smallFrame.X = 13+17;
				smallFrame.Y = y + 5 + titleSize.Height;
				smallFrame.Height = 15; // 12 -> 15
				smallFrame.Width -= (13+17);
				_timeLabel.Frame = smallFrame;
				
				smallFrame.Y = smallFrame.Y + smallFrame.Height + 17;
				_locationLabel.Frame = smallFrame;
	
				if (!String.IsNullOrEmpty(_session.Overview))
				{
					SizeF size = _descriptionTextView.StringSize (_session.Overview
										, _descriptionTextView.Font
										, new SizeF (310, 580)
										, UILineBreakMode.WordWrap);
					_descriptionTextView.Frame = new RectangleF(5, y + 115, 310, size.Height);
				}
				else
				{
					_descriptionTextView.Frame = new RectangleF(5, y + 115, 310, 30);
				}
				_button.Frame = new RectangleF (full.Width - _buttonSpace-15
					, y + topMargin + _titleLabel.Frame.Height
					, _buttonSpace
					, _buttonSpace); // just under the title, right of the small text
			}
			else
			{
				_toolbar.Frame = new RectangleF(0, 0, this.Bounds.Width, 40);

				int sideMargin = 13, topMargin = 10;
				SizeF titleSize = this._titleLabel.StringSize (this._session.Title
								, UIFont.FromName ("Helvetica-Light", AppDelegate.Font16pt)
								, new SizeF (full.Width - sideMargin, 400), UILineBreakMode.WordWrap);
				var titleFrame = full;
				titleFrame.X = sideMargin;
				titleFrame.Y = y + topMargin; 
				titleFrame.Height = titleSize.Height; 
				titleFrame.Width -= (sideMargin * 2);
				_titleLabel.Frame = titleFrame;
				
				var smallTextFrame = full;
				smallTextFrame.X = sideMargin;
				smallTextFrame.Y = y + 15 + titleFrame.Height;
				smallTextFrame.Height = 15; 
				smallTextFrame.Width -= (sideMargin * 2);
				_timeLabel.Frame = smallTextFrame;
				
				smallTextFrame.Y = _timeLabel.Frame.Y + _timeLabel.Frame.Height + 10;
				_locationLabel.Frame = smallTextFrame;
	
				var f = new SizeF (full.Width - sideMargin * 2, full.Height - (_locationLabel.Frame.Y + 20));
				if (!String.IsNullOrEmpty(_session.Overview))
				{
					SizeF size = _descriptionTextView.StringSize (_session.Overview
										, _descriptionTextView.Font
										, f
										, UILineBreakMode.WordWrap);
					_descriptionTextView.Frame = new RectangleF(5, _locationLabel.Frame.Y + 15, f.Width, f.Height);
				}
				else
				{
					_descriptionTextView.Frame = new RectangleF(5, _locationLabel.Frame.Y + 15, f.Width, 30);
				}
				_button.Frame = new RectangleF (full.Width - _buttonSpace-15
					, y + topMargin + _titleLabel.Frame.Height
					, _buttonSpace
					, _buttonSpace); // just under the title, right of the small text
			}
		}	
		
		public void Update (int sessionID)
		{
			_session = BL.Managers.SessionManager.GetSession ( sessionID );
			Update ();
			LayoutSubviews ();
		}
		public void Update (MWC.BL.Session session)
		{
			_session = session;
			Update ();
			LayoutSubviews ();
		}

		void Update()
		{
			this._titleLabel.Text = _session.Title;
			this._timeLabel.Text = this._session.Start.ToString("dddd") + " " +
								this._session.Start.ToShortTimeString() + " - " + 
								this._session.End.ToShortTimeString();
			this._locationLabel.Text = _session.Room;

			if (!String.IsNullOrEmpty(_session.Overview))
			{
				this._descriptionTextView.Text = _session.Overview;
				this._descriptionTextView.Font = UIFont.FromName ("Helvetica-Light", AppDelegate.Font10_5pt);
				this._descriptionTextView.TextColor = UIColor.Black;
			}
			else
			{
				this._descriptionTextView.Font = UIFont.FromName ("Helvetica-LightOblique", AppDelegate.Font10_5pt);
				this._descriptionTextView.TextColor = UIColor.Gray;
				this._descriptionTextView.Text = "No background information available.";
			}
			UpdateImage (FavoritesManager.IsFavorite (_session.Key));
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
	}
}