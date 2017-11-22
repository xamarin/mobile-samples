using System;
using Android.OS;

namespace Location.Droid.Services
{
	public class ServiceConnectedEventArgs : EventArgs
	{
		public IBinder Binder { get; set; }
	}
}