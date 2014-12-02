using System;
using System.Drawing;
using MonoTouch.Dialog.Utilities;
using UIKit;
using MWC.BL;
using CoreGraphics;

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
		EmptyOverlay emptyOverlay;
		
		const int imageSpace = 80;
		/// <summary>Only used for iPhone display. iPad scrolls the TextView only.</summary>
		UIScrollView scrollView;	

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
		
			if (AppDelegate.IsPad) {
				View.AddSubview (nameLabel);
				View.AddSubview (addressLabel);
				View.AddSubview (locationLabel);
				View.AddSubview (descriptionTextView);
				View.AddSubview (image);
			} else {
				scrollView = new UIScrollView();
	
				scrollView.AddSubview (nameLabel);
				scrollView.AddSubview (addressLabel);
				scrollView.AddSubview (locationLabel);
				scrollView.AddSubview (descriptionTextView);
				scrollView.AddSubview (image);	
	
				Add (scrollView);
			}
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			exhibitor = BL.Managers.ExhibitorManager.GetExhibitor (exhibitorId);
			// shouldn't be null, but it gets that way when the data
			// "shifts" underneath it. need to reload the screen or prevent
			// selection via loading overlay - neither great UIs :-(
			LayoutSubviews();
			if (exhibitor != null) {
				Update ();
			}
		}

		void LayoutSubviews ()
		{
			if (EmptyOverlay.ShowIfRequired(ref emptyOverlay, exhibitor, View, "No exhibitor selected", EmptyOverlayType.Exhibitor)) return;
//			if (exhibitor == null) {
//				if (emptyOverlay == null) {
//					emptyOverlay = new EmptyOverlay(View.Bounds, "No exhibitor selected");
//					View.AddSubview (emptyOverlay);
//				}
//				return;
//			} else{
//				if (emptyOverlay != null) {
//					emptyOverlay.RemoveFromSuperview ();
//					emptyOverlay = null;
//				}
//			}
			var full = View.Bounds;
			var bigFrame = full;
			
			bigFrame.X = imageSpace+13+17;
			bigFrame.Y = 27; // 15 -> 13
			bigFrame.Height = 26;
			bigFrame.Width -= (imageSpace+13+17);
			nameLabel.Frame = bigFrame;
			
			var smallFrame = full;
			smallFrame.X = imageSpace+13+17;
			smallFrame.Y = 27+26;
			smallFrame.Height = 15; // 12 -> 15
			smallFrame.Width -= (imageSpace+13+17);
			addressLabel.Frame = smallFrame;
			
			smallFrame.Y += 17;
			locationLabel.Frame = smallFrame;

			image.Frame = new CGRect (13, 15, 80, 80);
			
			if (AppDelegate.IsPhone)
			{
				scrollView.Frame = full;
				
				
					var f = new CGSize (full.Width - 13 * 2, 4000);
					CGSize size = descriptionTextView.StringSize (exhibitor.Overview
										, descriptionTextView.Font
										, f);
					descriptionTextView.Frame = new CGRect(5
										, 115
										, f.Width
										, size.Height + 20); // doesn't seem to measure properly... CR/LF issues?
				
					descriptionTextView.ScrollEnabled = true;
					
					scrollView.ContentSize = new CGSize(320, descriptionTextView.Frame.Y + descriptionTextView.Frame.Height + 10);
					
				
				descriptionTextView.Frame = new CGRect (10, 115, 300, f.Height);
			} else {
				// IsPad
				descriptionTextView.Frame = new CGRect (10, 115, 400, 900);	
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
				var u = new Uri (exhibitor.ImageUrl);
				image.Image = ImageLoader.DefaultRequestImage (u, this);
			}
		}
		
		public void UpdatedImage (Uri uri)
		{
			image.Image = ImageLoader.DefaultRequestImage (uri, this);
		}

		[Obsolete]
		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
        {
            return AppDelegate.IsPad;
        }
	}
}