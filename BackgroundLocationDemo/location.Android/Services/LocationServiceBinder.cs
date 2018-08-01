using System;

using Android.OS;

namespace Location.Droid.Services
{
    //This is our Binder subclass, the LocationServiceBinder
    public class LocationServiceBinder : Binder
    {
        protected LocationService service;

        // constructor
        public LocationServiceBinder(LocationService service)
        {
            this.service = service;
        }

        public LocationService Service => service;

        public bool IsBound { get; set; }
    }
}
