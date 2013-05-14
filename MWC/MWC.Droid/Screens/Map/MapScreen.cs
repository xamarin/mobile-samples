using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.GoogleMaps;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace MWC.Android.Screens {

#if !GOOGLEMAPSV1
	[Activity(Label = "Map", ScreenOrientation = ScreenOrientation.Portrait)]
	public class MapScreen : Activity
	{
		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			SetContentView(Resource.Layout.MapScreenV1NotSupported);
		}
#else
	// Only add the GOOGLEMAPSV1 compiler directive if you already have 
	// a Google Maps Version 1.0 API key. Google no longer supports this 
	// version of Google Maps - this sample code will be updated as soon
	// as possible.

    [Activity(Label = "Map", ScreenOrientation = ScreenOrientation.Portrait)]
    public class MapScreen : MapActivity
    {
        MyLocationOverlay myLocationOverlay;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.MapScreen);

            var map = FindViewById<MapView>(Resource.Id.map);

            map.Clickable = true;
            map.Traffic = false;
            map.Satellite = true;

            map.SetBuiltInZoomControls(true);
            map.Controller.SetZoom(15);
            map.Controller.SetCenter(new GeoPoint((int)(Constants.MapPinLatitude * 1e6), (int)(Constants.MapPinLongitude * 1e6)));
            
            AddMyLocationOverlay(map);
            AddPinOverlay(map);

            var animateButton = FindViewById<Button>(Resource.Id.animateButton);

            animateButton.Click += (sender, e) => {
                map.Controller.AnimateTo(
                    new GeoPoint((int)(Constants.MapPinLatitude * 1e6), (int)(Constants.MapPinLongitude * 1e6)), () => {
                        map.Controller.SetZoom(15);
                        var toast = Toast.MakeText(this, Constants.MapPinTitle, ToastLength.Short);
                        toast.Show();
                    });

            };
        }

        protected override bool IsRouteDisplayed {
            get {
                return false;
            }
        }

        void AddPinOverlay(MapView map)
        {
            var pin = Resources.GetDrawable(Resource.Drawable.pin_map);
            var pinOverlay = new MapPinOverlay(pin);
            map.Overlays.Add(pinOverlay);
        }

        void AddMyLocationOverlay(MapView map)
        {
            myLocationOverlay = new MyLocationOverlay(this, map);
            map.Overlays.Add(myLocationOverlay);

            myLocationOverlay.RunOnFirstFix(() => {
                map.Controller.AnimateTo(myLocationOverlay.MyLocation);

                RunOnUiThread(() => {
                    var toast = Toast.MakeText(this, "Located", ToastLength.Short);
                    toast.Show();
                });
            });
        }
        protected override void OnResume()
        {
            base.OnResume();
            myLocationOverlay.EnableMyLocation();
        }
        protected override void OnPause()
        {
            base.OnPause();
            myLocationOverlay.DisableMyLocation();
        }
#endif
        #region COPIED FROM BaseScreen.cs
        /// <summary>
        /// http://mgroves.com/monodroid-creating-an-options-menu/ 
        /// </summary>
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            var item = menu.Add(Menu.None, 1, 1, new Java.Lang.String("Schedule"));
            //item.SetIcon(Resource.Drawable.calendar);

            item = menu.Add(Menu.None, 2, 2, new Java.Lang.String("Speakers"));  // HACK: todo - add 'using' statement around Java.Lang.Strings for GC (as per novell hint)
            //item.SetIcon(Resource.Drawable.microphone);

            item = menu.Add(Menu.None, 3, 3, new Java.Lang.String("Sessions"));
            //item.SetIcon(Resource.Drawable.bullhorn);

            item = menu.Add(Menu.None, 4, 4, new Java.Lang.String("Map"));
            //item.SetIcon(Resource.Drawable.map);

            item = menu.Add(Menu.None, 5, 5, new Java.Lang.String("Favorites"));
            //item.SetIcon(Resource.Drawable.star);

            item = menu.Add(Menu.None, 6, 6, new Java.Lang.String("News"));
            //item.SetIcon(Resource.Drawable.star);

            item = menu.Add(Menu.None, 7, 7, new Java.Lang.String("Twitter"));
            //item.SetIcon(Resource.Drawable.star);

            item = menu.Add(Menu.None, 8, 8, new Java.Lang.String("Exhibitors"));
            //item.SetIcon(Resource.Drawable.star);

            item = menu.Add(Menu.None, 9, 9, new Java.Lang.String("About Xamarin"));
            //item.SetIcon(Resource.Drawable.star);

            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            var intent = new Intent();
            switch (item.TitleFormatted.ToString()) {
            case "Schedule":

            intent.SetClass(this, typeof(TabBar));
            intent.AddFlags(ActivityFlags.ClearTop);            // http://developer.android.com/reference/android/content/Intent.html#FLAG_ACTIVITY_CLEAR_TOP
            StartActivity(intent);
            return true;

            case "Speakers":

            intent.SetClass(this, typeof(SpeakersScreen));
            intent.AddFlags(ActivityFlags.ClearTop);            // http://developer.android.com/reference/android/content/Intent.html#FLAG_ACTIVITY_CLEAR_TOP
            StartActivity(intent);
            return true;

            case "Sessions":

            intent.SetClass(this, typeof(SessionsScreen));
            intent.AddFlags(ActivityFlags.ClearTop);            // http://developer.android.com/reference/android/content/Intent.html#FLAG_ACTIVITY_CLEAR_TOP
            StartActivity(intent);
            return true;

            case "Favorites":

            intent.SetClass(this, typeof(FavoritesScreen));
            StartActivity(intent);
            return true;

            case "News":

            intent.SetClass(this, typeof(NewsScreen));
            StartActivity(intent);
            return true;

            case "Twitter":

            intent.SetClass(this, typeof(TwitterScreen));
            StartActivity(intent);
            return true;

            case "Exhibitors":

            intent.SetClass(this, typeof(ExhibitorsScreen));
            StartActivity(intent);
            return true;

            case "About Xamarin":

            intent.SetClass(this, typeof(AboutXamScreen));
            StartActivity(intent);
            return true;

            case "Map":

            intent.SetClass(this, typeof(MapScreen));
            StartActivity(intent);
            return true;

            default:
            // generally shouldn't happen...
            return base.OnOptionsItemSelected(item);
            }
        }
        #endregion
    }
}
