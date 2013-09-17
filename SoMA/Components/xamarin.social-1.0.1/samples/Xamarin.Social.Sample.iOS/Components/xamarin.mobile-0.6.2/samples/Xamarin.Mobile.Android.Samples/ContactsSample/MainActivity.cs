using System;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Database;
using Android.Net;
using Android.OS;
using Android.Provider;
using Android.Widget;
using Xamarin.Contacts;
using System.Linq;
using System.Collections.Generic;

namespace ContactsSample
{
	[Activity(Label = "ContactsSample", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : ListActivity
	{
		List<String> contacts = new List<String>();
		List<String> contactIDs = new List<String>();

		public static readonly AddressBook AddressBook = new AddressBook (Application.Context) { PreferContactAggregation = true };

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			AddressBook.RequestPermission().ContinueWith (t =>
			{
				if (!t.Result)
				{
					Toast.MakeText (this, "Permission denied, check your manifest", ToastLength.Long).Show();
					return;
				}

				//
				// Loop through the contacts and put them into a List<String>
				//
				// Note that the contacts are ordered by last name - contacts can be selected and sorted using LINQ!
				// A better performing solution would create a custom adapter to lazily pull the contacts
				//
				// In this sample, we'll just use LINQ to grab the first 10 users with mobile phone entries
				//
				foreach (Contact contact in AddressBook.Where(c => c.Phones.Any(p => p.Type == PhoneType.Mobile)).Take(10))
				{
					contacts.Add(contact.DisplayName);
					contactIDs.Add(contact.Id); //save the ID in a parallel list
				}
			
				ListAdapter = new ArrayAdapter<string> (this, Resource.Layout.list_item, contacts.ToArray());
				ListView.TextFilterEnabled = true;
			
				//
				// When clicked, start a new activity to display more contact details
				//	
				ListView.ItemClick += delegate (object sender, AdapterView.ItemClickEventArgs args) {

					//
					// to show the contact on the details activity, we
					// need to send that activity the contacts ID
					//
					String contactID = contactIDs[args.Position];
					Intent showContactDetails = new Intent(this, typeof(ContactActivity));
					showContactDetails.PutExtra("contactID", contactID);
					StartActivity(showContactDetails);
				
					//
					// alternatively, show a toast with the name of the contact selected
					//
					//Toast.MakeText (Application, ((TextView)args.View).Text, ToastLength.Short).Show ();
				};
			}, TaskScheduler.FromCurrentSynchronizationContext());			
		}
	}
}