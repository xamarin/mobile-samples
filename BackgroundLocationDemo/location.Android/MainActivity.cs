using System;

using Android;
using Android.App;
using Android.Content.PM;
using Android.Locations;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Support.V7.App;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace Location.Droid
{
    [Activity(Label = "LocationDroid", MainLauncher = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.ScreenLayout)]
    public class MainActivity : AppCompatActivity
    {
        static readonly int RC_REQUEST_LOCATION_PERMISSION = 1000;
        static readonly string TAG = "MainActivity";
        static readonly string[] REQUIRED_PERMISSIONS = {Manifest.Permission.AccessFineLocation};
        TextView accText;
        TextView altText;
        TextView bearText;

        // make our labels
        TextView latText;
        TextView longText;
        TextView speedText;

        //Lifecycle stages
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            Log.Debug(TAG, "OnCreate: Location app is coming to life.");

            SetContentView(Resource.Layout.Main);

            // This event fires when the ServiceConnection lets the client (our App class) know that
            // the Service is connected. We use this event to start updating the UI with location
            // updates from the Service
            App.Current.LocationServiceConnected += (sender, e) =>
                                                    {
                                                        Log.Debug(TAG, "ServiceConnected Event Raised");
                                                        // notifies us of location changes from the system
                                                        App.Current.LocationService.LocationChanged += HandleLocationChanged;
                                                        //notifies us of user changes to the location provider (ie the user disables or enables GPS)
                                                        App.Current.LocationService.ProviderDisabled += HandleProviderDisabled;
                                                        App.Current.LocationService.ProviderEnabled += HandleProviderEnabled;
                                                        // notifies us of the changing status of a provider (ie GPS no longer available)
                                                        App.Current.LocationService.StatusChanged += HandleStatusChanged;
                                                    };


            latText = FindViewById<TextView>(Resource.Id.lat);
            longText = FindViewById<TextView>(Resource.Id.longx);
            altText = FindViewById<TextView>(Resource.Id.alt);
            speedText = FindViewById<TextView>(Resource.Id.speed);
            bearText = FindViewById<TextView>(Resource.Id.bear);
            accText = FindViewById<TextView>(Resource.Id.acc);

            altText.Text = "altitude";
            speedText.Text = "speed";
            bearText.Text = "bearing";
            accText.Text = "accuracy";

            // Start the location service:
            if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.AccessFineLocation) == (int) Permission.Granted)
            {
                Log.Debug(TAG, "User already has granted permission.");
                App.StartLocationService();
            }
            else
            {
                Log.Debug(TAG, "Have to request permission from the user. ");
                RequestLocationPermission();
            }
        }

        protected override void OnResume()
        {
            Log.Debug(TAG, "OnResume: Location app is moving into foreground");
            base.OnResume();
        }

        protected override void OnPause()
        {
            Log.Debug(TAG, "OnPause: Location app is moving to background");
            base.OnPause();
        }

        protected override void OnDestroy()
        {
            Log.Debug(TAG, "OnDestroy: Location app is becoming inactive");
            base.OnDestroy();

            // Stop the location service:
            App.StopLocationService();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            if (requestCode == RC_REQUEST_LOCATION_PERMISSION)
            {
                if (grantResults.Length == 1 && grantResults[0] == Permission.Granted)
                {
                    Log.Debug(TAG, "User granted permission for location.");
                    App.StartLocationService();
                }
                else
                {
                    Log.Warn(TAG, "User did not grant permission for the location.");
                }
            }
            else
            {
                base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            }
        }

        void RequestLocationPermission()
        {
            if (ActivityCompat.ShouldShowRequestPermissionRationale(this, Manifest.Permission.AccessFineLocation))
            {
                var layout = FindViewById(Android.Resource.Id.Content);
                Snackbar.Make(layout,
                              Resource.String.permission_location_rationale,
                              Snackbar.LengthIndefinite)
                        .SetAction(Resource.String.ok,
                                   new Action<View>(delegate
                                                    {
                                                        ActivityCompat.RequestPermissions(this, REQUIRED_PERMISSIONS,
                                                                                          RC_REQUEST_LOCATION_PERMISSION);
                                                    })
                                  ).Show();
            }
            else
            {
                ActivityCompat.RequestPermissions(this, REQUIRED_PERMISSIONS, RC_REQUEST_LOCATION_PERMISSION);
            }
        }

        /// <summary>
        ///     Updates UI with location data
        /// </summary>
        public void HandleLocationChanged(object sender, LocationChangedEventArgs e)
        {
            var location = e.Location;
            Log.Debug(TAG, "Foreground updating");

            // these events are on a background thread, need to update on the UI thread
            RunOnUiThread(() =>
                          {
                              latText.Text = $"Latitude: {location.Latitude}";
                              longText.Text = $"Longitude: {location.Longitude}";
                              altText.Text = $"Altitude: {location.Altitude}";
                              speedText.Text = $"Speed: {location.Speed}";
                              accText.Text = $"Accuracy: {location.Accuracy}";
                              bearText.Text = $"Bearing: {location.Bearing}";
                          });
        }

        public void HandleProviderDisabled(object sender, ProviderDisabledEventArgs e)
        {
            Log.Debug(TAG, "Location provider disabled event raised");
        }

        public void HandleProviderEnabled(object sender, ProviderEnabledEventArgs e)
        {
            Log.Debug(TAG, "Location provider enabled event raised");
        }

        public void HandleStatusChanged(object sender, StatusChangedEventArgs e)
        {
            Log.Debug(TAG, "Location status changed, event raised");
        }
    }
}
