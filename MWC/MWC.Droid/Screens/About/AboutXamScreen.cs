using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Webkit;

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

            var web = FindViewById<WebView>(Resource.Id.AboutWebView);
            web.LoadUrl("file:///android_asset/About/index.html");
        }
    }
}