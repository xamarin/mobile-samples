using Android.App;
using Android.Content.PM;
using Android.OS;

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
        }
    }
}