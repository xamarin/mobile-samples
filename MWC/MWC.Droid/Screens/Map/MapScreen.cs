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
            
            //var zoomInButton = FindViewById<Button>(Resource.Id.zoomInButton);
            //var zoomOutButton = FindViewById<Button>(Resource.Id.zoomOutButton);
            var animateButton = FindViewById<Button>(Resource.Id.animateButton);

            //zoomInButton.Click += (sender, e) =>
            //{
            //    map.Controller.ZoomIn();
            //    //map.Controller.ZoomInFixing (200, 200);
            //};

            //zoomOutButton.Click += (sender, e) =>
            //{
            //    map.Controller.ZoomOut();
            //    //map.Controller.ZoomOutFixing (200, 200);
            //};

            animateButton.Click += (sender, e) =>
            {
                //map.Controller.AnimateTo(new GeoPoint ((int)40.741773E6, (int)-74.004986E6));

                map.Controller.AnimateTo(
                    new GeoPoint((int)(Constants.MapPinLatitude * 1e6), (int)(Constants.MapPinLongitude * 1e6)), () =>
                    {
                        var toast = Toast.MakeText(this, Constants.MapPinTitle, ToastLength.Short);
                        toast.Show();
                    });

            };
        }

        protected override bool IsRouteDisplayed
        {
            get
            {
                return false;
            }
        }
    }
}
