using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Util;
using Android.Widget;
using MWC.BL;

namespace MWC.Android.Screens {
    [Activity(Label = "Sessions")]
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
            if (this.sessionListView != null) {
                this.sessionListView.ItemClick += (object sender, ItemEventArgs e) => {
                    var sessionDetails = new Intent(this, typeof(SessionDetailsScreen));
                    var session = sessionTimeslotListAdapter[e.Position];
                    sessionDetails.PutExtra("SessionID", session.ID);
                    StartActivity(sessionDetails);
                };
            }
        }

        protected override void PopulateTable()
        {
             Log.Debug("MWC", "SESSIONS PopulateTable");
             if (sessionTimeslots == null || sessionTimeslots.Count == 0) {
                 if (dayID >= 0) {
                     titleTextView.Text = "Day " + dayID.ToString() + " Sessions";
                     sessionTimeslots = MWC.BL.Managers.SessionManager.GetSessionTimeslots(dayID);
                 } else {
                     titleTextView.Text = "All sessions";
                     //titleTextView.Visibility = global::Android.Views.ViewStates.Gone;
                     sessionTimeslots = MWC.BL.Managers.SessionManager.GetSessionTimeslots();
                 }

                 // create our adapter
                 sessionTimeslotListAdapter = new MWC.Adapters.SessionTimeslotListAdapter(this, sessionTimeslots);

                 //Hook up our adapter to our ListView
                 sessionListView.Adapter = sessionTimeslotListAdapter;
             }
        }
    }
}