using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.Dialog;
using MonoTouch.Dialog.Utilities;
using System.Drawing;
using MWC.BL;
using System.Linq;

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

		int y = 0;
		int _sessionID;
		MWC.BL.Session _session;

		int width = 368;

		public SessionView (MWC.BL.Session session)
		{
			//_sessionID = sessionID;
			_session = session;

			this.BackgroundColor = UIColor.White;
			
			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad)
			{
				_toolbar = new UIToolbar(new RectangleF(0, 0, width, 40));
				_toolbar.TintColor = UIColor.DarkGray;
				_toolbar.Items = new UIBarButtonItem[]{
					new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
					new UIBarButtonItem("Sessions", UIBarButtonItemStyle.Plain, null),
					new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),};
				this.AddSubview (_toolbar);
				y = 40;
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

			this.AddSubview (_titleLabel);
			this.AddSubview (_timeLabel);
			this.AddSubview (_locationLabel);			
			this.AddSubview (_descriptionTextView);
		}

		public override void LayoutSubviews ()
		{
			//_session = BL.Managers.SessionManager.GetSession ( _sessionID );
			if (_session != null) 
			{	
				
				Update ();
			} else return;

			var full = this.Bounds;
			var bigFrame = full;
			
			SizeF titleSize = this._titleLabel.StringSize (this._session.Title
							, UIFont.FromName ("Helvetica-Light", AppDelegate.Font16pt)
							, new SizeF (245, 400), UILineBreakMode.WordWrap);

			bigFrame.X = 13;
			bigFrame.Y = y + 10; // 15 -> 13
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
			
		}
	}
}

