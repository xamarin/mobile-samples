using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Foundation;
using UIKit;
using MWC.BL;
using CoreGraphics;
using MonoTouch.Dialog.Utilities;

namespace MWC.iOS.Screens.iPhone.Speakers {
	/// <summary>
	/// Displays personal information about the speaker
	/// </summary>
	public class SpeakerDetailsScreen : UIViewController, IImageUpdated {
		UILabel nameLabel, titleLabel, companyLabel;
		UITextView bioTextView;
		UIImageView image;
		UIToolbar toolbar;
		UIScrollView scrollView;
		UITableView sessionTable;		
		int y = 0;
		int speakerId;
		Speaker speaker;
		const int ImageSpace = 80;
		
		public bool ShouldShowSessions { get; set; }

		public SpeakerDetailsScreen (int speakerID) : base()
		{
			ShouldShowSessions = true;

			speakerId = speakerID;

			View.BackgroundColor = UIColor.White;
			
			if (AppDelegate.IsPad) {
				toolbar = new UIToolbar (new CGRect(0,0,UIScreen.MainScreen.Bounds.Width, 40));
				
				View.AddSubview (toolbar);
				y = 40;
			}

			nameLabel = new UILabel () {
				TextAlignment = UITextAlignment.Left,
				Font = UIFont.FromName ("Helvetica-Light", AppDelegate.Font16pt),
				BackgroundColor = UIColor.FromWhiteAlpha (0f, 0f)
			};
			titleLabel = new UILabel () {
				TextAlignment = UITextAlignment.Left,
				Font = UIFont.FromName ("Helvetica-LightOblique", AppDelegate.Font10pt),
				TextColor = UIColor.DarkGray,
				BackgroundColor = UIColor.FromWhiteAlpha (0f, 0f)
			};
			companyLabel = new UILabel () {
				TextAlignment = UITextAlignment.Left,
				Font = UIFont.FromName ("Helvetica-Light", AppDelegate.Font10pt),
				TextColor = UIColor.DarkGray,
				BackgroundColor = UIColor.FromWhiteAlpha (0f, 0f)
			};
			 bioTextView = new UITextView () {
				TextAlignment = UITextAlignment.Left,
				Font = UIFont.FromName ("Helvetica-Light", AppDelegate.Font10_5pt),
				BackgroundColor = UIColor.FromWhiteAlpha (0f, 0f),
				ScrollEnabled = true,
				Editable = false
			};
			image = new UIImageView();

			
			scrollView = new UIScrollView();

			scrollView.AddSubview (nameLabel);
			scrollView.AddSubview (titleLabel);
			scrollView.AddSubview (companyLabel);
			scrollView.AddSubview (bioTextView);
			scrollView.AddSubview (image);	

			Add (scrollView);	
		}
		
		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			speaker = BL.Managers.SpeakerManager.GetSpeaker (speakerId);
			// this shouldn't be null, but it gets that way when the data
			// "shifts" underneath it. need to reload the screen or prevent
			// selection via loading overlay - neither great UIs :-(
			if (speaker != null)  {	
				LayoutSubviews ();
				Update ();
			}
		}

		void LayoutSubviews ()
		{
			var full = View.Bounds;
			var bigFrame = full;
			
			scrollView.Frame = full;

			bigFrame.X = ImageSpace+13+17;
			bigFrame.Y = y + 27; // 15 -> 13
			bigFrame.Height = 26;
			bigFrame.Width -= (ImageSpace+13+17);
			nameLabel.Frame = bigFrame;
			
			var smallFrame = full;
			smallFrame.X = ImageSpace+13+17;
			smallFrame.Y = y + 27+26;
			smallFrame.Height = 15; // 12 -> 15
			smallFrame.Width -= (ImageSpace+13+17);
			titleLabel.Frame = smallFrame;
			
			smallFrame.Y += y + 17;
			companyLabel.Frame = smallFrame;

			image.Frame = new CGRect(13, y + 15, 80, 80);
			
			bioTextView.Font = UIFont.FromName ("Helvetica-Light", AppDelegate.Font10_5pt);

			if (!String.IsNullOrEmpty(speaker.Bio)) {
				var f = new CGSize (full.Width - 13 * 2, 4000);
				CGSize size = bioTextView.StringSize (speaker.Bio
									, this.bioTextView.Font
									, f);
				bioTextView.Frame = new CGRect(5
									, y + 115
									, f.Width
									, size.Height + 120); // doesn't seem to measure properly... CR/LF issues?
			
				bioTextView.ScrollEnabled = true;
				
				scrollView.ContentSize = new CGSize(320, bioTextView.Frame.Y + bioTextView.Frame.Height + 20);
			} else {
				bioTextView.ScrollEnabled = false;
				bioTextView.Frame = new CGRect(5, y + 115, 310, 30);;
			}
			

			nfloat bottomOfTheseControls = bioTextView.Frame.Y + bioTextView.Frame.Height;

			if (ShouldShowSessions && speaker.Sessions != null && speaker.Sessions.Count > 0) {
				CGRect frame;
				//if (AppDelegate.IsPhone) {
					frame = new CGRect(5
									, bottomOfTheseControls
									, 310
									, speaker.Sessions.Count * 40 + 50); // plus 40 for header
				//}

				if (sessionTable == null) {
					sessionTable = new UITableView(frame, UITableViewStyle.Grouped);
					sessionTable.BackgroundColor = UIColor.White;
					var whiteView = new UIView();
					whiteView.BackgroundColor = UIColor.White;
					sessionTable.BackgroundView = whiteView;
					sessionTable.ScrollEnabled = false;
					scrollView.AddSubview (sessionTable);
				}
				sessionTable.Frame = frame;  
				sessionTable.Source = new SessionsTableSource(speaker.Sessions, this);

				scrollView.ContentSize = new CGSize(320, bottomOfTheseControls + sessionTable.Frame.Height + 20);

			} else { // there are NO sessions, remove the table if it exists
				if (sessionTable != null) {
					sessionTable.RemoveFromSuperview ();
					sessionTable.Dispose ();
					sessionTable = null;
				}
				if (AppDelegate.IsPhone)
					scrollView.ContentSize = new CGSize(320, bottomOfTheseControls);
			}
		}

		public void SelectSession (Session session)
		{
			var sds = new MWC.iOS.Screens.iPhone.Sessions.SessionDetailsScreen (session.ID);
			sds.ShouldShowSpeakers = false;
			sds.Title = "Session";
			NavigationController.PushViewController(sds, true);
		}

		void Update()
		{
			nameLabel.Text = speaker.Name;
			titleLabel.Text = speaker.Title;
			companyLabel.Text = speaker.Company;
			
			if (!String.IsNullOrEmpty(speaker.Bio)) {
				bioTextView.Text = speaker.Bio;
				bioTextView.TextColor = UIColor.Black;
			} else {
				bioTextView.TextColor = UIColor.Gray;
				bioTextView.Text = "No background information available.";
			}
			if (speaker.ImageUrl != "http://www.mobileworldcongress.com") {
				var u = new Uri(speaker.ImageUrl);
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