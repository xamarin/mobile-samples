using System;
using System.Globalization;
using System.IO;
using System.Threading;
using Android.App;
using Android.Preferences;
using Android.Util;

namespace MWC {
    [Application]
    public class MWCApp : Application {
        public static MWCApp Current { get; private set; }

        public const string PrefsEarliestUpdateKey = "EarliestUpdate";

        public MWCApp(IntPtr handle, global::Android.Runtime.JniHandleOwnership transfer)
            : base(handle, transfer)
        {
            Current = this;
        }

        public override void OnCreate()
        {
            LogDebug("MAIN OnCreate ---------------------------------");
            base.OnCreate();

            BL.Managers.UpdateManager.UpdateFinished += HandleFinishedUpdate;

            new Thread(new ThreadStart(() => {
                LogDebug("MAIN ThreadStart");

                var prefs = PreferenceManager.GetDefaultSharedPreferences(this);
                
                bool hasSeedData = BL.Managers.UpdateManager.HasDataAlready;
                LogDebug("MAIN ThreadStart, hasSeedData=" + hasSeedData);

                if (!hasSeedData) {   // only happens once
                    LogDebug("MAIN Load SeedData.xml");
                    Stream seedDataStream = Assets.Open(@"SeedData.xml");
                    //StreamReader reader = new StreamReader(seedDataStream); 
                    //string xml = reader.ReadToEnd();                            // fails on emulator
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    using (StreamReader reader = new StreamReader(seedDataStream)) {
                        //This is an arbitrary size for this example.
                        char[] c = null;

                        while (reader.Peek() >= 0) {
                            c = new char[4096];
                            reader.Read(c, 0, c.Length);
                            sb.Append(c);
                        }
                    }
                    string xml = sb.ToString();

                    BL.Managers.UpdateManager.UpdateFromFile(xml);
                } else {
                    LogDebug("MAIN Load new data from server !!!");

                    var earliestUpdateString = prefs.GetString(PrefsEarliestUpdateKey, "");
                    DateTime earliestUpdateTime = DateTime.MinValue;

                    if (!String.IsNullOrEmpty(earliestUpdateString)) {
                        CultureInfo provider = CultureInfo.InvariantCulture;

                        if (DateTime.TryParse(earliestUpdateString
                                                , provider
                                                , System.Globalization.DateTimeStyles.None
                                                , out earliestUpdateTime)) { 
                            LogDebug("MAIN Earliest update time: " + earliestUpdateTime); }
                        
                    }
                    if (earliestUpdateTime < DateTime.Now) {
                        if (true) { // TODO: reachability?
                            LogDebug("MAIN UpdateConference");
                            BL.Managers.UpdateManager.UpdateConference();
                        } else {
                            //LogDebug("No network, can't update data for now");
                        }
                    } else LogDebug("MAIN Too soon to update " + DateTime.Now);
                }
            })).Start();
        }

        public override void OnTerminate()
        {
            LogDebug("MAIN OnTerminate");
            BL.Managers.UpdateManager.UpdateFinished -= HandleFinishedUpdate;
            base.OnTerminate();
        }

        /// <summary>
        /// When updates finished, save the time so we don't check again too soon.
        /// </summary>
        void HandleFinishedUpdate(object sender, EventArgs ea)
        {
            LogDebug("MAIN HandleFinishedUpdate");

            var prefs = PreferenceManager.GetDefaultSharedPreferences(this);// GetSharedPreferences("MWC2012", global::Android.Content.FileCreationMode.Private);
            var edit = prefs.Edit();
            var args = ea as UpdateFinishedEventArgs;
            if (args != null) {
                // if we fail, we'll try again in an hour
                var earliestUpdate = DateTime.Now.AddHours(1);

                if (args.Success) {
                    LogDebug("MAIN HandleFinishedUpdate success");

                    if (args.UpdateType == UpdateType.SeedData) {	// SeedData is already out-of-date
                        earliestUpdate = DateTime.Now;
                    } else {	// having succeeded, we won't try again for another day
                        earliestUpdate = DateTime.Now.AddDays(1);
                    }
                    if (args.UpdateType == UpdateType.Conference) {	// now get the exhibitors, but don't really care if it fails
                        LogDebug("MAIN HandleFinishedUpdate UpdateExhibitors");
                        BL.Managers.UpdateManager.UpdateExhibitors();
                    }
                }

#if DEBUG
                earliestUpdate = DateTime.Now; // for testing, ALWAYS update :)
#endif
                CultureInfo provider = CultureInfo.InvariantCulture;
                var earliestUpdateString = earliestUpdate.ToString(provider);
                edit.PutString(PrefsEarliestUpdateKey, earliestUpdateString);
                edit.Commit();
            }
            
            LogDebug("MAIN HandleFinishedUpdate complete");
        }

/*
Use this to help with ADB watching in CMD 
"c:\Program Files (x86)\Android\android-sdk\platform-tools\adb" logcat -s MonoDroid:* mono:* MWC:* ActivityManager:*
*/
        public static void LogDebug(string message) {
            Console.WriteLine(message);
            Log.Debug("MWC", message);
        }
    }
}