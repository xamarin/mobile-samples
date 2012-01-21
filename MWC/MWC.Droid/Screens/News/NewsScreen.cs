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
    [Activity(Label = "News")]
    public class NewsScreen : BaseScreen
    {
        MWC.Adapters.NewsListAdapter _newsListAdapter;
        ListView _newsListView = null;

        public IList<BL.RSSEntry> NewsFeed;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // set our layout to be the home screen
            this.SetContentView(Resource.Layout.NewsScreen);

            //Find our controls
            this._newsListView = FindViewById<ListView>(Resource.Id.NewsList);

            // wire up task click handler
            if (this._newsListView != null)
            {
                this._newsListView.ItemClick += (object sender, ItemEventArgs e) =>
                {
                    var newsDetails = new Intent(this, typeof(NewsDetailsScreen));
                    newsDetails.PutExtra("NewsID", this.NewsFeed[e.Position].ID);
                    this.StartActivity(newsDetails);
                };
            }

            // get the tweets 
            NewsFeed = BL.Managers.NewsManager.GetNews();
            if (NewsFeed.Count == 0)
            {
                BL.Managers.NewsManager.Update();
            }
            else
            {
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
            NewsFeed = BL.Managers.NewsManager.GetNews();
            RunOnUiThread(() =>
            {
                PopulateData();
            });
        }
        void PopulateData()
        {
            this._newsListAdapter = new MWC.Adapters.NewsListAdapter(this, NewsFeed);
            this._newsListView.Adapter = this._newsListAdapter;
        }
    }
}