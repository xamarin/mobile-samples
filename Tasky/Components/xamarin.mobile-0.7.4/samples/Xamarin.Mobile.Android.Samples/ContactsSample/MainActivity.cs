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

			// We must request permission to access the user's address book
			// This will prompt the user on platforms that ask, or it will validate
			// manifest permissions on platforms that declare their required permissions.
			AddressBook.RequestPermission().ContinueWith (t =>
			{
				if (!t.Result) {
					Toast.MakeText (this, "Permission denied, check your manifest", ToastLength.Long).Show();
					return;
				}

				// Contacts can be selected and sorted using LINQ!
				//
				// In this sample, we'll just use LINQ to sort contacts by
				// their last name in reverse order.
				foreach (Contact contact in AddressBook.Where (c => c.FirstName != null).OrderBy (c => c.FirstName)) {
					contacts.Add (contact.DisplayName);
					contactIDs.Add (contact.Id); // Save the ID in a parallel list
				}

				ListAdapter = new ArrayAdapter<string> (this, Resource.Layout.list_item, contacts.ToArray());
				ListView.TextFilterEnabled = true;

				// When clicked, start a new activity to display more contact details
				ListView.ItemClick += delegate (object sender, AdapterView.ItemClickEventArgs args) {

					// To show the contact on the details activity, we
					// need to send that activity the contacts ID
					string contactId = contactIDs[args.Position];
					Intent showContactDetails = new Intent (this, typeof(ContactActivity));
					showContactDetails.PutExtra ("contactID", contactId);
					StartActivity (showContactDetails);
				
					// Alternatively, show a toast with the name of the contact selected
					//
					//Toast.MakeText (Application, ((TextView)args.View).Text, ToastLength.Short).Show ();
				};
			}, TaskScheduler.FromCurrentSynchronizationContext());			
		}
	}
}