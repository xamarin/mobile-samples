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
                _newsItem = BL.Managers.NewsManager.GetNews(id);
                if (_newsItem != null)
                {
                    FindViewById<TextView>(Resource.Id.TitleTextView).Text = _newsItem.Title;
                    FindViewById<TextView>(Resource.Id.PublishedTextView).Text = _newsItem.Published.ToString("d MMM yy");
                    FindViewById<TextView>(Resource.Id.ContentTextView).Text = _newsItem.Content;
                }
                else
                {   // shouldn't happen...
                    FindViewById<TextView>(Resource.Id.Title).Text = "NewsItem not found: " + id;
                }
            }
        }
    }
}