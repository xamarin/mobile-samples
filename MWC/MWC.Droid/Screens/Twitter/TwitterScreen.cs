using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using MWC.BL;
using MWC;
using MWC.SAL;
using System;

namespace MWC.Android.Screens
{
    [Activity(Label = "Twitter")]
    public class TwitterScreen : BaseScreen
    {
        MWC.Adapters.TwitterListAdapter _twitterListAdapter;
        ListView _twitterListView = null;

        public IList<BL.Tweet> TwitterFeed;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // set our layout to be the home screen
            this.SetContentView(Resource.Layout.TwitterScreen);

            //Find our controls
            this._twitterListView = FindViewById<ListView>(Resource.Id.TweetList);

            // wire up task click handler
            if (this._twitterListView != null)
            {
                this._twitterListView.ItemClick += (object sender, ItemEventArgs e) =>
                {
                    var tweetDetails = new Intent(this, typeof(TweetDetailsScreen));
                    tweetDetails.PutExtra("TweetID", this.TwitterFeed[e.Position].ID);
                    this.StartActivity(tweetDetails);
                };
            }

            // get the tweets 
            TwitterFeed = BL.Managers.TwitterFeedManager.GetTweets();
            if (TwitterFeed.Count == 0)
            {
                BL.Managers.TwitterFeedManager.Update();
            }
            else
            {
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
            this._twitterListAdapter = new MWC.Adapters.TwitterListAdapter(this, TwitterFeed);
            this._twitterListView.Adapter = this._twitterListAdapter;
        }
    }
}