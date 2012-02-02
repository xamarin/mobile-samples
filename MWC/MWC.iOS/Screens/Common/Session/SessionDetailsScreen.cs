using MonoTouch.UIKit;
using System.Drawing;
using System;
using MonoTouch.Foundation;
using MWC.BL;
using MWC.BL.Managers;

namespace MWC.iOS.Screens.Common.Session {
	/// <summary>
	/// Display session info (name, time, location) using UIKit controls and XIB file
	/// </summary>
	public partial class SessionDetailsScreen : UIViewController {
		protected BL.Session session;
		int sessionId;
		
		static bool UserInterfaceIdiomIsPhone {
			get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
		}

		public SessionDetailsScreen (int sessionID)
			: base (UserInterfaceIdiomIsPhone ? "SessionDetailsScreen_iPhone" : "SessionDetailsScreen_iPad", null)
		{
			Console.WriteLine ("Creating Session Details Screen, Session ID: " + sessionID.ToString());
			sessionId = sessionID;
		}
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			int width = 245;
			if (!UserInterfaceIdiomIsPhone)
				width = 700;

			this.session = BL.Managers.SessionManager.GetSession (sessionId);
			this.Title = "Session Detail";
			this.TitleLabel.Text = this.session.Title;
			this.SpeakerLabel.Text = this.session.SpeakerNames;			
			this.TimeLabel.Text = this.session.Start.ToString("dddd") + " " +
								this.session.Start.ToString("H:mm") + " - " + 
								this.session.End.ToString("H:mm");
			this.LocationLabel.Text = this.session.Room;
			this.OverviewLabel.Text = this.session.Overview;
			
			SizeF titleSize = this.TitleLabel.StringSize (this.session.Title
							, UIFont.FromName ("Helvetica-Light", AppDelegate.Font16pt)
							, new SizeF (245, 400), UILineBreakMode.WordWrap);
			this.TitleLabel.Font = UIFont.FromName("Helvetica-Light", AppDelegate.Font16pt);
			this.TitleLabel.TextColor = UIColor.Black;
			this.TitleLabel.Frame = new RectangleF(13, 15, width, titleSize.Height);
			this.TitleLabel.Lines = 0;
			this.TitleLabel.Font = UIFont.FromName ("Helvetica-Light", AppDelegate.Font16pt);

			SizeF speakerSize = this.TitleLabel.StringSize (this.session.SpeakerNames
							, UIFont.FromName ("Helvetica-LightOblique", AppDelegate.Font10pt)
							, new SizeF (245, 400), UILineBreakMode.WordWrap);
			this.SpeakerLabel.Font = UIFont.FromName("Helvetica-LightOblique", AppDelegate.Font10pt);
			this.SpeakerLabel.Frame = new RectangleF(13
													, 15 + 13 + titleSize.Height
													, width, speakerSize.Height);
			this.TimeLabel.Font = UIFont.FromName ("Helvetica-Light", AppDelegate.Font7_5pt);
			this.TimeLabel.Frame = new RectangleF(13
													, 15 + titleSize.Height + 13 + speakerSize.Height + 5
													, width, 10);
			
			this.LocationLabel.Font = UIFont.FromName ("Helvetica-Light", AppDelegate.Font7_5pt);
			this.LocationLabel.Frame = new RectangleF(13
													, 15 + titleSize.Height + 13 + speakerSize.Height + 7 + 12
													, width, 10);

			float overviewLabelWidth = 310;
			var overviewLabelY = 15 + titleSize.Height + 13 + speakerSize.Height + TimeLabel.Frame.Height + LocationLabel.Frame.Height + 20;
			float overviewLabelHeight = (UserInterfaceIdiomIsPhone?360:854) - overviewLabelY;
			this.OverviewLabel.Editable = false;
			this.OverviewLabel.Font = UIFont.FromName("Helvetica-Light", AppDelegate.Font10_5pt);
			if (AppDelegate.IsPhone) {
				// going to scroll the whole thing!
				this.OverviewLabel.ScrollEnabled = false;
			
				SizeF overviewSize = this.OverviewLabel.StringSize (
								  this.session.Overview
								, UIFont.FromName("Helvetica-Light", AppDelegate.Font10_5pt)
								, new SizeF(overviewLabelWidth, 2500) // just width wasn't working...
								, UILineBreakMode.WordWrap);

				overviewLabelHeight = overviewSize.Height + 30;
				
				this.ScrollView.ContentSize = new SizeF(320, overviewLabelY + overviewLabelHeight + 10);
			}
			
			this.OverviewLabel.Frame = new RectangleF(5
													, overviewLabelY
													, UserInterfaceIdiomIsPhone?overviewLabelWidth:700
													, overviewLabelHeight);


			this.FavoriteButton.TouchUpInside += (sender, e) => {
				ToggleFavorite ();
			};

			if (FavoritesManager.IsFavorite (session.Key))
				this.FavoriteButton.SetImage (new UIImage(AppDelegate.ImageIsFavorite), UIControlState.Normal);
			else
				this.FavoriteButton.SetImage (new UIImage(AppDelegate.ImageNotFavorite), UIControlState.Normal);
		}

		bool ToggleFavorite ()
		{
			if (FavoritesManager.IsFavorite (session.Key)) {
				this.FavoriteButton.SetImage (new UIImage(AppDelegate.ImageNotFavorite), UIControlState.Normal);
				FavoritesManager.RemoveFavoriteSession (session.Key);
				return false;
			} else {
				this.FavoriteButton.SetImage (new UIImage(AppDelegate.ImageIsFavorite), UIControlState.Normal);
				var fav = new Favorite{SessionID = session.ID, SessionKey = session.Key};
				FavoritesManager.AddFavoriteSession (fav);
				return true;
			}
		}
	}
}