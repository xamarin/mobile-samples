using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MWC.BL;
using MonoTouch.Dialog.Utilities;

namespace MWC.iOS.Screens.iPhone.Speakers
{
	/// <summary>
	/// Displays personal information about the speaker
	/// </summary>
	public class SpeakerDetailsScreen : UIViewController, IImageUpdated
	{
		UILabel _nameLabel, _titleLabel, _companyLabel;
		UITextView _bioTextView;
		UIImageView _image;

		int _speakerID;
		Speaker _speaker;

		const int ImageSpace = 80;

		public SpeakerDetailsScreen (int speakerID) : base()
		{
			_speakerID = speakerID;

			this.View.BackgroundColor = UIColor.White;

			_nameLabel = new UILabel () {
				TextAlignment = UITextAlignment.Left,
				Font = UIFont.FromName ("Helvetica-Light", AppDelegate.Font16pt),
				BackgroundColor = UIColor.FromWhiteAlpha (0f, 0f)
			};
			_titleLabel = new UILabel () {
				TextAlignment = UITextAlignment.Left,
				Font = UIFont.FromName ("Helvetica-LightOblique", AppDelegate.Font10pt),
				TextColor = UIColor.DarkGray,
				BackgroundColor = UIColor.FromWhiteAlpha (0f, 0f)
			};
			_companyLabel = new UILabel () {
				TextAlignment = UITextAlignment.Left,
				Font = UIFont.FromName ("Helvetica-Light", AppDelegate.Font10pt),
				TextColor = UIColor.DarkGray,
				BackgroundColor = UIColor.FromWhiteAlpha (0f, 0f)
			};
			 _bioTextView = new UITextView () {
				TextAlignment = UITextAlignment.Left,
				Font = UIFont.FromName ("Helvetica-Light", AppDelegate.Font10_5pt),
				BackgroundColor = UIColor.FromWhiteAlpha (0f, 0f),
				ScrollEnabled = true,
				Editable = false
			};
			_image = new UIImageView();

			this.View.AddSubview (_nameLabel);
			this.View.AddSubview (_titleLabel);
			this.View.AddSubview (_companyLabel);
			this.View.AddSubview (_bioTextView);
			this.View.AddSubview (_image);	
		}
		
		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			_speaker = BL.Managers.SpeakerManager.GetSpeaker ( _speakerID );
			// this shouldn't be null, but it gets that way when the data
			// "shifts" underneath it. need to reload the screen or prevent
			// selection via loading overlay - neither great UIs :-(
			if (_speaker != null) 
			{	
				LayoutSubviews ();
				Update ();
			}
		}

		void LayoutSubviews ()
		{
			var full = this.View.Bounds;
			var bigFrame = full;
			
			bigFrame.X = ImageSpace+13+17;
			bigFrame.Y = 27; // 15 -> 13
			bigFrame.Height = 26;
			bigFrame.Width -= (ImageSpace+13+17);
			_nameLabel.Frame = bigFrame;
			
			var smallFrame = full;
			smallFrame.X = ImageSpace+13+17;
			smallFrame.Y = 27+26;
			smallFrame.Height = 15; // 12 -> 15
			smallFrame.Width -= (ImageSpace+13+17);
			_titleLabel.Frame = smallFrame;
			
			smallFrame.Y += 17;
			_companyLabel.Frame = smallFrame;

			_image.Frame = new RectangleF(13, 15, 80, 80);
			
			if (!String.IsNullOrEmpty(_speaker.Bio))
			{
//				SizeF size = _bioTextView.StringSize (_speaker.Bio
//									, _bioTextView.Font
//									, new SizeF (290, 500)
//									, UILineBreakMode.WordWrap);
				_bioTextView.Frame = new RectangleF(5, 115, 310, 240); //size.Height);
			}
			else
			{
				_bioTextView.Frame = new RectangleF(5, 115, 310, 30);
			}
		}

		void Update()
		{
			this._nameLabel.Text = _speaker.Name;
			this._titleLabel.Text = _speaker.Title;
			this._companyLabel.Text = _speaker.Company;

			if (!String.IsNullOrEmpty(_speaker.Bio))
			{
				this._bioTextView.Text = _speaker.Bio;
				this._bioTextView.Font = UIFont.FromName ("Helvetica-Light", AppDelegate.Font10_5pt);
				this._bioTextView.TextColor = UIColor.Black;
			}
			else
			{
				this._bioTextView.Font = UIFont.FromName ("Helvetica-LightOblique", AppDelegate.Font10_5pt);
				this._bioTextView.TextColor = UIColor.Gray;
				this._bioTextView.Text = "No background information available.";
			}
			if (_speaker.ImageUrl != "http://www.mobileworldcongress.com")
			{
				//Console.WriteLine("INITIAL:" + speaker.ImageUrl);
				var u = new Uri(_speaker.ImageUrl);
				_image.Image = ImageLoader.DefaultRequestImage(u,this);
			}
		}

		public void UpdatedImage (Uri uri)
		{
			//Console.WriteLine("UPDATED:" + uri.AbsoluteUri);
			_image.Image = ImageLoader.DefaultRequestImage(uri, this);
		}
	}
}