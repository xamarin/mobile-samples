using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;

namespace MWC.Android.Screens
{
    /// <summary>
    /// http://docs.xamarin.com/android/tutorials/User_Interface/tab_layout
    /// </summary>
    [Activity(MainLauncher = true, Label = "Mobile World Congress", Theme = "@android:style/Theme.NoTitleBar", Icon = "@drawable/icon")]
    public class TabBar : TabActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.TabBar);

            TabHost.TabSpec spec;     // Resusable TabSpec for each tab
            Intent intent;            // Reusable Intent for each tab

            // Create an Intent to launch an Activity for the tab (to be reused)
            intent = new Intent(this, typeof(HomeScreen));
            intent.AddFlags(ActivityFlags.NewTask);

            // Initialize a TabSpec for each tab and add it to the TabHost
            spec = TabHost.NewTabSpec("home");
            spec.SetIndicator("Schedule", Resources.GetDrawable(Resource.Drawable.Icon));
            spec.SetContent(intent);
            TabHost.AddTab(spec);

            // Do the same for the other tabs
            // ------------
            intent = new Intent(this, typeof(SpeakersScreen));
            intent.AddFlags(ActivityFlags.NewTask);

            spec = TabHost.NewTabSpec("speakers");
            spec.SetIndicator("Speakers", Resources.GetDrawable(Resource.Drawable.Icon));
            spec.SetContent(intent);
            TabHost.AddTab(spec);

            // ------------
            intent = new Intent(this, typeof(SessionsScreen));
            intent.AddFlags(ActivityFlags.NewTask);

            spec = TabHost.NewTabSpec("sessions");
            spec.SetIndicator("Sessions", Resources.GetDrawable(Resource.Drawable.Icon));
            spec.SetContent(intent);
            TabHost.AddTab(spec);

            // ------------
            intent = new Intent(this, typeof(MapScreen));
            intent.AddFlags(ActivityFlags.NewTask);

            spec = TabHost.NewTabSpec("map");
            spec.SetIndicator("Map", Resources.GetDrawable(Resource.Drawable.Icon));
            spec.SetContent(intent);
            TabHost.AddTab(spec);

            // ------------
            intent = new Intent(this, typeof(ExhibitorsScreen));
            intent.AddFlags(ActivityFlags.NewTask);

            spec = TabHost.NewTabSpec("exhibitors");
            spec.SetIndicator("Exhibitors", Resources.GetDrawable(Resource.Drawable.Icon));
            spec.SetContent(intent);
            TabHost.AddTab(spec);

            // ------------
            intent = new Intent(this, typeof(TwitterScreen));
            intent.AddFlags(ActivityFlags.NewTask);

            spec = TabHost.NewTabSpec("twitter");
            spec.SetIndicator("Twitter", Resources.GetDrawable(Resource.Drawable.Icon));
            spec.SetContent(intent);
            TabHost.AddTab(spec);

            // ------------
            intent = new Intent(this, typeof(NewsScreen));
            intent.AddFlags(ActivityFlags.NewTask);

            spec = TabHost.NewTabSpec("news");
            spec.SetIndicator("News", Resources.GetDrawable(Resource.Drawable.Icon));
            spec.SetContent(intent);
            TabHost.AddTab(spec);


            // ------------
            intent = new Intent(this, typeof(FavoritesScreen));
            intent.AddFlags(ActivityFlags.NewTask);

            spec = TabHost.NewTabSpec("favorites");
            spec.SetIndicator("Favorites", Resources.GetDrawable(Resource.Drawable.Icon));
            spec.SetContent(intent);
            TabHost.AddTab(spec);

            // ------------
            intent = new Intent(this, typeof(AboutXamScreen));
            intent.AddFlags(ActivityFlags.NewTask);

            spec = TabHost.NewTabSpec("about");
            spec.SetIndicator("About Xamarin", Resources.GetDrawable(Resource.Drawable.Icon));
            spec.SetContent(intent);
            TabHost.AddTab(spec);
            
            
            
            TabHost.CurrentTab = 0;
        }
    }
}