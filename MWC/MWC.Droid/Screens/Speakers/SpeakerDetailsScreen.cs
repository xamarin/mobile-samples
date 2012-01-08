using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using MWC.BL;
using MWC;
using System;

namespace MWC.Android.Screens
{
    [Activity(Label = "Speaker")]
    public class SpeakerDetailsScreen : BaseScreen
    {
        Speaker _speaker;
        
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.SpeakerDetailsScreen);

            var id = Intent.GetIntExtra("SpeakerID", -1);

            if (id >= 0)
            {
                _speaker = BL.Managers.SpeakerManager.GetSpeaker(id);
                if (_speaker != null)
                {
                    FindViewById<TextView>(Resource.Id.Name).Text = _speaker.Name;
                    FindViewById<TextView>(Resource.Id.CompanyTextView).Text = _speaker.Company;

                    if (!String.IsNullOrEmpty(_speaker.Bio))
                        FindViewById<TextView>(Resource.Id.Bio).Text = _speaker.Bio;
                    else
                    {
                        var tv = FindViewById<TextView>(Resource.Id.Bio);
                        tv.Text = "no speaker bio available";
                    }
                }
                else
                {   // shouldn't happen...
                    FindViewById<TextView>(Resource.Id.Title).Text = "Session not found: " + id;
                }
            }
        }
    }
}