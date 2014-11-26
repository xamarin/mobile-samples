using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace CreditCardValidation.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to
    // application events from iOS.
    [Register("AppDelegate")]
    public class AppDelegate : UIApplicationDelegate
    {
        // class-level declarations
        UIViewController viewController;
        UIWindow window;
        //
        // This method is invoked when the application has loaded and is ready to run. In this
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {

            #region Code for starting up the Xamarin Test Cloud Agent
            #if DEBUG
            // This is not necessary for Android applications
            Xamarin.Calabash.Start();
            #endif
            #endregion

            window = new UIWindow(UIScreen.MainScreen.Bounds);

            viewController = new UINavigationController(new CreditCardValidationScreen());
            window.RootViewController = viewController;
            window.MakeKeyAndVisible();

            return true;
        }
    }
}
