using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.Dialog;
using MonoTouch.Dialog.Utilities;
using System.Drawing;
using MWC.BL;

namespace MWC.iOS.UI.Controls.Views
{
	/// <summary>
	/// from SpeakerDetailsScreen    TODO: merge/re-use
	/// </summary>
	public class SpeakerView : UIView, IImageUpdated
	{
		UILabel _nameLabel, _titleLabel, _companyLabel;
		UITextView _bioTextView;
		UIImageView _image;
		UIToolbar _toolbar;
		
		int y = 0;
		int _speakerID;
		Speaker _speaker;

		const int ImageSpace = 80;
		int width = 335;		
		
		public SpeakerView (int speakerID)
		{
			_speakerID = speakerID;

			this.BackgroundColor = UIColor.White;
			
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

			this.AddSubview (_nameLabel);
			this.AddSubview (_titleLabel);
			this.AddSubview (_companyLabel);
			this.AddSubview (_bioTextView);
			this.AddSubview (_image);	
		}

		public override void LayoutSubviews ()
		{
			if (_speaker == null) return;

			var full = this.Bounds;
			var bigFrame = full;
			
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

			if (!String.IsNullOrEmpty(_speaker.Bio))
			{
				if (AppDelegate.IsPhone)
				{	// for now, hardcode iPhone dimensions to reduce regressions
				
					SizeF size = _bioTextView.StringSize (_speaker.Bio
										, _bioTextView.Font
										, new SizeF (310, 580)
										, UILineBreakMode.WordWrap);
					_bioTextView.Frame = new RectangleF(5, y + 115, 310, size.Height);
				}
				else
				{
					var f = new SizeF (full.Width - 13 * 2, full.Height - (_image.Frame.Y + 80 + 20));
					SizeF size = _bioTextView.StringSize (_speaker.Bio
										, _bioTextView.Font
										, f
										, UILineBreakMode.WordWrap);
					_bioTextView.Frame = new RectangleF(5, _image.Frame.Y + 80 + 10
										, f.Width
										, f.Height);
				}
			}
			else
			{
				_bioTextView.Frame = new RectangleF(5, y + 115, 310, 30);
			}
		}
		
		// for masterdetail
		public void Update(int speakerID)
		{
			_speakerID = speakerID;
			_speaker = BL.Managers.SpeakerManager.GetSpeaker ( _speakerID );
			Update ();
			LayoutSubviews ();
		}

		public void Clear()
		{
			this._nameLabel.Text = "";
			this._titleLabel.Text = "";
			this._companyLabel.Text = "";
			this._bioTextView.Text = "";
			this._image.Image = null;
		}

		void Update()
		{
			Console.WriteLine ("SpeakerView.Update()");
			if (_speaker == null) {this._nameLabel.Text ="not found"; return;}
			
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