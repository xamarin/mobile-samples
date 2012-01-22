using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Android.App;
using System.Threading;
using System.Globalization;
using Android.Util;
/*
Use this to help with ADB watching in CMD 
"c:\Program Files (x86)\Android\android-sdk\platform-tools\" adb logcat -s MonoDroid:* mono:* MWC:* ActivityManager:*
*/
namespace MWC
{
    [Application(Label = "Mobile World Congress", Icon = "@drawable/icon", Theme = "@style/CustomTheme")]
    public class MWCApp : Application
    {
        public static MWCApp Current { get; private set; }

        const string prefsSeedDataKey = "SeedDataLoaded";
        public const string PrefsEarliestUpdate = "EarliestUpdate";

        public MWCApp(IntPtr handle)
            : base(handle)
        {
            Current = this;
        }

        public override void OnCreate()
        {
            Log.Debug("MWC", "MAIN OnCreate ---------------------------------");
            base.OnCreate();

            BL.Managers.UpdateManager.UpdateFinished += HandleFinishedUpdate;

            new Thread(new ThreadStart(() => 
            {
                Log.Debug("MWC", "MAIN ThreadStart");

                var prefs = GetSharedPreferences("MWC2012", global::Android.Content.FileCreationMode.Private);
                bool hasSeedData = prefs.GetBoolean(prefsSeedDataKey, false);

                Log.Debug("MWC", "MAIN ThreadStart, hasSeedData=" + hasSeedData);

                if (!hasSeedData)
                {   // only happens once
                    Log.Debug("MWC", "MAIN Load SeedData.xml");
                    Stream seedDataStream = Assets.Open(@"SeedData.xml");
                    StreamReader reader = new StreamReader(seedDataStream); 
                    string xml = reader.ReadToEnd();
                    BL.Managers.UpdateManager.UpdateFromFile(xml);
                }
                else
                {
                    Log.Debug("MWC", "MAIN Load new data from server !!!");

                    var earliestUpdateString = prefs.GetString(PrefsEarliestUpdate, "");
                    DateTime earliestUpdateTime = DateTime.MinValue;

                    if (!String.IsNullOrEmpty(earliestUpdateString))
                    {
                        CultureInfo provider = CultureInfo.InvariantCulture;

                        if (DateTime.TryParse(earliestUpdateString
                                                , provider
                                                , System.Globalization.DateTimeStyles.None
                                                , out earliestUpdateTime))
                        { Log.Debug("MWC", "MAIN Earliest update time: " + earliestUpdateTime); }
                        
                    }
                    if (earliestUpdateTime < DateTime.Now)
                    {
                        if (true) // TODO: reachability?
                        {
                            Log.Debug("MWC", "MAIN UpdateConference");
                            BL.Managers.UpdateManager.UpdateConference();
                        }
                        else
                        {
                            //Console.WriteLine("No network, can't update data for now");
                        }
                    }
                    else Log.Debug("MWC", "MAIN Too soon to update " + DateTime.Now);
                }
            
            })).Start();
        }

        public override void OnTerminate()
        {
            Log.Debug("MWC", "MAIN OnTerminate");
            BL.Managers.UpdateManager.UpdateFinished -= HandleFinishedUpdate;
            base.OnTerminate();
        }

        /// <summary>
        /// When updates finished, save the time so we don't check again
        /// too soon.
        /// </summary>
        void HandleFinishedUpdate(object sender, EventArgs ea)
        {
            Log.Debug("MWC", "MAIN HandleFinishedUpdate");
            
            var prefs = GetSharedPreferences("MWC2012", global::Android.Content.FileCreationMode.Private);
            var args = ea as UpdateFinishedEventArgs;
            if (args != null)
            {
                // if we fail, we'll try again in an hour
                var earliestUpdate = DateTime.Now.AddHours(1);

                if (args.Success)
                {
                    Log.Debug("MWC", "MAIN HandleFinishedUpdate success");
                    prefs.Edit().PutBoolean(prefsSeedDataKey, true);

                    if (args.UpdateType == UpdateType.SeedData)
                    {	// SeedData is already out-of-date
                        earliestUpdate = DateTime.Now;
                    }
                    else
                    {	// having succeeded, we won't try again for another day
                        earliestUpdate = DateTime.Now.AddDays(1);
                    }
                    if (args.UpdateType == UpdateType.Conference)
                    {	// now get the exhibitors, but don't really care if it fails
                        Log.Debug("MWC", "MAIN HandleFinishedUpdate UpdateExhibitors");
                        BL.Managers.UpdateManager.UpdateExhibitors();
                    }
                }

#if DEBUG
                earliestUpdate = DateTime.Now; // for testing, ALWAYS update :)
#endif
                CultureInfo provider = CultureInfo.InvariantCulture;
                var earliestUpdateString = earliestUpdate.ToString(provider);
                prefs.Edit().PutString(PrefsEarliestUpdate, earliestUpdateString);
            }
            prefs.Edit().Commit();
            Log.Debug("MWC", "MAIN HandleFinishedUpdate complete (prefs committed)");
        }
    }
}