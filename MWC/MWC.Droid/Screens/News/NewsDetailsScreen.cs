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
    [Activity(Label = "News")]
    class NewsDetailsScreen : BaseScreen
    {
        RSSEntry _newsItem;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.NewsDetailsScreen);

            var id = Intent.GetIntExtra("NewsID", -1);

            if (id >= 0)
            {
                _newsItem = BL.Managers.NewsManager.Get(id);
                if (_newsItem != null)
                {
                    FindViewById<TextView>(Resource.Id.TitleTextView).Text = _newsItem.Title;
                    FindViewById<TextView>(Resource.Id.PublishedTextView).Text = _newsItem.Published.ToString("d MMM yy");
                    FindViewById<WebView>(Resource.Id.ContentWebView).LoadData(
                                "<html><body>"+_newsItem.Content+"</body></html>", @"text/html", null);

            //        FindViewById<WebView>(Resource.Id.ContentWebView).LoadData(
            //"<html><body><b>adssdf</b><br />sdk sdfkjasfdj lskdfj ljkfds alsjkfd alkdfj lakdsfj laskdjfalskfdj fdj lsdkf jlksdfj laskd fjlaskfd jasldfkjalsdkfj aslkdf jaskdfj laskfdj alsjkdf lsdk fkdjf kdj ljf kj lkjdsa flkdj lfkj lkdsj flkjdslfa </body></html>", @"text/html", null);
                }
                else
                {   // shouldn't happen...
                    FindViewById<TextView>(Resource.Id.TitleTextView).Text = "NewsItem not found: " + id;
                }
            }
        }
    }
}