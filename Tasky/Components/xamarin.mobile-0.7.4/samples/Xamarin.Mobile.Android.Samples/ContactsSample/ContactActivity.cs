using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Contacts;

namespace ContactsSample
{
	[Activity (Label = "ContactActivity")]			
	public class ContactActivity : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			
			//
			// Get the contact ID that is passed in
			// from the main activity
			//
			String contactID = String.Empty;
			if(bundle != null)
			{
				contactID = bundle.GetString("contactID");
			}
			else
			{
				contactID = Intent.GetStringExtra("contactID");	
			}
			
			Contact contact = MainActivity.AddressBook.Load (contactID);
			
			//
			// If the contact is empty, we'll
			// display a 'not found' error
			//
			String displayName = "Contact Not Found";
			String mobilePhone = String.Empty;
			
			//
			// Set the displayName variable to the contact's
			// DisplayName property
			//
			if(contact != null)
			{
				displayName = contact.DisplayName;
				var phone = contact.Phones.FirstOrDefault (p => p.Type == PhoneType.Mobile);
				if(phone != null)
				{
					mobilePhone = phone.Number;	
				}
			}
			
			//
			// Show the contacts display name and mobile phone
			//
			SetContentView (Resource.Layout.contact_view);
			var fullNameTextView = FindViewById<TextView> (Resource.Id.full_name);
			fullNameTextView.Text = displayName;
			var mobilePhoneTextView = FindViewById<TextView> (Resource.Id.mobile_phone);
			mobilePhoneTextView.Text = mobilePhone;
			
		}
	}
}

