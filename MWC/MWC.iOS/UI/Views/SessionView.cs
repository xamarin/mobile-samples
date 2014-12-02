using System;
using System.Drawing;
using Foundation;
using UIKit;
using MWC.BL;
using MWC.BL.Managers;
using MWC.iOS.Screens.iPad;
using CoreGraphics;

namespace MWC.iOS.UI.Controls.Views {
	/// <summary>Could use an event here, interface was easier to refactor in place</summary>
	public interface ISessionViewHost {
		void SelectSpeaker (Speaker speaker);
	}
	
	/// <summary>
	/// Used in:
	///  iPad   * HomeScreen (via popup)
	///         * SessionSpeakersMasterDetail
	///         * SpeakerSessionsMasterDetail
	///  iPhone * SessionDetailsScreen
	/// </summary>
	public class SessionView : UIView {

		UILabel titleLabel, timeLabel, locationLabel;
		UITextView descriptionTextView;
		UIToolbar toolbar;
		UIButton button;
		UITableView speakerTable;

		SessionPopupScreen hostPopup;
		ISessionViewHost hostScreen;
		bool isDirty = false;
		int y = 0;
		EmptyOverlay emptyOverlay;		

		MWC.BL.Session showSession;

		const int buttonSpace = 45; //24;
		static UIImage favorite = UIImage.FromFile (AppDelegate.ImageNotFavorite);
		static UIImage favorited = UIImage.FromFile (AppDelegate.ImageIsFavorite);
		
