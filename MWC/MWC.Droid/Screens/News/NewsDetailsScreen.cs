using Android.App;
using Android.Content;
using Android.OS;
using Android.Webkit;
using Android.Widget;
using MWC.BL;
using Android.Content.PM;

namespace MWC.Android.Screens {
    [Activity(Label = "News", ScreenOrientation = ScreenOrientation.Portrait)]
    class NewsDetailsScreen : BaseScreen {
        RSSEntry newsItem;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.NewsDetailsScreen);

            var id = Intent.GetIntExtra("NewsID", -1);

            if (id >= 0) {
                newsItem = BL.Managers.NewsManager.Get(id);
                if (newsItem != null) {
                    FindViewById<TextView>(Resource.Id.TitleTextView).Visibility = global::Android.Views.ViewStates.Gone;
 
                    FindViewById<WebView>(Resource.Id.ContentWebView).LoadDataWithBaseURL(null,
                                "<html><body>" + newsItem.Content + "</body></html>", @"text/html", "utf-8", null);
                    // ugh - LoadData() method has problems when html contains a % SO USE LoadDataWithBaseURL instead even though we don't have a BaseURL
                    // http://code.google.com/p/android/issues/detail?id=1733
                    // http://code.google.com/p/android/issues/detail?id=4401
                } else {   // shouldn't happen...
                    var text = FindViewById<TextView>(Resource.Id.TitleTextView);
                    text.Text = "NewsItem not found: " + id;
                    text.Visibility = global::Android.Views.ViewStates.Visible;
                }
            }
        }
    }
}