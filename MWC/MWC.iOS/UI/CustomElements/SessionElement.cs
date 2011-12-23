using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.Dialog;
using System.Drawing;
using MWC.BL;

namespace MWC.iOS.UI.CustomElements
{
	public class SessionCell : UITableViewCell {
		static UIFont bigFont = UIFont.BoldSystemFontOfSize (16);
		static UIFont midFont = UIFont.BoldSystemFontOfSize (15);
		static UIFont smallFont = UIFont.SystemFontOfSize (14);
		static UIImage favorite = UIImage.FromFile ("Images/favorite.png");
		static UIImage favorited = UIImage.FromFile ("Images/favorited.png");
		UILabel bigLabel, smallLabel;
		UIButton button;
		Session session;
		const int ImageSpace = 32;
		const int Padding = 8;
		
		public SessionCell (UITableViewCellStyle style, NSString ident, Session session, string big, string small) : base (style, ident)
		{
			SelectionStyle = UITableViewCellSelectionStyle.Blue;
			
			bigLabel = new UILabel () {
				TextAlignment = UITextAlignment.Left,
				BackgroundColor = UIColor.FromWhiteAlpha (0f, 0f)
			};
			smallLabel = new UILabel () {
				TextAlignment = UITextAlignment.Left,
				Font = smallFont,
				TextColor = UIColor.DarkGray,
				BackgroundColor = UIColor.FromWhiteAlpha (0f, 0f)
			};
			button = UIButton.FromType (UIButtonType.Custom);
			button.TouchDown += delegate {
				UpdateImage (ToggleFavorite ());
			};
			UpdateCell (session, big, small);
			
			ContentView.Add (bigLabel);
			ContentView.Add (smallLabel);
			ContentView.Add (button);
		}
		
		public void UpdateCell (Session session, string big, string small)
		{
			this.session = session;
//			UpdateImage (AppDelegate.UserData.IsFavorite (session.Code));
			
			bigLabel.Font = big.Length > 35 ? midFont : bigFont;
			bigLabel.Text = big;
			
			smallLabel.Text = small;
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
			return true;
//			var udata = AppDelegate.UserData;
//			if (udata.IsFavorite (session.Code)){
//				udata.RemoveFavoriteSession (session.Code);
//				return false;
//			} else {
//				udata.AddFavoriteSession (session.Code);
//				return true;
//			}
		}
		
		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();
			var full = ContentView.Bounds;
			var bigFrame = full;
			
			bigFrame.Height = 22;
			bigFrame.X = Padding;
			bigFrame.Width -= ImageSpace+Padding;
			bigLabel.Frame = bigFrame;
			
			var smallFrame = full;
			smallFrame.Y = 22;
			smallFrame.Height = 21;
			smallFrame.X = Padding;
			smallFrame.Width = bigFrame.Width;
			smallLabel.Frame = smallFrame;
			
			button.Frame = new RectangleF (full.Width-ImageSpace, -3, ImageSpace, 48);
		}
	}
	
	// Renders a session
	public class SessionElement : Element {
		static NSString key = new NSString ("sessionElement");
	
		Session session;
		string subtitle;
		
		public SessionElement (Session session) : base (session.Title)
		{
			this.session = session;
			if(String.IsNullOrEmpty(session.Room))
				subtitle = String.Format ("{0}", session.Speakers);
			else if (String.IsNullOrEmpty(session.Speakers))
				subtitle = String.Format("{0} room", session.Room);
			else
				subtitle = String.Format ("{0} room; {1}", session.Room, session.Speakers);

		}
		
		static int count;
		public override UITableViewCell GetCell (UITableView tv)
		{
			var cell = tv.DequeueReusableCell (key);
			count++;
			if (cell == null)
			{
				cell = new SessionCell (UITableViewCellStyle.Subtitle, key, session, Caption, subtitle);
			}
			else
				((SessionCell)cell).UpdateCell (session, Caption, subtitle);
			
			return cell;
		}

		public override void Selected (DialogViewController dvc, UITableView tableView, MonoTouch.Foundation.NSIndexPath path)
		{
			var sds = new MWC.iOS.Screens.Common.Session.SessionDetailsScreen (session.ID);
			dvc.ActivateController (sds);
		}

	}

}