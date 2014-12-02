using System;
using Foundation;
using UIKit;
using CocosSharp;

namespace GoneBananas
{
    [Register ("AppDelegate")]
    public partial class AppDelegate : UIApplicationDelegate
    {
        public override void FinishedLaunching (UIApplication app)
        {
            var application = new CCApplication ();
            application.ApplicationDelegate = new GoneBananasApplicationDelegate ();
            application.StartGame ();
        }
    }
}