using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using MWC.BL;
using MWC;
using System;
using MWC.SAL;
using Android.Webkit;

namespace MWC.Android.Screens
{
    [Activity(Label = "Tweet")]
    public class TweetDetailsScreen : BaseScreen
    {
        Tweet _tweet;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.TweetDetailsScreen);

            var id = Intent.GetIntExtra("TweetID", -1);

            if (id >= 0)
            {
                _tweet = BL.Managers.TwitterFeedManager.GetTweet(id);
                if (_tweet != null)
                {
                    FindViewById<TextView>(Resource.Id.NameTextView).Text = _tweet.RealName;
                    FindViewById<TextView>(Resource.Id.TimeTextView).Text = _tweet.FormattedTime;
                    FindViewById<TextView>(Resource.Id.HandleTextView).Text = _tweet.FormattedAuthor;
                    FindViewById<WebView>(Resource.Id.ContentWebView).LoadData(
                        "<html><body>" + _tweet.Content + "</body></html>", @"text/html", null);
                }
                else
                {   // shouldn't happen...
                    FindViewById<TextView>(Resource.Id.TitleTextView).Text = "Tweet not found: " + id;
                }
            }
        }
    }
}