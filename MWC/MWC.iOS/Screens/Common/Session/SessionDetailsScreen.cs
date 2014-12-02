using UIKit;
using System.Drawing;
using System;
using Foundation;
using MWC.BL;
using MWC.BL.Managers;
using CoreGraphics;

namespace MWC.iOS.Screens.Common.Session {
	/// <summary>
	/// Display session info (name, time, location) using UIKit controls and XIB file
	/// </summary>
	public partial class SessionDetailsScreen : UIViewController {
		BL.Session session;
		int sessionId;
		
		static bool UserInterfaceIdiomIsPhone {
			get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
		}

		public SessionDetailsScreen (int sessionID)
			: base (UserInterfaceIdiomIsPhone ? "SessionDetailsScreen_iPhone" : "SessionDetailsScreen_iPad", null)
		{
			ConsoleD.WriteLine ("Creating Session Details Screen, Session ID: " + sessionID.ToString());
			sessionId = sessionID;
		}
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			int width = 245;
			if (!UserInterfaceIdiomIsPhone)
				width = 700;

			session = BL.Managers.SessionManager.GetSession (sessionId);
			
			Title = "Session Detail";
			TitleLabel.Text = session.Title;
			SpeakerLabel.Text = session.SpeakerNames;			
			TimeLabel.Text = session.Start.ToString("dddd") + " " +
								session.Start.ToString("H:mm") + " - " + 
								session.End.ToString("H:mm");
			LocationLabel.Text = session.Room;
			OverviewLabel.Text = session.Overview;
			
			CGSize titleSize = TitleLabel.StringSize (session.Title
							, UIFont.FromName ("Helvetica-Light", AppDelegate.Font16pt)
							, new SizeF (245, 400), UILineBreakMode.WordWrap);
			TitleLabel.Font = UIFont.FromName("Helvetica-Light", AppDelegate.Font16pt);
			TitleLabel.TextColor = UIColor.Black;
			TitleLabel.Frame = new CGRect(13, 15, width, titleSize.Height);
			TitleLabel.Lines = 0;
			TitleLabel.Font = UIFont.FromName ("Helvetica-Light", AppDelegate.Font16pt);

			CGSize speakerSize = TitleLabel.StringSize (session.SpeakerNames
							, UIFont.FromName ("Helvetica-LightOblique", AppDelegate.Font10pt)
							, new SizeF (245, 400), UILineBreakMode.WordWrap);
			SpeakerLabel.Font = UIFont.FromName("Helvetica-LightOblique", AppDelegate.Font10pt);
			SpeakerLabel.Frame = new CGRect(13
													, 15 + 13 + titleSize.Height
													, width, speakerSize.Height);
			TimeLabel.Font = UIFont.FromName ("Helvetica-Light", AppDelegate.Font7_5pt);
			TimeLabel.Frame = new CGRect(13
													, 15 + titleSize.Height + 13 + speakerSize.Height + 5
													, width, 10);
			
			LocationLabel.Font = UIFont.FromName ("Helvetica-Light", AppDelegate.Font7_5pt);
			LocationLabel.Frame = new CGRect(13
													, 15 + titleSize.Height + 13 + speakerSize.Height + 7 + 12
													, width, 10);

			float overviewLabelWidth = 310;
			var overviewLabelY = 15 + titleSize.Height + 13 + speakerSize.Height + TimeLabel.Frame.Height + LocationLabel.Frame.Height + 20;
			nfloat overviewLabelHeight = (UserInterfaceIdiomIsPhone?360:854) - overviewLabelY;
			OverviewLabel.Editable = false;
			OverviewLabel.Font = UIFont.FromName("Helvetica-Light", AppDelegate.Font10_5pt);
			if (AppDelegate.IsPhone) {
				// going to scroll the whole thing!
				OverviewLabel.ScrollEnabled = false;
			
				CGSize overviewSize = OverviewLabel.StringSize (
								  session.Overview
								, UIFont.FromName("Helvetica-Light", AppDelegate.Font10_5pt)
								, new CGSize(overviewLabelWidth, 2500) // just width wasn't working...
								, UILineBreakMode.WordWrap);

				overviewLabelHeight = overviewSize.Height + 30;
				
				ScrollView.ContentSize = new CGSize(320, overviewLabelY + overviewLabelHeight + 10);
			}

			OverviewLabel.Frame = new CGRect(5
													, overviewLabelY
													, UserInterfaceIdiomIsPhone?overviewLabelWidth:700
													, overviewLabelHeight);


			FavoriteButton.TouchUpInside += (sender, e) => {
				ToggleFavorite ();
			};

			if (FavoritesManager.IsFavorite (session.Key))
				FavoriteButton.SetImage (new UIImage(AppDelegate.ImageIsFavorite), UIControlState.Normal);
			else
				FavoriteButton.SetImage (new UIImage(AppDelegate.ImageNotFavorite), UIControlState.Normal);
		}

		bool ToggleFavorite ()
		{
			if (FavoritesManager.IsFavorite (session.Key)) {
				FavoriteButton.SetImage (new UIImage(AppDelegate.ImageNotFavorite), UIControlState.Normal);
				FavoritesManager.RemoveFavoriteSession (session.Key);
				return false;
			} else {
				FavoriteButton.SetImage (new UIImage(AppDelegate.ImageIsFavorite), UIControlState.Normal);
				var fav = new Favorite{SessionID = session.ID, SessionKey = session.Key};
				FavoritesManager.AddFavoriteSession (fav);
				return true;
			}
		}
	}
}