using System;
using Android.App;
using Android.Widget;
using Android.OS;
using System.Threading;
using Xamarin.Geolocation;

namespace GeolocationSample
{
	[Activity (Label = "GeolocationSample", MainLauncher = true)]
	public class MainActivity : Activity
	{
		private Button toggleListenButton, cancelPositionButton;
		
		private TextView	positionStatus, positionLatitude, positionLongitude, positionAccuracy,
							listenStatus, listenLatitude, listenLongitude, listenAccuracy;
		
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			FindViewById<Button> (Resource.Id.getPositionButton)
				.Click += OnGetPosition;
			
			this.cancelPositionButton = FindViewById<Button> (Resource.Id.cancelPositionButton);
			this.cancelPositionButton.Click += OnCancelPosition;
			
			this.toggleListenButton = FindViewById<Button> (Resource.Id.toggleListeningButton);
			this.toggleListenButton.Click += OnToggleListening;
			
			this.positionStatus = FindViewById<TextView> (Resource.Id.status);
			this.positionAccuracy = FindViewById<TextView> (Resource.Id.pAccuracy);
			this.positionLatitude = FindViewById<TextView> (Resource.Id.pLatitude);
			this.positionLongitude = FindViewById<TextView> (Resource.Id.pLongitude);
			
			this.listenStatus = FindViewById<TextView> (Resource.Id.listenStatus);
			this.listenAccuracy = FindViewById<TextView> (Resource.Id.lAccuracy);
			this.listenLatitude = FindViewById<TextView> (Resource.Id.lLatitude);
			this.listenLongitude = FindViewById<TextView> (Resource.Id.lLongitude);
		}
		
		private Geolocator geolocator;
		private CancellationTokenSource cancelSource;
		
		private void Setup()
		{
			if (this.geolocator != null)
				return;

			this.geolocator = new Geolocator (this) { DesiredAccuracy = 50 };
			this.geolocator.PositionError += OnListeningError;
			this.geolocator.PositionChanged += OnPositionChanged;
		}

		private void OnGetPosition (object sender, EventArgs e)
		{
			Setup();

			if (!this.geolocator.IsGeolocationAvailable || !this.geolocator.IsGeolocationEnabled)
			{
				Toast.MakeText (this, "Geolocation is unavailable", ToastLength.Long).Show();
				return;
			}
			
			this.cancelSource = new CancellationTokenSource();
			
			this.positionStatus.Text = String.Empty;
			this.positionAccuracy.Text = String.Empty;
			this.positionLatitude.Text = String.Empty;
			this.positionLongitude.Text = String.Empty;
			
			this.geolocator.GetPositionAsync (timeout: 10000, cancelToken: this.cancelSource.Token)
				.ContinueWith (t => RunOnUiThread (() =>
				{
					if (t.IsFaulted)
						this.positionStatus.Text = ((GeolocationException)t.Exception.InnerException).Error.ToString();
					else if (t.IsCanceled)
						this.positionStatus.Text = "Canceled";
					else
					{
						this.positionStatus.Text = t.Result.Timestamp.ToString("G");
						this.positionAccuracy.Text = t.Result.Accuracy + "m";
						this.positionLatitude.Text = "La: " + t.Result.Latitude.ToString("N4");
						this.positionLongitude.Text = "Lo: " + t.Result.Longitude.ToString("N4");
					}
				}));
		}
		
		private void OnCancelPosition (object sender, EventArgs e)
		{
			CancellationTokenSource cancel = this.cancelSource;
			if (cancel != null)
				cancel.Cancel();
		}
		
		private void OnToggleListening (object sender, EventArgs e)
		{
			Setup();
			
			if (!this.geolocator.IsListening)
			{
				if (!this.geolocator.IsGeolocationAvailable || !this.geolocator.IsGeolocationEnabled)
				{
					Toast.MakeText (this, "Geolocation is unavailable", ToastLength.Long).Show();
					return;
				}

				this.toggleListenButton.SetText (Resource.String.stopListening);
				this.geolocator.StartListening (minTime: 30000, minDistance: 0);
			}
			else
			{
				this.toggleListenButton.SetText (Resource.String.startListening);
				this.geolocator.StopListening();
			}
		}
		
		private void OnListeningError (object sender, PositionErrorEventArgs e)
		{
			RunOnUiThread (() => {
				this.listenStatus.Text = e.Error.ToString();
			});
		}
		
		private void OnPositionChanged (object sender, PositionEventArgs e)
		{
			RunOnUiThread (() => {
				this.listenStatus.Text = e.Position.Timestamp.ToString("G");
				this.listenAccuracy.Text = e.Position.Accuracy + "m";
				this.listenLatitude.Text = "La: " + e.Position.Latitude.ToString("N4");
				this.listenLongitude.Text = "Lo: " + e.Position.Longitude.ToString("N4");
			});
		}
	}
}


