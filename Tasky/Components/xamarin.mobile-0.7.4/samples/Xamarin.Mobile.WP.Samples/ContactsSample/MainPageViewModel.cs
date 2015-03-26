using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Media.Imaging;
using Xamarin.Contacts;
using System.Threading.Tasks;

namespace ContactsSample
{
	public class MainPageViewModel
		: INotifyPropertyChanged
	{
		public MainPageViewModel()
		{
			Setup();
		}

		private async Task Setup()
		{
			if (!await this.addressBook.RequestPermission())
				this.addressBook = null;
		}

		public event EventHandler SelectedContact;
		public event PropertyChangedEventHandler PropertyChanged;

		private bool showProgress = true;
		public bool ShowProgress
		{
			get { return this.showProgress; }
			set
			{
				if (this.showProgress == value)
					return;

				this.showProgress = value;
				OnPropertyChanged("ShowProgress");
			}
		}

		public IEnumerable<Contact> Contacts
		{
			get { return FinishWhenIterated (this.addressBook ?? Enumerable.Empty<Contact>()); }
		}

		private BitmapImage thumb;
		public BitmapImage Thumbnail
		{
			get
			{
				if (this.thumb == null && this.contact != null)
					this.thumb = this.contact.GetThumbnail();

				return this.thumb;
			}
		}

		private Contact contact;
		public Contact Contact
		{
			get { return this.contact; }
			set
			{
				if (this.contact == value)
					return;

				this.contact = value;
				this.thumb = null;
				OnPropertyChanged ("Contact");
				OnPropertyChanged ("Thumbnail");

				if (value != null)
					OnSelectedContact (EventArgs.Empty);
			}
		}

		private AddressBook addressBook = new AddressBook();

		private IEnumerable<Contact> FinishWhenIterated (IEnumerable<Contact> contacts)
		{
			foreach (var contact in contacts)
				yield return contact;

			ShowProgress = false;
		}
		
		private void OnPropertyChanged (string name)
		{
			var changed = PropertyChanged;
			if (changed != null)
				changed (this, new PropertyChangedEventArgs (name));
		}

		private void OnSelectedContact (EventArgs e)
		{
			var selected = SelectedContact;
			if (selected != null)
				selected (this, e);
		}
	}
}
