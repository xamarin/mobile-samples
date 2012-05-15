using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Widget;

namespace MWC.Android.Screens {
    [Activity(Label = "News", ScreenOrientation = ScreenOrientation.Portrait)]
    public class NewsScreen : BaseScreen {
        MWC.Adapters.NewsListAdapter newsListAdapter;
        ListView newsListView = null;
        RelativeLayout loadingPanel, emptyPanel;
        Button refreshButton;

        public IList<BL.RSSEntry> NewsFeed;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // set our layout to be the home screen
            SetContentView(Resource.Layout.NewsScreen);

            //Find our controls
            newsListView = FindViewById<ListView>(Resource.Id.NewsList);
            loadingPanel = FindViewById<RelativeLayout>(Resource.Id.LoadingPanel);
            emptyPanel = FindViewById<RelativeLayout>(Resource.Id.EmptyPanel);
            refreshButton = FindViewById<Button>(Resource.Id.RefreshButton);

            // wire up task click handler
            if (newsListView != null) {
                newsListView.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) => {
                    var newsDetails = new Intent(this, typeof(NewsDetailsScreen));
                    newsDetails.PutExtra("NewsID", NewsFeed[e.Position].ID);
                    StartActivity(newsDetails);
                };
            }

            refreshButton.Click += (object sender, EventArgs e) => {
                loadingPanel.Visibility = global::Android.Views.ViewStates.Visible;
                emptyPanel.Visibility = global::Android.Views.ViewStates.Invisible;
                BL.Managers.NewsManager.Update();
            };

            NewsFeed = BL.Managers.NewsManager.Get();
            if (NewsFeed.Count == 0) {
                // whoops there isn't any, get new news
                emptyPanel.Visibility = global::Android.Views.ViewStates.Invisible;
                loadingPanel.Visibility = global::Android.Views.ViewStates.Visible;
                BL.Managers.NewsManager.Update();
            } else {
                // load existing news
                emptyPanel.Visibility = global::Android.Views.ViewStates.Invisible;
                loadingPanel.Visibility = global::Android.Views.ViewStates.Invisible;
                PopulateData();
            }
        }


        protected override void OnResume()
        {
            base.OnResume();
            BL.Managers.NewsManager.UpdateFinished += HandleUpdateFinished;
        }
        protected override void OnPause()
        {
            base.OnPause();
            BL.Managers.NewsManager.UpdateFinished -= HandleUpdateFinished;
        }
        void HandleUpdateFinished(object sender, EventArgs ea)
        {
            // assume we can 'Get()' them, since update has finished
            NewsFeed = BL.Managers.NewsManager.Get();
            RunOnUiThread(() => {
                PopulateData();
            });
        }
        void PopulateData()
        {
            newsListAdapter = new MWC.Adapters.NewsListAdapter(this, NewsFeed);
            newsListView.Adapter = newsListAdapter;
            if (NewsFeed.Count == 0)
                emptyPanel.Visibility = global::Android.Views.ViewStates.Visible;
            else
                emptyPanel.Visibility = global::Android.Views.ViewStates.Invisible;
            loadingPanel.Visibility = global::Android.Views.ViewStates.Invisible;
        }
    }
}