using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using MWC.BL;
using MWC;
using Android.GoogleMaps;
using Android.Views;
using Android.Content.PM;

namespace MWC.Android.Screens {
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
            var pin = Resources.GetDrawable(Resource.Drawable.pin);
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


         /// <summary>
        /// http://mgroves.com/monodroid-creating-an-options-menu/ 
        /// </summary>
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            var item = menu.Add(Menu.None, 1, 1, new Java.Lang.String("Home"));
            return true;
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            var intent = new Intent();
            switch (item.TitleFormatted.ToString()) {
            case "Home":
                intent.SetClass(this, typeof(TabBar));
                intent.AddFlags(ActivityFlags.ClearTop);            // http://developer.android.com/reference/android/content/Intent.html#FLAG_ACTIVITY_CLEAR_TOP
                StartActivity(intent);
                return true;
            default:
                // generally shouldn't happen...
                return base.OnOptionsItemSelected(item);
            }
        }
    }
}
