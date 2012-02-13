using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using MWC.BL;
using MWC;
using Android.GoogleMaps;

namespace MWC.Android.Screens {
    [Activity(Label = "Map")]
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
    }
}
