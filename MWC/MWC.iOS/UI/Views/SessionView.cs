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
		UILabel titleLabel, timeLabel, locationLabel;
		UITextView descriptionTextView;
		UIToolbar toolbar;
		UIButton button;
		SessionPopupScreen hostScreen;
		bool isPopup = false;
		int y = 0;
		EmptyOverlay emptyOverlay;		

		MWC.BL.Session showSession;

		const int _buttonSpace = 24;
		static UIImage favorite = UIImage.FromFile (AppDelegate.ImageNotFavorite);
		static UIImage favorited = UIImage.FromFile (AppDelegate.ImageIsFavorite);

		public SessionView () : this(null)
		{
		}
		public SessionView (SessionPopupScreen host)
		{
			hostScreen = host;
			isPopup = (hostScreen != null);

			this.BackgroundColor = UIColor.White;
			
			if (AppDelegate.IsPad)
			{
				toolbar = new UIToolbar();
				toolbar.TintColor = UIColor.DarkGray;
				if (isPopup)
				{
					toolbar.Items = new UIBarButtonItem[]{
						new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
						new UIBarButtonItem("Session Info", UIBarButtonItemStyle.Plain, null),
						new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
						new UIBarButtonItem("Close", UIBarButtonItemStyle.Done
							, (o,e)=>
								{
									hostScreen.Dismiss();
								}
						)};
					this.AddSubview (toolbar);
					y = 40;
				}
			}
			
			titleLabel = new UILabel () {
				TextAlignment = UITextAlignment.Left,
				Font = UIFont.FromName ("Helvetica-Light", AppDelegate.Font16pt),
				BackgroundColor = UIColor.FromWhiteAlpha (0f, 0f),
				Lines = 0
			};
			timeLabel = new UILabel () {
				TextAlignment = UITextAlignment.Left,
				Font = UIFont.FromName ("Helvetica-LightOblique", AppDelegate.Font10pt),
				TextColor = UIColor.DarkGray,
				BackgroundColor = UIColor.FromWhiteAlpha (0f, 0f)
			};
			locationLabel = new UILabel () {
				TextAlignment = UITextAlignment.Left,
				Font = UIFont.FromName ("Helvetica-Light", AppDelegate.Font10pt),
				TextColor = UIColor.DarkGray,
				BackgroundColor = UIColor.FromWhiteAlpha (0f, 0f)
			};

			descriptionTextView = new UITextView () {
				TextAlignment = UITextAlignment.Left,
				Font = UIFont.FromName ("Helvetica-Light", AppDelegate.Font10_5pt),
				BackgroundColor = UIColor.FromWhiteAlpha (0f, 0f),
				ScrollEnabled = true,
				Editable = false
			};
			button = UIButton.FromType (UIButtonType.Custom);
			button.TouchDown += delegate {
				UpdateImage (ToggleFavorite ());
			};
			
			

			AddSubview (titleLabel);
			AddSubview (timeLabel);
			AddSubview (locationLabel);			
			AddSubview (descriptionTextView);
			AddSubview (button);
		}

		public override void LayoutSubviews ()
		{
			if (EmptyOverlay.ShowIfRequired (ref emptyOverlay, showSession, this, "No session info")) return;
			
			var full = Bounds;

			if (AppDelegate.IsPhone)
			{	// for now, hardcode iPhone dimensions to reduce regressions
				int topMargin = 10;
				SizeF titleSize = titleLabel.StringSize (showSession.Title
								, UIFont.FromName ("Helvetica-Light", AppDelegate.Font16pt)
								, new SizeF (245, 400), UILineBreakMode.WordWrap);
				var bigFrame = full;
				bigFrame.X = 13;
				bigFrame.Y = y + topMargin; // 15 -> 13
				bigFrame.Height = titleSize.Height; //26
				bigFrame.Width -= (13+17);
				titleLabel.Frame = bigFrame;
				
				var smallFrame = full;
				smallFrame.X = 13+17;
				smallFrame.Y = y + 5 + titleSize.Height;
				smallFrame.Height = 15; // 12 -> 15
				smallFrame.Width -= (13+17);
				timeLabel.Frame = smallFrame;
				
				smallFrame.Y = smallFrame.Y + smallFrame.Height + 17;
				locationLabel.Frame = smallFrame;
	
				if (!String.IsNullOrEmpty(showSession.Overview))
				{
					SizeF size = descriptionTextView.StringSize (showSession.Overview
										, descriptionTextView.Font
										, new SizeF (310, 580)
										, UILineBreakMode.WordWrap);
					descriptionTextView.Frame = new RectangleF(5, y + 115, 310, size.Height);
				}
				else
				{
					descriptionTextView.Frame = new RectangleF(5, y + 115, 310, 30);
				}
				button.Frame = new RectangleF (full.Width - _buttonSpace-15
					, y + topMargin + titleLabel.Frame.Height
					, _buttonSpace
					, _buttonSpace); // just under the title, right of the small text
			}
			else
			{
				toolbar.Frame = new RectangleF(0, 0, this.Bounds.Width, 40);

				int sideMargin = 13, topMargin = 10;
				SizeF titleSize = titleLabel.StringSize (showSession.Title
								, UIFont.FromName ("Helvetica-Light", AppDelegate.Font16pt)
								, new SizeF (full.Width - sideMargin, 400), UILineBreakMode.WordWrap);
				var titleFrame = full;
				titleFrame.X = sideMargin;
				titleFrame.Y = y + topMargin; 
				titleFrame.Height = titleSize.Height; 
				titleFrame.Width -= (sideMargin * 2);
				titleLabel.Frame = titleFrame;
				
				var smallTextFrame = full;
				smallTextFrame.X = sideMargin;
				smallTextFrame.Y = y + 15 + titleFrame.Height;
				smallTextFrame.Height = 15; 
				smallTextFrame.Width -= (sideMargin * 2);
				timeLabel.Frame = smallTextFrame;
				
				smallTextFrame.Y = timeLabel.Frame.Y + timeLabel.Frame.Height + 10;
				locationLabel.Frame = smallTextFrame;
	
				var f = new SizeF (full.Width - sideMargin * 2, full.Height - (locationLabel.Frame.Y + 20));
				if (!String.IsNullOrEmpty(showSession.Overview))
				{
					SizeF size = descriptionTextView.StringSize (showSession.Overview
										, descriptionTextView.Font
										, f
										, UILineBreakMode.WordWrap);
					descriptionTextView.Frame = new RectangleF(5, locationLabel.Frame.Y + 15, f.Width, f.Height);
				}
				else
				{
					descriptionTextView.Frame = new RectangleF(5, locationLabel.Frame.Y + 15, f.Width, 30);
				}
				button.Frame = new RectangleF (full.Width - _buttonSpace-15
					, y + topMargin + titleLabel.Frame.Height
					, _buttonSpace
					, _buttonSpace); // just under the title, right of the small text
			}
		}	
		
		public void Update (int sessionID)
		{
			showSession = BL.Managers.SessionManager.GetSession (sessionID);
			Update ();
			LayoutSubviews ();
		}
		public void Update (MWC.BL.Session session)
		{
			showSession = session;
			Update ();
			LayoutSubviews ();
		}

		void Update()
		{
			titleLabel.Text = showSession.Title;
			timeLabel.Text = showSession.Start.ToString("dddd") + " " +
								showSession.Start.ToString("H:mm") + " - " + 
								showSession.End.ToString("H:mm");
			locationLabel.Text = showSession.Room;

			if (!String.IsNullOrEmpty(showSession.Overview))
			{
				descriptionTextView.Text = showSession.Overview;
				descriptionTextView.Font = UIFont.FromName ("Helvetica-Light", AppDelegate.Font10_5pt);
				descriptionTextView.TextColor = UIColor.Black;
			}
			else
			{
				descriptionTextView.Font = UIFont.FromName ("Helvetica-LightOblique", AppDelegate.Font10_5pt);
				descriptionTextView.TextColor = UIColor.Gray;
				descriptionTextView.Text = "No background information available.";
			}
			UpdateImage (FavoritesManager.IsFavorite (showSession.Key));
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
			if (FavoritesManager.IsFavorite (showSession.Key)){
				FavoritesManager.RemoveFavoriteSession (showSession.Key);
				return false;
			} else {
				var fav = new Favorite {SessionID = showSession.ID, SessionKey = showSession.Key};
				FavoritesManager.AddFavoriteSession (fav);
				return true;
			}
		}
	}
}