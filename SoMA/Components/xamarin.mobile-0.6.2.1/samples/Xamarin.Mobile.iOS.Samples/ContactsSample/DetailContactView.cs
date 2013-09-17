using System;
using Xamarin.Contacts;
using MonoTouch.UIKit;
using System.Text;
using System.Globalization;
using System.Drawing;

namespace ContactsSample
{
	/// <summary>
	/// Detail contact view. This view displays the details of a contact.
	/// </summary>
	public class DetailContactView : UIViewController
	{
		Contact contact;
		UIImageView contactImage;
		UILabel nameLabel;
		UILabel phoneLabel;
		UILabel emailLabel;
		public DetailContactView (Contact c)
		{
			this.contact = c;
		}
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			View.BackgroundColor = UIColor.White;
			
			//
			// position and display the contact thumbnail image
			//
			contactImage = new UIImageView(new RectangleF(210, 5, 60, 60));
			this.View.AddSubview(contactImage);
			contactImage.Image = contact.GetThumbnail();
			
			//
			// position and display the contact Display name
			//
			nameLabel = new UILabel(new RectangleF(5, 5, 200, 30));
			this.View.AddSubview(nameLabel);
			nameLabel.Text = contact.DisplayName;
			
			//
			// position and display the first phone number in the contact object
			//
			// A more complete implementation may display all the phone numbers in 
			// a table, or query for a specific type of phone number
			//
			phoneLabel = new UILabel(new RectangleF(5, 40, 200, 30));
			this.View.AddSubview(phoneLabel);
			
			String phoneString = String.Empty; 
			foreach(var phone in contact.Phones)
			{
				phoneString = String.Format("{0}: {1}", phone.Label, phone.Number);
				break; //just take the first phone number in this example
			}
			
			phoneLabel.Text = phoneString;
			
			//
			// position and display the first email in the contact object
			//
			// A more complete implementation may display all the email addresses in 
			// a table, or query for a specific type of email
			//
			emailLabel = new UILabel(new RectangleF(5, 80, 200, 30));
			this.View.AddSubview(emailLabel);
			
			String emailString = String.Empty; 
			foreach(var email in contact.Emails)
			{
				emailString = String.Format("{0}: {1}", email.Label, email.Address);
				break; //just take the first email in this example
			}
			
			emailLabel.Text = emailString;
			
		}
	}
}

