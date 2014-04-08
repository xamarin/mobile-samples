using System;

using Android.Content;
using Android.OS;
using Android.Util;

namespace Location.Droid.Services
{
	public class LocationServiceConnection : Java.Lang.Object, IServiceConnection
	{
		public event EventHandler<ServiceConnectedEventArgs> ServiceConnected = delegate {};

		public LocationServiceBinder Binder
		{
			get { return this.binder; }
			set { this.binder = value; }
		}
		protected LocationServiceBinder binder;

		public LocationServiceConnection (LocationServiceBinder binder)
		{
			if (binder != null) {
				this.binder = binder;
			}
		}

		// This gets called when a client tries to bind to the Service with an Intent and an 
		// instance of the ServiceConnection. The system will locate a binder associated with the 
		// running Service 
		public void OnServiceConnected (ComponentName name, IBinder service)
		{
			// cast the binder located by the OS as our local binder subclass
			LocationServiceBinder serviceBinder = service as LocationServiceBinder;
			if (serviceBinder != null) {
				this.binder = serviceBinder;
				this.binder.IsBound = true;
				Log.Debug ( "ServiceConnection", "OnServiceConnected Called" );
				// raise the service connected event
				this.ServiceConnected(this, new ServiceConnectedEventArgs () { Binder = service } );

				// now that the Service is bound, we can start gathering some location data
				serviceBinder.Service.StartLocationUpdates();
			}
		}
			
		// This will be called when the Service unbinds, or when the app crashes
		public void OnServiceDisconnected (ComponentName name)
		{
			this.binder.IsBound = false;
			Log.Debug ( "ServiceConnection", "Service unbound" );
		}
	}
}

