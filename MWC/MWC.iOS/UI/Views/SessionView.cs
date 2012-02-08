using System;
using System.Drawing;
using MonoTouch.UIKit;
using MWC.BL;
using MWC.BL.Managers;
using MWC.iOS.Screens.iPad;

namespace MWC.iOS.UI.Controls.Views {
	/// <summary>
	/// from SessionDetailsScreen    TODO: merge/re-use
	/// </summary>
	public class SessionView : UIView {

		UILabel titleLabel, timeLabel, locationLabel;
		UITextView descriptionTextView;
		UIToolbar toolbar;
		UIButton button;
		UIButton[] speakerButtons = new UIButton[0]; 
		UITableView speakerTable;

		SessionPopupScreen hostPopup;
		MWC.iOS.Screens.iPad.Sessions.SessionSpeakersMasterDetail hostScreen;
		bool isPopup = false;
		bool isDirty = false;
		int y = 0;
		EmptyOverlay emptyOverlay;		

		MWC.BL.Session showSession;

		const int buttonSpace = 24;
		static UIImage favorite = UIImage.FromFile (AppDelegate.ImageNotFavorite);
		static UIImage favorited = UIImage.FromFile (AppDelegate.ImageIsFavorite);

		public SessionView (MWC.iOS.Screens.iPad.Sessions.SessionSpeakersMasterDetail host) : this(false)
		{
			hostScreen = host;
		}
		public SessionView (SessionPopupScreen host) : this(true)
		{
			hostPopup = host;
			isPopup = (hostPopup != null);
		}
		public SessionView (bool isPopup) 
		{
			this.BackgroundColor = UIColor.White;
			
			if (AppDelegate.IsPad) {
				toolbar = new UIToolbar();
				toolbar.TintColor = UIColor.DarkGray;
				if (isPopup) {
					toolbar.Items = new UIBarButtonItem[]{
						new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
						new UIBarButtonItem("Session Info", UIBarButtonItemStyle.Plain, null),
						new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
						new UIBarButtonItem("Close", UIBarButtonItemStyle.Done
							, (o,e)=>
								{
									hostPopup.Dismiss(isDirty);
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

			if (AppDelegate.IsPhone) {	
				// for now, hardcode iPhone dimensions to reduce regressions
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
	
				if (!String.IsNullOrEmpty(showSession.Overview)) {
					SizeF size = descriptionTextView.StringSize (showSession.Overview
										, descriptionTextView.Font
										, new SizeF (310, 580)
										, UILineBreakMode.WordWrap);
					descriptionTextView.Frame = new RectangleF(5, y + 115, 310, size.Height);
				} else {
					descriptionTextView.Frame = new RectangleF(5, y + 115, 310, 30);
				}
				button.Frame = new RectangleF (full.Width - buttonSpace-15
					, y + topMargin + titleLabel.Frame.Height
					, buttonSpace
					, buttonSpace); // just under the title, right of the small text
			} else {
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
				if (!String.IsNullOrEmpty(showSession.Overview)) {
//					SizeF size = descriptionTextView.StringSize (showSession.Overview
//										, descriptionTextView.Font
//										, f
//										, UILineBreakMode.WordWrap);
					descriptionTextView.Frame = new RectangleF(5, locationLabel.Frame.Y + 15, f.Width, f.Height);
				} else {
					descriptionTextView.Frame = new RectangleF(5, locationLabel.Frame.Y + 15, f.Width, 30);
				}
				button.Frame = new RectangleF (full.Width - buttonSpace-15
					, y + topMargin + titleLabel.Frame.Height
					, buttonSpace
					, buttonSpace); // just under the title, right of the small text
			}
			

//			for (var i = 0; i < speakerButtons.Length; i++) {
//			//foreach (var button in speakerButtons) {	
//				var button = speakerButtons[i];
//				button.RemoveFromSuperview ();
//				button.Dispose ();
//				button = null;
//			}
			
			if (showSession.Speakers != null && showSession.Speakers.Count > 0) {
//				speakerButtons = new UIButton[showSession.Speakers.Count];
//				
//				for (var i = 0; i < showSession.Speakers.Count; i++) {
//					var sp = showSession.Speakers[i];
//					UIButton speakerButton = UIButton.FromType (UIButtonType.RoundedRect);
//					speakerButton.SetTitle(sp.Name, UIControlState.Normal);
//					speakerButton.Frame = new RectangleF (15, full.Height - 40 - (i * 40), 300, 30);
//					speakerButton.TouchUpInside += (sender, e) => {
//						hostScreen.Update(sp);
//					};
//					AddSubview (speakerButton);
//					speakerButtons[i] = speakerButton;
//				}
				
				var frame = new RectangleF(15
								, full.Height - 40 - (showSession.Speakers.Count * 40)
								, 300
								, showSession.Speakers.Count * 40 + 40);
				if (speakerTable == null) {
					speakerTable = new UITableView(frame, UITableViewStyle.Grouped);
					speakerTable.BackgroundColor = UIColor.White;
					var whiteView = new UIView();
					whiteView.BackgroundColor = UIColor.White;
					speakerTable.BackgroundView = whiteView;
					AddSubview (speakerTable);
				} else 
					speakerTable.Frame = frame;
				speakerTable.Source = new SpeakersTableSource(showSession.Speakers, this);

				var df = descriptionTextView.Frame;
				df.Height = df.Height - 40 - (showSession.Speakers.Count * 40);
				descriptionTextView.Frame = df;
			} else {
//				speakerButtons = new UIButton[0];
				if (speakerTable != null) {
					speakerTable.RemoveFromSuperview ();
					speakerTable.Dispose ();
					speakerTable = null;
				}
			}

			
		}	
		public void SelectSpeaker(Speaker speaker) {
			hostScreen.Update(speaker);
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

			if (!String.IsNullOrEmpty(showSession.Overview)) {
				descriptionTextView.Text = showSession.Overview;
				descriptionTextView.Font = UIFont.FromName ("Helvetica-Light", AppDelegate.Font10_5pt);
				descriptionTextView.TextColor = UIColor.Black;
			} else {
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
			isDirty = true;
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