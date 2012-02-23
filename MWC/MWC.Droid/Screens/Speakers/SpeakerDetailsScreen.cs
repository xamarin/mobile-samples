using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Widget;
using MWC.BL;
using System.Net;
using Android.Graphics.Drawables;
using System.IO;

namespace MWC.Android.Screens {
    [Activity(Label = "Speaker", ScreenOrientation = ScreenOrientation.Portrait)]
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
                    MWCApp.LogDebug("speaker.ImageUrl " + speaker.ImageUrl);
                    try {
                        var drawable = MonoTouch.Dialog.Utilities.ImageLoader.DefaultRequestImage(uri, this);
                        if (drawable != null) // use it
                            imageview.SetImageDrawable(drawable);
                        else // we're just going to grab it ourselves and not wait for the callback from ImageLoader
                            LoadImageDirectly(uri);
                        
                    } catch (Exception ex) {
                        MWCApp.LogDebug(ex.ToString());
                    }

                } else {   // shouldn't happen...
                    FindViewById<TextView>(Resource.Id.TitleTextView).Text = "Speaker not found: " + id;
                }
            }
        }

        public void LoadImageDirectly(Uri uri)
        {
            var webClient = new WebClient();
            MWCApp.LogDebug("Get speaker image directly, bypassing the ImageLoader which is taking too long");
            webClient.DownloadDataCompleted += (sender, e) => {
                try {
                    //var image = new global::Android.Graphics.Drawables.BitmapDrawable(bitmap);
                    var byteArray = e.Result;
                    MemoryStream ms = new MemoryStream(byteArray, 0, byteArray.Length);
                    ms.Write(byteArray, 0, byteArray.Length);
                    ms.Position = 0;
                    var d = new global::Android.Graphics.Drawables.BitmapDrawable(ms);
                    RunOnUiThread(() => {
                        MWCApp.LogDebug("DETAILS speaker.ImageUrl");
                        imageview.SetImageDrawable(d);
                    });
                } catch (Exception ex) {
                    MWCApp.LogDebug("Image error: " + ex);
                }
            };
            webClient.DownloadDataAsync(uri);
        }

        /// <summary>
        /// temporarily disabled this in favor of the LoadImageDirectly method
        /// until i figure out why this isn't always called-back
        /// </summary>
        public void UpdatedImage(Uri uri)
        {
            MWCApp.LogDebug("IGNORING... speaker.ImageUrl CALLBACK");
            //var drawable = MonoTouch.Dialog.Utilities.ImageLoader.DefaultRequestImage(uri, this);
            //RunOnUiThread(() => {
            //    imageview.SetImageDrawable(drawable);
            //});
        }
    }
}