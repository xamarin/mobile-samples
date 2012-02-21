using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Widget;
using Android.Content;

namespace MWC.Android.Screens
{
    [Activity(Label = "About Xamarin", ScreenOrientation = ScreenOrientation.Portrait)]
    public class AboutXamScreen : BaseScreen
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // set our layout to be the home screen
            this.SetContentView(Resource.Layout.AboutXamScreen);

            var txt = FindViewById<TextView>(Resource.Id.AboutTextView);
            txt.Text = Constants.AboutText;

            var so = FindViewById<ImageView>(Resource.Id.StackOverflowImage);
            so.Click += (object sender, System.EventArgs e) => {
                OpenUrl(Constants.AboutUrlStackOverflow);
            };
            var li = FindViewById<ImageView>(Resource.Id.LinkedInImage);
            li.Click += (object sender, System.EventArgs e) => {
                OpenUrl(Constants.AboutUrlLinkedIn);
            };
            var tw = FindViewById<ImageView>(Resource.Id.TwitterImage);
            tw.Click += (object sender, System.EventArgs e) => {
                OpenUrl(Constants.AboutUrlTwitter);
            };
            var yt = FindViewById<ImageView>(Resource.Id.YouTubeImage);
            yt.Click += (object sender, System.EventArgs e) => {
                OpenUrl(Constants.AboutUrlTwitter);
            };
            var fb = FindViewById<ImageView>(Resource.Id.FacebookImage);
            fb.Click += (object sender, System.EventArgs e) => {
                OpenUrl(Constants.AboutUrlFacebook);
            };
            var rs = FindViewById<ImageView>(Resource.Id.RssImage);
            rs.Click += (object sender, System.EventArgs e) => {
                OpenUrl(Constants.AboutUrlBlogRss);
            };
        }

        void OpenUrl (string url)
        {
            Intent browserIntent = new Intent(Intent.ActionView, global::Android.Net.Uri.Parse(url));
            StartActivity(browserIntent); 
        }
    }
}