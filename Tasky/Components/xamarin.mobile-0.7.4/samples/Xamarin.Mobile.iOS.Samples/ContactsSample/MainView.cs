using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoTouch.Dialog;
using Foundation;
using UIKit;
using Xamarin.Contacts;
using CoreGraphics;

namespace ContactsSample
{
	public class MainView : UIViewController
	{
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			SetupUI();

			var book = new Xamarin.Contacts.AddressBook ();

			// We must request permission to access the user's address book
			// This will prompt the user on platforms that ask, or it will validate
			// manifest permissions on platforms that declare their required permissions.
			book.RequestPermission().ContinueWith (t =>
			{
				if (!t.Result)
				{
					alert = new UIAlertView ("Permission denied", "User has denied this app access to their contacts", null, "Close");
					alert.Show();
				}
				else
				{
					// Contacts can be selected and sorted using LINQ!
					//
					// In this sample, we'll just use LINQ to sort contacts by
					// their last name in reverse order.
					list = book.OrderByDescending (c => c.LastName).ToList();

					tableView.Source = new TableViewDataSource (list);
					tableView.ReloadData();
				}
			}, TaskScheduler.FromCurrentSynchronizationContext());
		}

		// Simple table view data source using the List<Contact> as the datasource
		private class TableViewDataSource : UITableViewSource
		{
			public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
			{
				Contact contact = this.list[indexPath.Row];
				
				RootElement root = new RootElement ("Info");

				UIView title = new UIView (new CGRect (0, 0, UIScreen.MainScreen.Bounds.Width, 64)) {
					AutoresizingMask = UIViewAutoresizing.FlexibleWidth,
				};

				title.AddSubview (new UIImageView (new CGRect (0, 0, 64, 64)) {
					Image = contact.GetThumbnail(),
					BackgroundColor = UIColor.White
				});

				title.AddSubview (new UITextView (new CGRect (64, -10, title.Bounds.Width, 36)) {
					Text = contact.DisplayName,
					AutoresizingMask = UIViewAutoresizing.FlexibleWidth,
					BackgroundColor = UIColor.Clear,
					Font = UIFont.SystemFontOfSize (22)
				});

				var org = contact.Organizations.FirstOrDefault();
				if (org != null) {
					title.AddSubview (new UITextView (new CGRect (65, 13, title.Bounds.Width, 25)) {
						Text = org.ContactTitle,
						AutoresizingMask = UIViewAutoresizing.FlexibleWidth,
						BackgroundColor = UIColor.Clear,
						TextColor = UIColor.DarkGray,
					});

					title.AddSubview (new UITextView (new CGRect (65, 28, title.Bounds.Width, 25)) {
						Text = org.Name,
						AutoresizingMask = UIViewAutoresizing.FlexibleWidth,
						BackgroundColor = UIColor.Clear,
						TextColor = UIColor.DarkGray
					});
				}

				root.Add (new Section { new UIViewElement (String.Empty, title, true) });

				var phoneElements = contact.Phones.Select (p =>
					new StringElement (p.Label.ToLower(), p.Number)).ToArray();

				if (phoneElements.Length > 0) {
					Section phones = new Section();
					phones.AddAll (phoneElements);
					root.Add (phones);
				}

				var emailElements = contact.Emails.Select (e =>
					new StringElement ((e.Label == "Other") ? "Email" : e.Label, e.Address)).ToArray();

				if (emailElements.Length > 0) {
					Section emails = new Section();
					emails.AddAll (emailElements);
					root.Add (emails);
				}

				var addressElements = contact.Addresses.Select (a => {
					StringBuilder address = new StringBuilder();
					if (!String.IsNullOrEmpty (a.StreetAddress))
						address.Append (a.StreetAddress);

					if (!(String.IsNullOrEmpty (a.City) && String.IsNullOrEmpty (a.Region) && String.IsNullOrEmpty (a.PostalCode)))
						address.AppendFormat("{3}{0} {1} {2}", a.City, a.Region, a.PostalCode, (address.Length > 0) ? Environment.NewLine : String.Empty);

					if (!String.IsNullOrEmpty (a.Country))
						address.AppendFormat ("{1}{0}", a.Country, (address.Length > 0) ? Environment.NewLine : String.Empty);

					return new StyledMultilineElement (
						(a.Label == "Other") ? "Address" : a.Label,
						address.ToString());
				}).ToArray();

				if (addressElements.Length > 0) {
					Section addresses = new Section();
					addresses.AddAll (addressElements);
					root.Add (addresses);
				}

				var dialog = new DialogViewController (root, pushing: true);
				
				AppDelegate appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate;
				appDelegate.NavigationController.PushViewController (dialog, true);
			}

			public override nint RowsInSection (UITableView tableview, nint section)
			{
				return list.Count;
			}

			static readonly NSString CellIdentifier = new NSString ("contactIdentifier");
			private readonly List<Contact> list;

			public TableViewDataSource (List<Contact> list)
			{
				this.list = list;
			}

			public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
			{
				UITableViewCell cell = tableView.DequeueReusableCell (CellIdentifier);
				if (cell == null)
					cell = new UITableViewCell (UITableViewCellStyle.Subtitle, CellIdentifier);

				Contact contact = list[indexPath.Row];

				cell.TextLabel.Text = contact.DisplayName;
				Email firstEmail = contact.Emails.FirstOrDefault();
				if (firstEmail != null)
					cell.DetailTextLabel.Text = String.Format ("{0}: {1}", firstEmail.Label.ToLower(), firstEmail.Address);
				
				return cell;
			}
		}
		
		UITableView tableView;
		List<Contact> list;
		private UIAlertView alert;

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
			this.tableView.DeselectRow (this.tableView.IndexPathForSelectedRow, animated: true);
		}

		private void SetupUI()
		{
			Title = "Contacts";

			this.tableView = new UITableView {
				AutoresizingMask = UIViewAutoresizing.FlexibleHeight | UIViewAutoresizing.FlexibleWidth,
				Frame = new CGRect (0, 0, View.Frame.Width, View.Frame.Height)
			};

			View.AddSubview (this.tableView);
		}
	}
}