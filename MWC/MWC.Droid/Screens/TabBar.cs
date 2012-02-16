using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Widget;

namespace MWC.Android.Screens {
    /// <summary>
    /// http://docs.xamarin.com/android/tutorials/User_Interface/tab_layout
    /// </summary>
    /// <remarks>
    /// Icon design guidelines
    /// http://developer.android.com/guide/practices/ui_guidelines/icon_design_tab.html
    /// </remarks>
    [Activity(Label = "Home"
            , Theme = "@android:style/Theme.NoTitleBar"
            , ScreenOrientation = ScreenOrientation.Portrait)]
    public class TabBar : TabActivity {
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
            spec.SetIndicator("Schedule", Resources.GetDrawable(Resource.Drawable.tab_schedule));
            spec.SetContent(intent);
            TabHost.AddTab(spec);

            // Do the same for the other tabs
            // ------------
            intent = new Intent(this, typeof(SpeakersScreen));
            intent.AddFlags(ActivityFlags.NewTask);

            spec = TabHost.NewTabSpec("speakers");
            spec.SetIndicator("Speakers", Resources.GetDrawable(Resource.Drawable.tab_speakers));
            spec.SetContent(intent);
            TabHost.AddTab(spec);

            // ------------
            intent = new Intent(this, typeof(SessionsScreen));
            intent.AddFlags(ActivityFlags.NewTask);

            spec = TabHost.NewTabSpec("sessions");
            spec.SetIndicator("Sessions", Resources.GetDrawable(Resource.Drawable.tab_sessions));
            spec.SetContent(intent);
            TabHost.AddTab(spec);

            // ------------
            intent = new Intent(this, typeof(MapScreen));
            intent.AddFlags(ActivityFlags.NewTask);

            spec = TabHost.NewTabSpec("map");
            spec.SetIndicator("Map", Resources.GetDrawable(Resource.Drawable.tab_maps));
            spec.SetContent(intent);
            TabHost.AddTab(spec);

            // ------------
            intent = new Intent(this, typeof(MoreScreen));
            intent.AddFlags(ActivityFlags.NewTask);

            spec = TabHost.NewTabSpec("more");
            spec.SetIndicator("", Resources.GetDrawable(global::Android.Resource.Drawable.IcMenuMore));   // android.R.drawable.ic_menu_more
            spec.SetContent(intent);
            TabHost.AddTab(spec);
            
            //// ------------
            //intent = new Intent(this, typeof(ExhibitorsScreen));
            //intent.AddFlags(ActivityFlags.NewTask);

            //spec = TabHost.NewTabSpec("exhibitors");
            //spec.SetIndicator("Exhibitors", Resources.GetDrawable(Resource.Drawable.tab_exhibitors));
            //spec.SetContent(intent);
            //TabHost.AddTab(spec);

            //// ------------
            //intent = new Intent(this, typeof(TwitterScreen));
            //intent.AddFlags(ActivityFlags.NewTask);

            //spec = TabHost.NewTabSpec("twitter");
            //spec.SetIndicator("Twitter", Resources.GetDrawable(Resource.Drawable.tab_twitter));
            //spec.SetContent(intent);
            //TabHost.AddTab(spec);

            //// ------------
            //intent = new Intent(this, typeof(NewsScreen));
            //intent.AddFlags(ActivityFlags.NewTask);

            //spec = TabHost.NewTabSpec("news");
            //spec.SetIndicator("News", Resources.GetDrawable(Resource.Drawable.tab_rss));
            //spec.SetContent(intent);
            //TabHost.AddTab(spec);


            //// ------------
            //intent = new Intent(this, typeof(FavoritesScreen));
            //intent.AddFlags(ActivityFlags.NewTask);

            //spec = TabHost.NewTabSpec("favorites");
            //spec.SetIndicator("Favorites", Resources.GetDrawable(Resource.Drawable.tab_favorites));
            //spec.SetContent(intent);
            //TabHost.AddTab(spec);

            //// ------------
            //intent = new Intent(this, typeof(AboutXamScreen));
            //intent.AddFlags(ActivityFlags.NewTask);

            //spec = TabHost.NewTabSpec("about");
            //spec.SetIndicator("About Xamarin", Resources.GetDrawable(Resource.Drawable.tab_about));
            //spec.SetContent(intent);
            //TabHost.AddTab(spec);


            //var tab = Intent.GetStringExtra("tabName");
            //switch (tab)
            //{
            //    case "speakers":
            //        TabHost.CurrentTab = 1;
            //        break;
            //    case "sessions":
            //        TabHost.CurrentTab = 2;
            //        break;
            //    case "map":
            //        TabHost.CurrentTab = 3;
            //        break;
            //    case "more":
            //        TabHost.CurrentTab = 4;
            //        break;
            //    default:
            //        TabHost.CurrentTab = 0;
            //        break;
            //}
        }
    }
}