using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Xamarin.Media;

namespace MediaPickerSample
{
	public class MainPageViewModel
		: INotifyPropertyChanged
	{
		public MainPageViewModel()
		{
			TakePhoto = new DelegatedCommand (TakePhotoHandler, s => this.picker.PhotosSupported && this.picker.IsCameraAvailable);
			PickPhoto = new DelegatedCommand (PickPhotoHandler, s => this.picker.PhotosSupported);
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private string state;
		public string State
		{
			get { return this.state; }
			private set
			{
				if (this.state == value)
					return;

				this.state = value;
				OnPropertyChanged ("State");
			}
		}

		public ICommand TakePhoto
		{
			get;
			private set;
		}

		public ICommand PickPhoto
		{
			get;
			private set;
		}

		private BitmapImage image;
		public BitmapImage Image
		{
			get { return this.image; }
			private set
			{
				if (this.image == value)
					return;

				this.image = value;
				OnPropertyChanged ("Image");
			}
		}

		private readonly MediaPicker picker = new MediaPicker();

		private async void TakePhotoHandler (object state)
		{
			try
			{
				using (MediaFile file = await this.picker.TakePhotoAsync (new StoreCameraMediaOptions()))
				{
					State = file.Path;
					Image = new BitmapImage();
					Image.SetSource (file.GetStream());
				}
			}
			catch (TaskCanceledException)
			{
				State = "Canceled";
			}
			catch (Exception ex)
			{
				State = "Error: " + ex.Message;
			}
		}

		private async void PickPhotoHandler (object state)
		{
			try
			{
				using (MediaFile file = await this.picker.PickPhotoAsync())
				{
					State = file.Path;
					Image = new BitmapImage();
					Image.SetSource (file.GetStream());
				}
			}
			catch (TaskCanceledException)
			{
				State = "Canceled";
			}
			catch (Exception ex)
			{
				State = "Error: " + ex.Message;
			}
		}

		private void OnPropertyChanged (string name)
		{
			var changed = PropertyChanged;
			if (changed != null)
				changed (this, new PropertyChangedEventArgs (name));
		}
	}
}