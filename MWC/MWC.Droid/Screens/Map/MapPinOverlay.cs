using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.GoogleMaps;
using Android.Graphics.Drawables;

using MWC;
using Android.Graphics;
namespace MWC.Android.Screens {
    class MapPinOverlay : ItemizedOverlay {
        List<OverlayItem> pins;

        public MapPinOverlay(Drawable pin)
            : base(pin)
        {
            // populate some sample location data for the overlay items
            pins = new List<OverlayItem>{
					new OverlayItem (new GeoPoint((int)(Constants.MapPinLatitude * 1e6), (int)(Constants.MapPinLongitude * 1e6)), null, null),
				};

            BoundCenterBottom(pin);
            Populate();
        }

        protected override Java.Lang.Object CreateItem(int i)
        {
            var item = pins[i];
            return item;
        }

        public override int Size()
        {
            return pins.Count();
        }
    }
}