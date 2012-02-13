using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using MWC.BL;
using MWC;
using MWC.SAL;
using System;

namespace MWC.Android.Screens {
    [Activity(Label = "Twitter")]
    public class TwitterScreen : BaseScreen {
        MWC.Adapters.TwitterListAdapter twitterListAdapter;
        ListView twitterListView = null;
        RelativeLayout loadingPanel, emptyPanel;
        Button refreshButton;

        public IList<BL.Tweet> TwitterFeed;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // set our layout to be the home screen
            this.SetContentView(Resource.Layout.TwitterScreen);

            //Find our controls
            twitterListView = FindViewById<ListView>(Resource.Id.TweetList);
            loadingPanel = FindViewById<RelativeLayout>(Resource.Id.LoadingPanel);
            emptyPanel = FindViewById<RelativeLayout>(Resource.Id.EmptyPanel);
            refreshButton = FindViewById<Button>(Resource.Id.RefreshButton);

            // wire up task click handler
            if (this.twitterListView != null) {
                this.twitterListView.ItemClick += (object sender, ItemEventArgs e) => {
                    var tweetDetails = new Intent(this, typeof(TweetDetailsScreen));
                    tweetDetails.PutExtra("TweetID", TwitterFeed[e.Position].ID);
                    StartActivity(tweetDetails);
                };
            }

            refreshButton.Click += (object sender, EventArgs e) =>
            {
                loadingPanel.Visibility = global::Android.Views.ViewStates.Visible;
                emptyPanel.Visibility = global::Android.Views.ViewStates.Invisible;
                BL.Managers.TwitterFeedManager.Update();
            };

            TwitterFeed = BL.Managers.TwitterFeedManager.GetTweets();
            if (TwitterFeed.Count == 0) { 
                // whoops there isn't any, get new news
                emptyPanel.Visibility = global::Android.Views.ViewStates.Invisible;
                loadingPanel.Visibility = global::Android.Views.ViewStates.Visible;
                BL.Managers.TwitterFeedManager.Update();
            } else {
                emptyPanel.Visibility = global::Android.Views.ViewStates.Invisible;
                loadingPanel.Visibility = global::Android.Views.ViewStates.Invisible;
                PopulateData();
            }
        }

        protected override void OnResume()
        {
            base.OnResume();
            BL.Managers.TwitterFeedManager.UpdateFinished += HandleUpdateFinished;
            
        }
        protected override void OnPause()
        {
            base.OnPause();
            BL.Managers.TwitterFeedManager.UpdateFinished -= HandleUpdateFinished;
        }
        void HandleUpdateFinished(object sender, EventArgs ea)
        {
            // assume we can 'Get()' them, since update has finished
            TwitterFeed = BL.Managers.TwitterFeedManager.GetTweets();
            RunOnUiThread(() =>
            {
                PopulateData();
            });
        }
        void PopulateData()
        {
            this.twitterListAdapter = new MWC.Adapters.TwitterListAdapter(this, TwitterFeed);
            this.twitterListView.Adapter = this.twitterListAdapter;
            if (TwitterFeed.Count == 0)
                emptyPanel.Visibility = global::Android.Views.ViewStates.Visible;
            else
                emptyPanel.Visibility = global::Android.Views.ViewStates.Invisible;
            loadingPanel.Visibility = global::Android.Views.ViewStates.Invisible;
        }
    }
}