using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Android.App;
using System.Threading;

namespace MWC
{
    [Application(Label = "Mobile World Congress", Icon = "@drawable/icon")]
    public class MWCApp : Application
    {
        public static MWCApp Current { get; private set; }

        public const string NewsUrl = "http://news.google.com/news?q=mobile%20world%20congress&output=rss";
        public const string TwitterUrl = "http://search.twitter.com/search.atom?q=%40mobileworldlive@show-user=true&rpp=20";


        public MWCApp(IntPtr handle)
            : base(handle)
        {
            Current = this;
        }

        public override void OnCreate()
        {
            base.OnCreate();


            new Thread(new ThreadStart(() => { BL.Managers.UpdateManager.UpdateAll(); })).Start();

        }
    }
}