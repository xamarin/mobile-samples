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
		UIToolbar _toolbar;
		UIScrollView _scrollView;		

		int y = 0;
		int _speakerID;
		Speaker _speaker;

		const int ImageSpace = 80;

		public SpeakerDetailsScreen (int speakerID) : base()
		{
			_speakerID = speakerID;

			this.View.BackgroundColor = UIColor.White;
			
			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad)
			{
				_toolbar = new UIToolbar(new RectangleF(0,0,UIScreen.MainScreen.Bounds.Width, 40));
				
				this.View.AddSubview (_toolbar);
				y = 40;
			}

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

			
			_scrollView = new UIScrollView();

			_scrollView.AddSubview (_nameLabel);
			_scrollView.AddSubview (_titleLabel);
			_scrollView.AddSubview (_companyLabel);
			_scrollView.AddSubview (_bioTextView);
			_scrollView.AddSubview (_image);	

			this.Add (_scrollView);	
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
			
			_scrollView.Frame = full;

			bigFrame.X = ImageSpace+13+17;
			bigFrame.Y = y + 27; // 15 -> 13
			bigFrame.Height = 26;
			bigFrame.Width -= (ImageSpace+13+17);
			_nameLabel.Frame = bigFrame;
			
			var smallFrame = full;
			smallFrame.X = ImageSpace+13+17;
			smallFrame.Y = y + 27+26;
			smallFrame.Height = 15; // 12 -> 15
			smallFrame.Width -= (ImageSpace+13+17);
			_titleLabel.Frame = smallFrame;
			
			smallFrame.Y += y + 17;
			_companyLabel.Frame = smallFrame;

			_image.Frame = new RectangleF(13, y + 15, 80, 80);
			
			this._bioTextView.Font = UIFont.FromName ("Helvetica-Light", AppDelegate.Font10_5pt);
			
			if (!String.IsNullOrEmpty(_speaker.Bio))
			{
				var f = new SizeF (full.Width - 13 * 2, 4000);
				SizeF size = _bioTextView.StringSize (_speaker.Bio
									, this._bioTextView.Font
									, f);
				_bioTextView.Frame = new RectangleF(5
									, y + 115
									, f.Width
									, size.Height + 120); // doesn't seem to measure properly... CR/LF issues?
			
				_bioTextView.ScrollEnabled = true;
				
				_scrollView.ContentSize = new SizeF(320, _bioTextView.Frame.Y + _bioTextView.Frame.Height + 20);
			}
			else
			{
				_bioTextView.ScrollEnabled = false;
				_bioTextView.Frame = new RectangleF(5, y + 115, 310, 30);;
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
				this._bioTextView.TextColor = UIColor.Black;
			}
			else
			{
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

		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
        {
            return true;
        }

	}
}