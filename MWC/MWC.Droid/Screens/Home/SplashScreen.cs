using Android.App;
using Android.Content.PM;
using Android.OS;

namespace MWC.Android.Screens {

    [Activity(MainLauncher = true
        , Label = "@string/ApplicationName"
        , Theme = "@style/Theme.Splash"
        , Icon = "@drawable/icon"
        , ScreenOrientation = ScreenOrientation.Portrait
        , NoHistory = true)]
    public class SplashScreen : Activity {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            StartActivity(typeof(MWC.Android.Screens.TabBar));
        }
    }
}