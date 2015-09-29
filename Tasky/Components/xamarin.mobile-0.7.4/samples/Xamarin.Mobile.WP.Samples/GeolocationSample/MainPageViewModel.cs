using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Geolocation;

namespace GeolocationSample
{
	public class MainPageViewModel
		: INotifyPropertyChanged
	{
		public MainPageViewModel (Geolocator geolocator, SynchronizationContext syncContext)
		{
			if (geolocator == null)
				throw new ArgumentNullException ("geolocator");
			if (syncContext == null)
				throw new ArgumentNullException ("syncContext");

			this.sync = syncContext;
			this.geolocator = geolocator;
			this.geolocator.DesiredAccuracy = 50;
			this.geolocator.PositionError += GeolocatorOnPositionError;
			this.geolocator.PositionChanged += GeolocatorOnPositionChanged;
			this.getPosition = new DelegatedCommand (GetPositionHandler, s => true);
			this.toggleListening = new DelegatedCommand (ToggleListeningHandler, s => true);
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private bool showProgress;
		public bool ShowProgress
		{
			get { return this.showProgress; }
			set
			{
				if (this.showProgress == value)
					return;

				this.showProgress = value;
				OnPropertyChanged ("ShowProgress");
			}
		}

		private string status;
		public string Status
		{
			get { return this.status; }
			private set
			{
				if (this.status == value)
					return;

				this.status = value;
				OnPropertyChanged ("Status");
			}
		}

		private readonly DelegatedCommand getPosition;
		public ICommand GetPosition
		{
			get { return this.getPosition; }
		}

		private readonly DelegatedCommand toggleListening;
		public ICommand ToggleListening
		{
			get { return this.toggleListening; }
		}

		private Position currentPosition;
		public Position CurrentPosition
		{
			get { return this.currentPosition; }
			private set
			{
				if (this.currentPosition == value)
					return;

				this.currentPosition = value;
				OnPropertyChanged ("CurrentPosition");
			}
		}

		private readonly Geolocator geolocator;
		private readonly SynchronizationContext sync;

		private async void GetPositionHandler (object state)
		{
			ShowProgress = true;
			Status = "Getting location..";

			if (!this.geolocator.IsGeolocationEnabled)
			{
				Status = "Location disabled";
				return;
			}

			try
			{
				Position p = await this.geolocator.GetPositionAsync (10000, includeHeading: true);
				CurrentPosition = p;
				Status = "Success";
			}
			catch (GeolocationException ex)
			{
				Status = "Error: (" + ex.Error + ") " + ex.Message;
			}
			catch (TaskCanceledException cex)
			{
				Status = "Canceled";
			}

			ShowProgress = false;
		}

		private void ToggleListeningHandler (object o)
		{
			if (!this.geolocator.IsGeolocationEnabled)
			{
				Status = "Location disabled";
				return;
			}

			if (!this.geolocator.IsListening)
			{
				Status = "Listening";
				this.geolocator.StartListening (0, 0, includeHeading: true);
			}
			else
			{
				Status = "Stopped listening";
				this.geolocator.StopListening();
			}
		}

		private void GeolocatorOnPositionError (object sender, PositionErrorEventArgs e)
		{
			Status = e.Error.ToString();
		}

		private void GeolocatorOnPositionChanged (object sender, PositionEventArgs e)
		{
			CurrentPosition = e.Position;
		}

		private void OnPropertyChanged (string name)
		{
			var changed = PropertyChanged;
			if (changed != null)
				this.sync.Post (n => changed (this, new PropertyChangedEventArgs ((string)n)), name);
		}
	}
}