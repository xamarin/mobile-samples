using System;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;
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
			TakeVideo = new DelegatedCommand (TakeVideoHandler, s => this.picker.VideosSupported && this.picker.IsCameraAvailable);
			PickVideo = new DelegatedCommand (PickVideoHandler, s => this.picker.VideosSupported);
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

		public ICommand TakeVideo
		{
			get;
			private set;
		}

		public ICommand PickVideo
		{
			get;
			private set;
		}

		public Visibility PhotoVisibility
		{
			get { return (VideoPlayerVisibility == Visibility.Collapsed) ? Visibility.Visible : Visibility.Collapsed; }
		}

		private Visibility videoPlayerVisibility = Visibility.Collapsed;
		public Visibility VideoPlayerVisibility
		{
			get { return this.videoPlayerVisibility; }
			set
			{
				if (this.videoPlayerVisibility == value)
					return;

				this.videoPlayerVisibility = value;
				OnPropertyChanged ("VideoPlayerVisibility");
				OnPropertyChanged ("PhotoVisibility");
			}
		}

		private BitmapImage image;
		public BitmapImage Image
		{
			get { return this.image; }
			private set
			{
				this.VideoPlayerVisibility = Visibility.Collapsed;

				if (this.image == value)
					return;

				this.image = value;
				OnPropertyChanged ("Image");
			}
		}

		private IRandomAccessStream video;
		public IRandomAccessStream Video
		{
			get { return this.video; }
			private set
			{
				VideoPlayerVisibility = Visibility.Visible;

				if (this.video == value)
					return;

				this.video = value;
				OnPropertyChanged ("Video");
			}
		}

		private readonly MediaPicker picker = new MediaPicker();

		private async void TakePhotoHandler (object s)
		{
			try
			{
				using (MediaFile file = await this.picker.TakePhotoAsync (new StoreCameraMediaOptions()))
				{
					State = file.Path;
					StorageFile sfile = await StorageFile.GetFileFromPathAsync (file.Path);
					Image = new BitmapImage();
					Image.SetSource (await sfile.OpenReadAsync());
				}
			}
			catch (OperationCanceledException)
			{
				State = "Canceled";
			}
			catch (Exception ex)
			{
				State = "Error: " + ex.Message;
			}
		}

		private async void PickPhotoHandler (object s)
		{
			try
			{
				using (MediaFile file = await this.picker.PickPhotoAsync())
				{
					State = file.Path;
					StorageFile sfile = await StorageFile.GetFileFromPathAsync (file.Path);
					Image = new BitmapImage();
					Image.SetSource (await sfile.OpenReadAsync());
				}
			}
			catch (OperationCanceledException)
			{
				State = "Canceled";
			}
			catch (Exception ex)
			{
				State = "Error: " + ex.Message;
			}
		}

		private async void PickVideoHandler (object o)
		{
			try
			{
				using (MediaFile file = await this.picker.PickVideoAsync())
				{
					State = file.Path;
					StorageFile sfile = await StorageFile.GetFileFromPathAsync(file.Path);
					Video = await sfile.OpenReadAsync();
				}
			}
			catch (OperationCanceledException)
			{
				State = "Canceled";
			}
			catch (Exception ex)
			{
				State = "Error: " + ex.Message;
			}
		}

		private async void TakeVideoHandler (object o)
		{
			try
			{
				using (MediaFile file = await this.picker.TakeVideoAsync (new StoreVideoOptions()))
				{
					State = file.Path;
					StorageFile sfile = await StorageFile.GetFileFromPathAsync (file.Path);
					Video = await sfile.OpenReadAsync();
				}
			}
			catch (OperationCanceledException)
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