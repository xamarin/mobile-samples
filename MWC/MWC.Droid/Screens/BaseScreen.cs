using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;

namespace MWC.Android.Screens
{
    public class BaseScreen : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
        }

        /// <summary>
        /// http://mgroves.com/monodroid-creating-an-options-menu/ 
        /// </summary>
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            var item = menu.Add(MenuConsts.None, 1, 1, new Java.Lang.String("Schedule"));
            //item.SetIcon(Resource.Drawable.calendar);

            item = menu.Add(MenuConsts.None, 2, 2, new Java.Lang.String("Speakers"));  // HACK: todo - add 'using' statement around Java.Lang.Strings for GC (as per novell hint)
            //item.SetIcon(Resource.Drawable.microphone);

            item = menu.Add(MenuConsts.None, 3, 3, new Java.Lang.String("Sessions"));
            //item.SetIcon(Resource.Drawable.bullhorn);

            item = menu.Add(MenuConsts.None, 4, 4, new Java.Lang.String("Map"));
            //item.SetIcon(Resource.Drawable.map);

            item = menu.Add(MenuConsts.None, 5, 5, new Java.Lang.String("Favorites"));
            //item.SetIcon(Resource.Drawable.star);

            item = menu.Add(MenuConsts.None, 6, 6, new Java.Lang.String("News"));
            //item.SetIcon(Resource.Drawable.star);

            item = menu.Add(MenuConsts.None, 7, 7, new Java.Lang.String("Twitter"));
            //item.SetIcon(Resource.Drawable.star);

            item = menu.Add(MenuConsts.None, 8, 8, new Java.Lang.String("Exhibitors"));
            //item.SetIcon(Resource.Drawable.star);

            item = menu.Add(MenuConsts.None, 9, 9, new Java.Lang.String("Xamarin"));
            //item.SetIcon(Resource.Drawable.star);

            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            var intent = new Intent();
            switch (item.TitleFormatted.ToString())
            {
                case "Schedule":

                    intent.SetClass(this, typeof(TabBar));
                    intent.AddFlags(ActivityFlags.ClearTop);            // http://developer.android.com/reference/android/content/Intent.html#FLAG_ACTIVITY_CLEAR_TOP
                    StartActivity(intent);
                    return true;

                case "Speakers":

                    intent.SetClass(this, typeof(SpeakersScreen));
                    intent.AddFlags(ActivityFlags.ClearTop);            // http://developer.android.com/reference/android/content/Intent.html#FLAG_ACTIVITY_CLEAR_TOP
                    StartActivity(intent);
                    return true;

                case "Sessions":

                    intent.SetClass(this, typeof(SessionsScreen));
                    intent.AddFlags(ActivityFlags.ClearTop);            // http://developer.android.com/reference/android/content/Intent.html#FLAG_ACTIVITY_CLEAR_TOP
                    StartActivity(intent);
                    return true;

                case "Favorites":

                    intent.SetClass(this, typeof(FavoritesScreen));
                    StartActivity(intent);
                    return true;
                
                case "News":

                    intent.SetClass(this, typeof(NewsScreen));
                    StartActivity(intent);
                    return true;

                case "Twitter":

                    intent.SetClass(this, typeof(TwitterScreen));
                    StartActivity(intent);
                    return true;

                case "Exhibitors":

                    intent.SetClass(this, typeof(ExhibitorsScreen));
                    StartActivity(intent);
                    return true;

                case "Xamarin":

                    intent.SetClass(this, typeof(AboutXamScreen));
                    StartActivity(intent);
                    return true;

                case "Map":

                    // TODO: implement some map stuff
                    //var geoUri = Android.Net.Uri.Parse ("geo:42.374260,-71.120824");
                    //var mapIntent = new Intent (Intent.ActionView, geoUri);
                    //StartActivity (mapIntent);

                default:
                    // generally shouldn't happen...
                    return base.OnOptionsItemSelected(item);
            }
        }
    }
}