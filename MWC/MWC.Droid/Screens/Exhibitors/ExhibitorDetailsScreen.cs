using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Widget;
using MWC.BL;

namespace MWC.Android.Screens {
    [Activity(Label = "Exhibitor", ScreenOrientation = ScreenOrientation.Portrait)]
    public class ExhibitorDetailsScreen : BaseScreen, MonoTouch.Dialog.Utilities.IImageUpdated {
        Exhibitor exhibitor;
        ImageView imageview;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.ExhibitorDetailsScreen);

            var id = Intent.GetIntExtra("ExhibitorID", -1);

            if (id >= 0) {
                exhibitor = BL.Managers.ExhibitorManager.GetExhibitor(id);
                if (exhibitor != null) {
                    FindViewById<TextView>(Resource.Id.NameTextView).Text = exhibitor.Name;
                    FindViewById<TextView>(Resource.Id.CountryTextView).Text = exhibitor.FormattedCityCountry;
                    FindViewById<TextView>(Resource.Id.LocationTextView).Text = exhibitor.Locations;
                    if (!String.IsNullOrEmpty(exhibitor.Overview))
                        FindViewById<TextView>(Resource.Id.DescriptionTextView).Text = exhibitor.Overview;
                    else
                        FindViewById<TextView>(Resource.Id.DescriptionTextView).Text = "No background information available.";
                    // now do the image
                    imageview = FindViewById<ImageView>(Resource.Id.ExhibitorImageView);
                    var uri = new Uri(exhibitor.ImageUrl);
                    Console.WriteLine("speaker.ImageUrl " + exhibitor.ImageUrl);
                    try {
                        var drawable = MonoTouch.Dialog.Utilities.ImageLoader.DefaultRequestImage(uri, this);
                        if (drawable != null)
                            imageview.SetImageDrawable(drawable);
                    } catch (Exception ex) {
                        Console.WriteLine(ex.ToString());
                    }
                } else {   // shouldn't happen...
                    FindViewById<TextView>(Resource.Id.NameTextView).Text = "Exhibitor not found: " + id;
                }
            }
        }

        public void UpdatedImage(Uri uri)
        {
            Console.WriteLine("speaker.ImageUrl CALLBACK ");
            RunOnUiThread(() => {
                var drawable = MonoTouch.Dialog.Utilities.ImageLoader.DefaultRequestImage(uri, this);
                imageview.SetImageDrawable(drawable);
            });
        }
    }
}