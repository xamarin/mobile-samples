using System;
using Android.App;
using Android.Widget;
using Android.OS;
using Android.Util;
using Android.Locations;
using Location.Droid.Services;

namespace Location.Droid
{
	[Activity (Label = "AndroidLocationService", MainLauncher = true)]
	public class MainActivity : Activity
	{
		readonly string logTag = "MainActivity";

		// make our labels
		TextView latText;
		TextView longText;
		TextView altText;
		TextView speedText;
		TextView bearText;
		TextView accText;
		
		#region Lifecycle

		//Lifecycle stages
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			
			SetContentView (Resource.Layout.Main);

			// This event fires when the ServiceConnection lets the client (our App class) know that
			// the Service is connected. We use this event to start updating the UI with location
			// updates from the Service
			App.Current.LocationServiceConnected += (object sender, ServiceConnectedEventArgs e) => {
				Log.Debug (logTag, "ServiceConnected Event Raised");
				// notifies us of location changes from the system
				App.Current.LocationService.LocationChanged += HandleLocationChanged;
				//notifies us of user changes to the location provider (ie the user disables or enables GPS)
				App.Current.LocationService.ProviderDisabled += HandleProviderDisabled;
				App.Current.LocationService.ProviderEnabled += HandleProviderEnabled;
				// notifies us of the changing status of a provider (ie GPS no longer available)
				App.Current.LocationService.StatusChanged += HandleStatusChanged;
			};

			latText = FindViewById<TextView> (Resource.Id.lat);
			longText = FindViewById<TextView> (Resource.Id.longx);
			altText = FindViewById<TextView> (Resource.Id.alt);
			speedText = FindViewById<TextView> (Resource.Id.speed);
			bearText = FindViewById<TextView> (Resource.Id.bear);
			accText = FindViewById<TextView> (Resource.Id.acc);
		}

		protected override void OnPause()
		{
			Log.Debug (logTag, "Location app is moving to background");
			base.OnPause();
		}
		
		protected override void OnResume()
		{
			Log.Debug (logTag, "Location app is moving into foreground");
			base.OnPause();
		}
		
		protected override void OnDestroy ()
		{
			Log.Debug (logTag, "Location app is becoming inactive");
			base.OnDestroy ();
		}

		#endregion

		#region Android Location Service methods

		///<summary>
		/// Updates UI with location data
		/// </summary>
		public void HandleLocationChanged(object sender, LocationChangedEventArgs e)
		{
			Android.Locations.Location location = e.Location;
			Log.Debug (logTag, "Foreground updating");

			// these events are on a background thread, need to update on the UI thread
			RunOnUiThread (() => {
				latText.Text = String.Format ("Latitude: {0}", location.Latitude);
				longText.Text = String.Format ("Longitude: {0}", location.Longitude);
				altText.Text = String.Format ("Altitude: {0}", location.Altitude);
				speedText.Text = String.Format ("Speed: {0}", location.Speed);
				accText.Text = String.Format ("Accuracy: {0}", location.Accuracy);
				bearText.Text = String.Format ("Bearing: {0}", location.Bearing);
			});

		}

		public void HandleProviderDisabled(object sender, ProviderDisabledEventArgs e)
		{
			Log.Debug (logTag, "Location provider disabled event raised");
		}

		public void HandleProviderEnabled(object sender, ProviderEnabledEventArgs e)
		{
			Log.Debug (logTag, "Location provider enabled event raised");
		}

		public void HandleStatusChanged(object sender, StatusChangedEventArgs e)
		{
			Log.Debug (logTag, "Location status changed, event raised");
		}

		#endregion
	
	}
}