		public SessionView (ISessionViewHost host) : this(false)
		{
			hostScreen = host;
		}
		public SessionView (SessionPopupScreen host) : this(true)
		{
			hostPopup = host;
		}
		public SessionView (bool isPopup) 
		{
			this.BackgroundColor = UIColor.White;
			
			if (AppDelegate.IsPad) {
				toolbar = new UIToolbar();
				toolbar.TintColor = AppDelegate.ColorNavBarTint;
				if (isPopup) { // Popup needs to have a toolbar across the top, with a 'close' button
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
					AddSubview (toolbar);
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
				if (AppDelegate.IsPad) {
					NSObject o = new NSObject();
					NSDictionary progInfo = NSDictionary.FromObjectAndKey(o, new NSString("FavUpdate"));

					NSNotificationCenter.DefaultCenter.PostNotificationName(
						"NotificationFavoriteUpdated", o, progInfo);
				}
			};
			
			AddSubview (descriptionTextView);
			AddSubview (titleLabel);
			AddSubview (timeLabel);
			AddSubview (locationLabel);			
			AddSubview (button);
			// speakerTable is added/removed below, if required
		}

		public override void LayoutSubviews ()
		{
			if (EmptyOverlay.ShowIfRequired (ref emptyOverlay, showSession, this, "No session info", EmptyOverlayType.Session)) return;
			
			var full = Bounds;

			if (AppDelegate.IsPad)
				toolbar.Frame = new CGRect(0, 0, this.Bounds.Width, 40);

			int sideMargin = 13, topMargin = 10;
			CGSize titleSize = titleLabel.StringSize (showSession.Title
							, UIFont.FromName ("Helvetica-Light", AppDelegate.Font16pt)
							, new CGSize (full.Width - sideMargin, 400), UILineBreakMode.WordWrap);
			// Session.Title
			var titleFrame = full;
			titleFrame.X = sideMargin;
			titleFrame.Y = y + topMargin; 
			titleFrame.Height = titleSize.Height; 
			titleFrame.Width -= (sideMargin * 2);
			titleLabel.Frame = titleFrame;
			// Session.StartTime, EndTime
			var timeFrame = full;
			timeFrame.X = sideMargin;
			timeFrame.Y = y + 15 + titleFrame.Height;
			timeFrame.Height = 15; 
			timeFrame.Width -= (sideMargin * 2);
			timeLabel.Frame = timeFrame;
			// Session.Room
			timeFrame.Y = timeLabel.Frame.Y + timeLabel.Frame.Height + 10;
			locationLabel.Frame = timeFrame;
			// Session.IsFavorite ~ star (favorites) button
			button.Frame = new CGRect (full.Width - buttonSpace-15
				, y + topMargin + titleLabel.Frame.Height
				, buttonSpace
				, buttonSpace); // just under the title, right of the small text
			// Session.Overview
			// Now determine how big the Overview text is, and adjust sizes accordingly
			// iPad requires scrolling of the TextView
			// iPhone requires the TextView to be expanded to encompass all text
			if (!String.IsNullOrEmpty(showSession.Overview)) {
				if (AppDelegate.IsPad) {
					var f = new CGSize (full.Width - sideMargin * 2, full.Height - (locationLabel.Frame.Y + 20));
					descriptionTextView.Frame = new CGRect(5, locationLabel.Frame.Y + 15, f.Width, f.Height);
					descriptionTextView.ScrollEnabled = true;
				} else {
					var f = new CGSize (290, 4000);
					CGSize size = descriptionTextView.StringSize (showSession.Overview
										, descriptionTextView.Font
										, f); //, UILineBreakMode.WordWrap);
					descriptionTextView.Frame = new CGRect(5
										, locationLabel.Frame.Y + 15
										, size.Width + 10 // hack: measure seems to underestimate
										, size.Height + 50);
					// going to scroll the whole thing!
					descriptionTextView.ScrollEnabled = false;
				}
			} else {
				descriptionTextView.Frame = new CGRect(5, locationLabel.Frame.Y + 15, full.Width - sideMargin * 2, 30);
			}

			nfloat bottomOfTheseControls = descriptionTextView.Frame.Y + descriptionTextView.Frame.Height;

			// now add the Session.Speakers table underneath (if there _are_ speakers)
			// iPad fixes it to the bottom, and makes the Overview TextView smaller (View height is constant)
			// iPhone adds it to the bottom, and makes the View itself longer to fit
			if (shouldShowSpeakers && showSession.Speakers != null && showSession.Speakers.Count > 0) {
				CGRect frame;
				if (AppDelegate.IsPhone) {
					frame = new CGRect(5
									, bottomOfTheseControls
									, 310
									, showSession.Speakers.Count * 40 + 60); // plus 40 for header
				} else {// IsPad, fixed height
					frame = new CGRect(5
									, full.Height - 40 - (showSession.Speakers.Count * 40) - 5 // 5 is for margin
									, 310
									, showSession.Speakers.Count * 40 + 60); // plus 40 for header
				}
				
				if (speakerTable == null) {
					speakerTable = new UITableView(frame, UITableViewStyle.Grouped);
					speakerTable.BackgroundColor = UIColor.White;
					var whiteView = new UIView();
					whiteView.BackgroundColor = UIColor.White;
					speakerTable.BackgroundView = whiteView;
					speakerTable.ScrollEnabled = false;
					AddSubview (speakerTable);
				}
				speakerTable.Frame = frame;  
				speakerTable.Source = new SpeakersTableSource(showSession.Speakers, this);
				
				if (AppDelegate.IsPad) { // shrink the overview to accomodate speakers
					var df = descriptionTextView.Frame;
					df.Height = df.Height - 40 - (showSession.Speakers.Count * 40) - 5; // 5 is for margin
					descriptionTextView.Frame = df;
					
					// set the highlight for whatever speaker is showing in the other column (only iPad, obviously)
					if (speakerTable.IndexPathForSelectedRow == null)
						speakerTable.SelectRow (NSIndexPath.FromRowSection (0,0), true, UITableViewScrollPosition.Top);
					else
						speakerTable.SelectRow (speakerTable.IndexPathForSelectedRow, true, UITableViewScrollPosition.Top);
					
			
				} else // extend the Frame to encompass the speakers table
					Frame = new CGRect(0,0,320, bottomOfTheseControls + speakerTable.Frame.Height + 20); // 10 margin top & bottom
			} else { // there are NO speakers, remove the table if it exists
				if (speakerTable != null) {
					speakerTable.RemoveFromSuperview ();
					speakerTable.Dispose ();
					speakerTable = null;
				}
				if (AppDelegate.IsPhone)
					Frame = new CGRect(0,0,320, bottomOfTheseControls);
			}
			
			if (AppDelegate.IsPhone && Bounds.Size.Height < 370) {
				// if the view is smaller than the display area, enlarge it to fit snugly
				Frame = new CGRect(0,0, 320, 370);
			}
		}

		/// <summary>
		/// When a speaker is selected, show their details (either in splitview or push on NavCtrl)
		/// </summary>
		public void SelectSpeaker(Speaker speaker) 
		{
			hostScreen.SelectSpeaker(speaker);
		}

		public void Clear()
		{
			showSession = null;
			LayoutSubviews (); // show the grey 'no session' message
		}

		bool shouldShowSpeakers = true;
		/// <summary>
		/// Change the session info being displayed in the view
		/// </summary>
		public void Update (int sessionID, bool shouldShowSpeakers)
		{
			if (speakerTable != null) // need to re-set, incase index 10 was selected last time and new session has fewer speakers
				speakerTable.SelectRow (NSIndexPath.FromRowSection (0,0), true, UITableViewScrollPosition.Top);

			this.shouldShowSpeakers = shouldShowSpeakers;
			showSession = BL.Managers.SessionManager.GetSession (sessionID);
			Update ();
			LayoutSubviews ();
		}
		/// <summary>
		/// Change the session info being displayed in the view
		/// </summary>
		public void Update (MWC.BL.Session session)
		{
			if (speakerTable != null) // need to re-set, incase index 10 was selected last time and new session has fewer speakers
				speakerTable.SelectRow (NSIndexPath.FromRowSection (0,0), true, UITableViewScrollPosition.Top);

			showSession = session;
			Update ();
			LayoutSubviews ();
		}
		
		/// <summary>
		/// Used in ViewWillAppear (SessionsScreen, SessionDayScheduleScreen) 
		/// to sync favorite-stars that have changed in other views
		/// </summary>		
		public void UpdateFavorite() {
			if (showSession != null)
				UpdateImage (FavoritesManager.IsFavorite (showSession.Key)) ;
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
			if (FavoritesManager.IsFavorite (showSession.Key)) {
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