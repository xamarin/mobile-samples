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
using Android.Util;

namespace MWC.Android.Screens
{
    [Activity(Label = "Tweet")]
    public class TweetDetailsScreen : BaseScreen, MonoTouch.Dialog.Utilities.IImageUpdated
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
                    imageview = FindViewById<ImageView>(Resource.Id.TwitterImageView);

                    var uri = new Uri(_tweet.ImageUrl);
                    try
                    {
                        var drawable = MonoTouch.Dialog.Utilities.ImageLoader.DefaultRequestImage(uri, this);
                        if (drawable != null)
                            imageview.SetImageDrawable(drawable);
                    }
                    catch (Exception ex)
                    {
                        Log.Debug("TWITTER", ex.ToString());
                    }
                }
                else
                {   // shouldn't happen...
                    FindViewById<TextView>(Resource.Id.TitleTextView).Text = "Tweet not found: " + id;
                }
            }
        }
        ImageView imageview;
        public void UpdatedImage(Uri uri)
        {
            var drawable = MonoTouch.Dialog.Utilities.ImageLoader.DefaultRequestImage(uri, this);
            imageview.SetImageDrawable(drawable);
        }
    }
}