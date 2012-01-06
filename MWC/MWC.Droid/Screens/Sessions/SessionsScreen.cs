using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using MWC.BL;
using MWC;

namespace MWC.Android.Screens
{
    [Activity(Label = "Sessions")]
    public class SessionsScreen : BaseScreen
    {
        protected MWC.Adapters.SessionListAdapter _sessionList;
        protected IList<Session> _sessions;
        protected ListView _sessionListView = null;

        
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // set our layout to be the home screen
            this.SetContentView(Resource.Layout.SessionsScreen);

            //TODO: GetIntExtra("DayID", -1)

            //Find our controls
            this._sessionListView = FindViewById<ListView>(Resource.Id.SessionList);

            // wire up task click handler
            if (this._sessionListView != null)
            {
                this._sessionListView.ItemClick += (object sender, ItemEventArgs e) =>
                {
                    var sessionDetails = new Intent(this, typeof(SessionDetailsScreen));
                    sessionDetails.PutExtra("SessionID", this._sessions[e.Position].ID);
                    this.StartActivity(sessionDetails);
                };
            }
        }

        protected override void OnResume()
        {
            base.OnResume();

            this._sessions = MWC.BL.Managers.SessionManager.GetSessions();

            // create our adapter
            this._sessionList = new MWC.Adapters.SessionListAdapter(this, this._sessions);

            //Hook up our adapter to our ListView
            this._sessionListView.Adapter = this._sessionList;
        }
    }
}