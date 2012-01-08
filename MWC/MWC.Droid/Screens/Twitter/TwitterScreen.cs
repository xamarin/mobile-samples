using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using MWC.BL;
using MWC;
using MWC.SAL;

namespace MWC.Android.Screens
{
    [Activity(Label = "Twitter")]
    public class TwitterScreen : BaseScreen
    {
        MWC.Adapters.TwitterListAdapter _twitterListAdapter;
        IList<Tweet> _tweets = new List<Tweet>();
        ListView _twitterListView = null;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // set our layout to be the home screen
            this.SetContentView(Resource.Layout.NewsScreen);

            //Find our controls
            this._twitterListView = FindViewById<ListView>(Resource.Id.NewsList);

            // wire up task click handler
            if (this._twitterListView != null)
            {
                this._twitterListView.ItemClick += (object sender, ItemEventArgs e) =>
                {
                    var tweetDetails = new Intent(this, typeof(TweetDetailsScreen));
                    tweetDetails.PutExtra("TweetID", e.Position);
                    this.StartActivity(tweetDetails);
                };
            }

            var parser = new TwitterParser<Tweet>(MWCApp.TwitterUrl);

            parser.Refresh(delegate
            {
                RunOnUiThread(() =>
                {
                    _tweets = parser.AllItems;
                    this._twitterListAdapter = new MWC.Adapters.TwitterListAdapter(this, this._tweets);
                    this._twitterListView.Adapter = this._twitterListAdapter;
                });
            });
        }

        protected override void OnResume()
        {
            base.OnResume();
            this._twitterListAdapter = new MWC.Adapters.TwitterListAdapter(this, this._tweets);
            this._twitterListView.Adapter = this._twitterListAdapter;
        }
    }
}