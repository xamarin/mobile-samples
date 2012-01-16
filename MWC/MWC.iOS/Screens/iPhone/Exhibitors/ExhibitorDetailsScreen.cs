using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using MonoTouch.Dialog.Utilities;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MWC.BL;

namespace MWC.iOS.Screens.iPhone.Exhibitors
{
	/// <summary>
	/// Displays information about the Exhibitor
	/// </summary>
	public class ExhibitorDetailsScreen : UIViewController, IImageUpdated
	{
		UILabel _nameLabel, _addressLabel, _locationLabel;
		UITextView _descriptionTextView;
		UIImageView _image;
		
		int _exhibitorID;
		Exhibitor _exhibitor;
		
		const int ImageSpace = 80;

		public ExhibitorDetailsScreen (int exhibitorID) : base()
		{
			_exhibitorID = exhibitorID;
			this.View.BackgroundColor = UIColor.White;
			_nameLabel = new UILabel () {
				TextAlignment = UITextAlignment.Left,
				Font = UIFont.FromName ("Helvetica-Light", AppDelegate.Font16pt),
				BackgroundColor = UIColor.FromWhiteAlpha (0f, 0f)
			};
			_addressLabel = new UILabel () {
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
				Editable = false
			};
			_image = new UIImageView();

			this.View.AddSubview (_nameLabel);
			this.View.AddSubview (_addressLabel);
			this.View.AddSubview (_locationLabel);
			this.View.AddSubview (_descriptionTextView);
			this.View.AddSubview (_image);
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			_exhibitor = BL.Managers.ExhibitorManager.GetExhibitor ( _exhibitorID );
			// this shouldn't be null, but it gets that way when the data
			// "shifts" underneath it. need to reload the screen or prevent
			// selection via loading overlay - neither great UIs :-(
			if (_exhibitor != null) 
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
			_addressLabel.Frame = smallFrame;
			
			smallFrame.Y += 17;
			_locationLabel.Frame = smallFrame;

			_image.Frame = new RectangleF(13, 15, 80, 80);
			
			_descriptionTextView.Frame = new RectangleF(10, 115, 300, 250);
		}

		void Update()
		{
			this._nameLabel.Text = _exhibitor.Name;
			this._addressLabel.Text = _exhibitor.Address;
			this._locationLabel.Text = _exhibitor.Locations;

			if (!String.IsNullOrEmpty(_exhibitor.Overview))
			{
				this._descriptionTextView.Text = _exhibitor.Overview;
				this._descriptionTextView.Font = UIFont.FromName ("Helvetica-Light", AppDelegate.Font10_5pt);
				this._descriptionTextView.TextColor = UIColor.Black;
			}
			else
			{
				this._descriptionTextView.Font = UIFont.FromName ("Helvetica-LightOblique", AppDelegate.Font10_5pt);
				this._descriptionTextView.TextColor = UIColor.Gray;
				this._descriptionTextView.Text = "No background information available.";
			}
			if (_exhibitor.ImageUrl != "http://www.mobileworldcongress.com")
			{
				//Console.WriteLine("INITIAL:" + speaker.ImageUrl);
				var u = new Uri(_exhibitor.ImageUrl);
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