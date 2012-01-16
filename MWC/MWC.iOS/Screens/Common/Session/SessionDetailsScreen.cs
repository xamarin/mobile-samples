using MonoTouch.UIKit;
using System.Drawing;
using System;
using MonoTouch.Foundation;
using MWC.BL;
using MWC.BL.Managers;

namespace MWC.iOS.Screens.Common.Session
{
	/// <summary>
	/// Display session info (name, time, location) using UIKit controls and XIB file
	/// </summary>
	public partial class SessionDetailsScreen : UIViewController
	{
		protected BL.Session _session;
		int _sessionID;
		
		static bool UserInterfaceIdiomIsPhone {
			get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
		}

		public SessionDetailsScreen ( int sessionID )
			: base (UserInterfaceIdiomIsPhone ? "SessionDetailsScreen_iPhone" : "SessionDetailsScreen_iPad", null)
		{
			Console.WriteLine ( "Creating Session Details Screen, Session ID: " + sessionID.ToString() );
			_sessionID = sessionID;
		}
				
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			this._session = BL.Managers.SessionManager.GetSession ( _sessionID );
			this.Title = "Session Detail";
			this.TitleLabel.Text = this._session.Title;
			this.SpeakerLabel.Text = this._session.SpeakerNames;			
			this.TimeLabel.Text = this._session.Start.ToShortTimeString() + " - " + this._session.End.ToShortTimeString();
			this.OverviewLabel.Text = this._session.Overview;
			

			SizeF titleSize = this.TitleLabel.StringSize (this._session.Title
							, UIFont.FromName ("Helvetica-Light", AppDelegate.Font16pt)
							, new SizeF (245, 400), UILineBreakMode.WordWrap);
			this.TitleLabel.Font = UIFont.FromName("Helvetica-Light", AppDelegate.Font16pt);
			this.TitleLabel.TextColor = UIColor.Black;
			this.TitleLabel.Frame = new RectangleF(13, 15, 245, titleSize.Height);
			this.TitleLabel.Lines = 0;
			this.TitleLabel.Font = UIFont.FromName ("Helvetica-Light", AppDelegate.Font16pt);

			SizeF speakerSize = this.TitleLabel.StringSize (this._session.SpeakerNames
							, UIFont.FromName ("Helvetica-LightOblique", AppDelegate.Font10pt)
							, new SizeF (245, 400), UILineBreakMode.WordWrap);
			this.SpeakerLabel.Font = UIFont.FromName("Helvetica-LightOblique", AppDelegate.Font10pt);
			this.SpeakerLabel.Frame = new RectangleF(13
													, 15 + 13 + titleSize.Height
													, 245, speakerSize.Height);
			this.TimeLabel.Font = UIFont.FromName ("Helvetica-Light", AppDelegate.Font7_5pt);
			this.TimeLabel.Frame = new RectangleF(13
													, 15 + titleSize.Height + 13 + speakerSize.Height + 3
													, 245, 10);
			
			this.OverviewLabel.Font = UIFont.FromName("Helvetica-Light", AppDelegate.Font10_5pt);
			this.OverviewLabel.Editable = false;
			this.OverviewLabel.Frame = new RectangleF(5
													, 15 + titleSize.Height + 13 + speakerSize.Height + TimeLabel.Frame.Height + 20
													, 310
													, 360 - (15 + titleSize.Height + 13 + speakerSize.Height + TimeLabel.Frame.Height + 20));

			this.FavoriteButton.TouchUpInside += (sender, e) => {
				ToggleFavorite ();
			};

			if (FavoritesManager.IsFavorite (_session.Title))
				this.FavoriteButton.SetImage (new UIImage(AppDelegate.ImageIsFavorite), UIControlState.Normal);
			else
				this.FavoriteButton.SetImage (new UIImage(AppDelegate.ImageNotFavorite), UIControlState.Normal);
		}
		bool ToggleFavorite ()
		{
			if (FavoritesManager.IsFavorite (_session.Title)) {
				this.FavoriteButton.SetImage (new UIImage(AppDelegate.ImageNotFavorite), UIControlState.Normal);
				FavoritesManager.RemoveFavoriteSession (_session.Title);
				return false;
			} else {
				this.FavoriteButton.SetImage (new UIImage(AppDelegate.ImageIsFavorite), UIControlState.Normal);
				var fav = new Favorite{SessionID = _session.ID, SessionName = _session.Title};
				FavoritesManager.AddFavoriteSession (fav);
				return true;
			}
		}
	}
}

