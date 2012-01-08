using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using MWC.BL;
using MWC;

namespace MWC.Android.Screens
{
    [Activity(Label = "Exhibitor")]
    public class ExhibitorDetailsScreen : BaseScreen
    {
        Exhibitor _exhibitor;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.ExhibitorDetailsScreen);

            var id = Intent.GetIntExtra("ExhibitorID", -1);

            if (id >= 0)
            {
                _exhibitor = BL.Managers.ExhibitorManager.GetExhibitor(id);
                if (_exhibitor != null)
                {
                    FindViewById<TextView>(Resource.Id.NameTextView).Text = _exhibitor.Name;
                    FindViewById<TextView>(Resource.Id.LocationTextView).Text = _exhibitor.City + ", " + _exhibitor.Country;
                }
                else
                {   // shouldn't happen...
                    FindViewById<TextView>(Resource.Id.NameTextView).Text = "Exhibitor not found: " + id;
                }
            }
        }
    }
}