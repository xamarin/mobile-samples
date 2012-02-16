using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Util;
using Android.Widget;
using MWC.BL;

namespace MWC.Android.Screens {
    [Activity(Label = "Sessions", ScreenOrientation = ScreenOrientation.Portrait)]
    public class SessionsScreen : UpdateManagerLoadingScreen {
        MWC.Adapters.SessionTimeslotListAdapter sessionTimeslotListAdapter;
        IList<SessionTimeslot> sessionTimeslots;
        ListView sessionListView = null;
        TextView titleTextView;
        int dayID = -1;

        protected override void OnCreate(Bundle bundle)
        {
            Log.Debug("MWC", "SESSIONS OnCreate");
            base.OnCreate(bundle);

            // set our layout to be the home screen
            SetContentView(Resource.Layout.SessionsScreen);

            dayID = Intent.GetIntExtra("DayID", -1);

            //Find our controls
            sessionListView = FindViewById<ListView>(Resource.Id.SessionList);
            sessionListView = FindViewById<ListView>(Resource.Id.SessionList);
            titleTextView = FindViewById<TextView>(Resource.Id.TitleTextView);
            
            // wire up task click handler
            if (sessionListView != null) {
                sessionListView.ItemClick += (object sender, ItemEventArgs e) => {
                    var sessionDetails = new Intent(this, typeof(SessionDetailsScreen));
                    var session = sessionTimeslotListAdapter[e.Position];
                    sessionDetails.PutExtra("SessionID", session.ID);
                    StartActivity(sessionDetails);
                };
            }
        }


        protected override void PopulateTable()
        {
            Console.WriteLine("SESSIONS PopulateTable");
            
            if (sessionTimeslots == null || sessionTimeslots.Count == 0) {
                // no data already here, so load it up
                if (dayID >= 0) {
                    var days = DaysManager.GetDays();
                    titleTextView.Text = days[dayID-1].ToString("dddd").ToUpper();
                    sessionTimeslots = MWC.BL.Managers.SessionManager.GetSessionTimeslots(dayID);
                } else {
                    titleTextView.Text = "ALL SESSIONS";
                    //titleTextView.Visibility = global::Android.Views.ViewStates.Gone;
                    sessionTimeslots = MWC.BL.Managers.SessionManager.GetSessionTimeslots();
                }
            }
            
            // Adapter is created every time, so Favorite changes are reflected each time the screen is visited
            sessionTimeslotListAdapter = new MWC.Adapters.SessionTimeslotListAdapter(this, sessionTimeslots);
            //Hook up our adapter to our ListView
            sessionListView.Adapter = sessionTimeslotListAdapter;
        }
    }
}