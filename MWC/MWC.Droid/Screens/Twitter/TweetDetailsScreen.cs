using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using MWC.BL;
using MWC;
using System;
using MWC.SAL;

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
                _tweet = new Tweet();// BL.Managers.SessionManager.GetSession(id);
                if (_tweet != null)
                {
                    FindViewById<TextView>(Resource.Id.TitleTextView).Text = _tweet.Title;
                    FindViewById<TextView>(Resource.Id.PublishedTextView).Text = _tweet.Published.ToString("d MMM yy");
                    FindViewById<TextView>(Resource.Id.ContentTextView).Text = _tweet.Content;
                }
                else
                {   // shouldn't happen...
                    FindViewById<TextView>(Resource.Id.Title).Text = "Tweet not found: " + id;
                }
            }
        }
    }
}