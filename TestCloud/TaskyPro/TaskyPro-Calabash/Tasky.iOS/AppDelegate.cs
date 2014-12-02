using Foundation;
using UIKit;
using Xamarin;
using Tasky.Screens.iPhone;


namespace Tasky
{
    [Register("AppDelegate")]
    public class AppDelegate : UIApplicationDelegate
    {
        // class-level declarations
        UITableViewController homeViewController;
        UINavigationController navController;
        UIWindow window;

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            // create a new window instance based on the screen size
            window = new UIWindow(UIScreen.MainScreen.Bounds);

            // make the window visible
            window.MakeKeyAndVisible();

            // create our nav controller
            navController = new UINavigationController();

            // create our home controller based on the device
            if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone)
            {
                homeViewController = new HomeScreen();
            }
            else
            {
                homeViewController = new HomeScreen(); // TODO: replace with iPad screen if we implement for iPad
            }

            // Styling
            UINavigationBar.Appearance.TintColor = UIColor.FromRGB(38, 117, 255); // nice blue
            UITextAttributes ta = new UITextAttributes();
            ta.Font = UIFont.FromName("AmericanTypewriter-Bold", 0f);
            UINavigationBar.Appearance.SetTitleTextAttributes(ta);
            ta.Font = UIFont.FromName("AmericanTypewriter", 0f);
            UIBarButtonItem.Appearance.SetTitleTextAttributes(ta, UIControlState.Normal);

            // push the view controller onto the nav controller and show the window
            navController.PushViewController(homeViewController, false);
            window.RootViewController = navController;
            window.MakeKeyAndVisible();

#if DEBUG
            Calabash.Start();
#endif

            return true;
        }
    }
}
