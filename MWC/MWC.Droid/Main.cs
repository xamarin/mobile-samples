using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Android.App;
using System.Threading;

namespace MWC
{
    [Application(Label = "Mobile World Congress", Icon = "@drawable/icon", Theme = "@style/CustomTheme")]
    public class MWCApp : Application
    {
        public static MWCApp Current { get; private set; }

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