using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Util;
using Android.Widget;
using MWC.BL;

namespace MWC.Android.Screens {
    [Activity(Label = "Speaker")]
    public class SpeakerDetailsScreen : BaseScreen, MonoTouch.Dialog.Utilities.IImageUpdated {
        Speaker speaker;
        ImageView imageview;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.SpeakerDetailsScreen);

            var id = Intent.GetIntExtra("SpeakerID", -1);

            if (id >= 0) {
                speaker = BL.Managers.SpeakerManager.GetSpeaker(id);
                if (speaker != null) {
                    FindViewById<TextView>(Resource.Id.NameTextView).Text = speaker.Name;
                    FindViewById<TextView>(Resource.Id.PositionTextView).Text = speaker.Title;
                    FindViewById<TextView>(Resource.Id.CompanyTextView).Text = speaker.Company;
                    imageview = FindViewById<ImageView>(Resource.Id.SpeakerImageView);

                    if (!String.IsNullOrEmpty(speaker.Bio)) {
                        FindViewById<TextView>(Resource.Id.Bio).Text = speaker.Bio;
                    } else {
                        var tv = FindViewById<TextView>(Resource.Id.Bio);
                        tv.Text = "no speaker bio available";
                    }

                    var uri = new Uri(speaker.ImageUrl);
                    Console.WriteLine("speaker.ImageUrl " + speaker.ImageUrl);
                    try {
                        var drawable = MonoTouch.Dialog.Utilities.ImageLoader.DefaultRequestImage(uri, this);
                        if (drawable != null) 
                            imageview.SetImageDrawable(drawable);
                    } catch (Exception ex) {
                        Console.WriteLine(ex.ToString());
                    }

                } else {   // shouldn't happen...
                    FindViewById<TextView>(Resource.Id.TitleTextView).Text = "Speaker not found: " + id;
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