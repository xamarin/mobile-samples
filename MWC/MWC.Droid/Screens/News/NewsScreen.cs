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
    [Activity(Label = "News")]
    public class NewsScreen : BaseScreen
    {
        MWC.Adapters.NewsListAdapter _newsList;
        IList<RSSEntry> _news = new List<RSSEntry>();
        ListView _newsListView = null;

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
                    var sessionDetails = new Intent(this, typeof(SessionDetailsScreen));
                    sessionDetails.PutExtra("NewsID", e.Position);
                    this.StartActivity(sessionDetails);
                };
            }

            var parser = new RSSParser<RSSEntry>(MWCApp.NewsUrl);
            parser.Refresh(delegate {
                RunOnUiThread(() => {
                    _news = parser.AllItems;
                    this._newsList = new MWC.Adapters.NewsListAdapter(this, this._news);
                    this._newsListView.Adapter = this._newsList;
                }); 
            });
        }

        protected override void OnResume()
        {
            base.OnResume();

            // create our adapter
            this._newsList = new MWC.Adapters.NewsListAdapter(this, this._news);

            //Hook up our adapter to our ListView
            this._newsListView.Adapter = this._newsList;
        }
    }
}