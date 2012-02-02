using System;
using System.Drawing;
using MonoTouch.Dialog.Utilities;
using MonoTouch.UIKit;
using MWC.BL;

namespace MWC.iOS.Screens.iPhone.Exhibitors {
	/// <summary>
	/// Displays information about the Exhibitor
	/// </summary>
	public class ExhibitorDetailsScreen : UIViewController, IImageUpdated {
		UILabel nameLabel, addressLabel, locationLabel;
		UITextView descriptionTextView;
		UIImageView image;
		
		int exhibitorId;
		Exhibitor exhibitor;
		
		const int ImageSpace = 80;

		public ExhibitorDetailsScreen (int exhibitorID) : base()
		{
			exhibitorId = exhibitorID;
		}

		public override void ViewDidLoad ()
		{
			View.BackgroundColor = UIColor.White;
			nameLabel = new UILabel () {
				TextAlignment = UITextAlignment.Left,
				Font = UIFont.FromName ("Helvetica-Light", AppDelegate.Font16pt),
				BackgroundColor = UIColor.FromWhiteAlpha (0f, 0f)
			};
			addressLabel = new UILabel () {
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
				Editable = false
			};
			image = new UIImageView();
		
			View.AddSubview (nameLabel);
			View.AddSubview (addressLabel);
			View.AddSubview (locationLabel);
			View.AddSubview (descriptionTextView);
			View.AddSubview (image);

			LayoutSubviews();
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			exhibitor = BL.Managers.ExhibitorManager.GetExhibitor (exhibitorId);
			// shouldn't be null, but it gets that way when the data
			// "shifts" underneath it. need to reload the screen or prevent
			// selection via loading overlay - neither great UIs :-(
			if (exhibitor != null) {
				Update ();
			}
		}

		void LayoutSubviews ()
		{
			var full = View.Bounds;
			var bigFrame = full;
			
			bigFrame.X = ImageSpace+13+17;
			bigFrame.Y = 27; // 15 -> 13
			bigFrame.Height = 26;
			bigFrame.Width -= (ImageSpace+13+17);
			nameLabel.Frame = bigFrame;
			
			var smallFrame = full;
			smallFrame.X = ImageSpace+13+17;
			smallFrame.Y = 27+26;
			smallFrame.Height = 15; // 12 -> 15
			smallFrame.Width -= (ImageSpace+13+17);
			addressLabel.Frame = smallFrame;
			
			smallFrame.Y += 17;
			locationLabel.Frame = smallFrame;

			image.Frame = new RectangleF (13, 15, 80, 80);
			
			if (AppDelegate.IsPhone)
				descriptionTextView.Frame = new RectangleF (10, 115, 300, 250);
			else {
				// IsPad
				descriptionTextView.Frame = new RectangleF (10, 115, 400, 900);	
				//_descriptionTextView.AutoresizingMask = UIViewAutoresizing.FlexibleWidth;
			}
		}

		void Update()
		{
			nameLabel.Text = exhibitor.Name;
			addressLabel.Text = exhibitor.City + ", " + exhibitor.Country;
			locationLabel.Text = exhibitor.Locations;

			if (!String.IsNullOrEmpty(exhibitor.Overview)) {
				descriptionTextView.Text = exhibitor.Overview;
				descriptionTextView.Font = UIFont.FromName ("Helvetica-Light", AppDelegate.Font10_5pt);
				descriptionTextView.TextColor = UIColor.Black;
			} else {
				descriptionTextView.Font = UIFont.FromName ("Helvetica-LightOblique", AppDelegate.Font10_5pt);
				descriptionTextView.TextColor = UIColor.Gray;
				descriptionTextView.Text = "No background information available.";
			}
			if (exhibitor.ImageUrl != "http://www.mobileworldcongress.com") {
				// empty image shows this
				Console.WriteLine ("#Update:" + exhibitor.ImageUrl);
				var u = new Uri (exhibitor.ImageUrl);
				image.Image = ImageLoader.DefaultRequestImage (u, this);
			}
		}

		public void UpdatedImage (Uri uri)
		{
			Console.WriteLine ("#UpdatedImage:" + uri.AbsoluteUri);
			image.Image = ImageLoader.DefaultRequestImage (uri, this);
		}
	}
}