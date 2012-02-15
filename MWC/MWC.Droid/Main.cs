using System;
using System.Globalization;
using System.IO;
using System.Threading;
using Android.App;

namespace MWC {
    // Note: don't duplicate these Application attributes in AndroidManifest.xml - build error "Duplicate attributes"
    [Application(Label="MWC 2012", Icon = "@drawable/icon", Theme = "@style/CustomTheme")]
    public class MWCApp : Application {
        public static MWCApp Current { get; private set; }

        const string prefsSeedDataKey = "SeedDataLoaded";
        public const string PrefsEarliestUpdate = "EarliestUpdate";

        public MWCApp(IntPtr handle, global::Android.Runtime.JniHandleOwnership transfer)
            : base(handle, transfer)
        {
            Current = this;
        }

        public override void OnCreate()
        {
            Console.WriteLine("MAIN OnCreate ---------------------------------");
            base.OnCreate();

            BL.Managers.UpdateManager.UpdateFinished += HandleFinishedUpdate;

            new Thread(new ThreadStart(() => {
                Console.WriteLine("MAIN ThreadStart");

                var prefs = GetSharedPreferences("MWC2012", global::Android.Content.FileCreationMode.Private);
                bool hasSeedData = prefs.GetBoolean(prefsSeedDataKey, false);

                Console.WriteLine("MAIN ThreadStart, hasSeedData=" + hasSeedData);

                if (!hasSeedData) {   // only happens once
                    Console.WriteLine("MAIN Load SeedData.xml");
                    Stream seedDataStream = Assets.Open(@"SeedData.xml");
                    StreamReader reader = new StreamReader(seedDataStream); 
                    string xml = reader.ReadToEnd();
                    BL.Managers.UpdateManager.UpdateFromFile(xml);
                } else {
                    Console.WriteLine("MAIN Load new data from server !!!");

                    var earliestUpdateString = prefs.GetString(PrefsEarliestUpdate, "");
                    DateTime earliestUpdateTime = DateTime.MinValue;

                    if (!String.IsNullOrEmpty(earliestUpdateString)) {
                        CultureInfo provider = CultureInfo.InvariantCulture;

                        if (DateTime.TryParse(earliestUpdateString
                                                , provider
                                                , System.Globalization.DateTimeStyles.None
                                                , out earliestUpdateTime)) { 
                            Console.WriteLine("MAIN Earliest update time: " + earliestUpdateTime); }
                        
                    }
                    if (earliestUpdateTime < DateTime.Now) {
                        if (true) { // TODO: reachability?
                            Console.WriteLine("MAIN UpdateConference");
                            BL.Managers.UpdateManager.UpdateConference();
                        } else {
                            //Console.WriteLine("No network, can't update data for now");
                        }
                    } else Console.WriteLine("MAIN Too soon to update " + DateTime.Now);
                }
            })).Start();
        }

        public override void OnTerminate()
        {
            Console.WriteLine("MAIN OnTerminate");
            BL.Managers.UpdateManager.UpdateFinished -= HandleFinishedUpdate;
            base.OnTerminate();
        }

        /// <summary>
        /// When updates finished, save the time so we don't check again
        /// too soon.
        /// </summary>
        void HandleFinishedUpdate(object sender, EventArgs ea)
        {
            Console.WriteLine("MAIN HandleFinishedUpdate");
            
            var prefs = GetSharedPreferences("MWC2012", global::Android.Content.FileCreationMode.Private);
            var args = ea as UpdateFinishedEventArgs;
            if (args != null) {
                // if we fail, we'll try again in an hour
                var earliestUpdate = DateTime.Now.AddHours(1);

                if (args.Success) {
                    Console.WriteLine("MAIN HandleFinishedUpdate success");
                    prefs.Edit().PutBoolean(prefsSeedDataKey, true);

                    if (args.UpdateType == UpdateType.SeedData) {	// SeedData is already out-of-date
                        earliestUpdate = DateTime.Now;
                    } else {	// having succeeded, we won't try again for another day
                        earliestUpdate = DateTime.Now.AddDays(1);
                    }
                    if (args.UpdateType == UpdateType.Conference) {	// now get the exhibitors, but don't really care if it fails
                        Console.WriteLine("MAIN HandleFinishedUpdate UpdateExhibitors");
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
            Console.WriteLine("MAIN HandleFinishedUpdate complete (prefs committed)");
        }
    }
}