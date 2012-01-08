using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using MWC.BL;

namespace MWC.Android.Screens
{
    [Activity(Label = "Sessions")]
    public class SessionsScreen : BaseScreen
    {
        MWC.Adapters.SessionTimeslotListAdapter _sessionTimeslotListAdapter;
        IList<SessionTimeslot> _sessionTimeslots;
        ListView _sessionListView = null;
        TextView _titleTextView;
        int _dayID = -1;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // set our layout to be the home screen
            this.SetContentView(Resource.Layout.SessionsScreen);

            _dayID = Intent.GetIntExtra("DayID", -1);

            //Find our controls
            this._sessionListView = FindViewById<ListView>(Resource.Id.SessionList);
            this._sessionListView = FindViewById<ListView>(Resource.Id.SessionList);
            this._titleTextView = FindViewById<TextView>(Resource.Id.TitleTextView);
            
            // wire up task click handler
            if (this._sessionListView != null)
            {
                this._sessionListView.ItemClick += (object sender, ItemEventArgs e) =>
                {
                    var sessionDetails = new Intent(this, typeof(SessionDetailsScreen));
                    var session = this._sessionTimeslotListAdapter[e.Position];
                    sessionDetails.PutExtra("SessionID", session.ID);
                    this.StartActivity(sessionDetails);
                };
            }
        }

        protected override void OnResume()
        {
            base.OnResume();

            if (_dayID >= 0)
            {
                this._titleTextView.Text = "Day " + _dayID.ToString();
                this._sessionTimeslots = MWC.BL.Managers.SessionManager.GetSessionTimeslots(_dayID);
            }
            else
            {
                this._titleTextView.Text = "All sessions";
                this._sessionTimeslots = MWC.BL.Managers.SessionManager.GetSessionTimeslots();
            }

            // create our adapter
            this._sessionTimeslotListAdapter = new MWC.Adapters.SessionTimeslotListAdapter(this, this._sessionTimeslots);

            //Hook up our adapter to our ListView
            this._sessionListView.Adapter = this._sessionTimeslotListAdapter;
        }
    }
}