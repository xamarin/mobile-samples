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

namespace MWC.Android.Screens {

    [Activity(MainLauncher = true
        , Label = "@string/ApplicationName"
        , Theme = "@style/Theme.Splash"
        , Icon = "@drawable/icon"
        , NoHistory = true)]
    public class SplashScreen : Activity {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            StartActivity(typeof(MWC.Android.Screens.TabBar));
        }
    }
}