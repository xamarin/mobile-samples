using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using MWC.BL;
using MWC;
using MWC.SAL;

namespace MWC.Android.Screens
{
    [Activity(Label = "Exhibitors")]
    public class ExhibitorsScreen : BaseScreen
    {
        protected MWC.Adapters.ExhibitorListAdapter _exhibitorListAdapter;
        protected IList<Exhibitor> _exhibitors;
        protected ListView _exhibitorListView = null;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // set our layout to be the home screen
            this.SetContentView(Resource.Layout.SpeakersScreen);

            //Find our controls
            this._exhibitorListView = FindViewById<ListView>(Resource.Id.SpeakerList);

            // wire up task click handler
            if (this._exhibitorListView != null)
            {
                this._exhibitorListView.ItemClick += (object sender, ItemEventArgs e) =>
                {
                    var exhibitorDetails = new Intent(this, typeof(ExhibitorDetailsScreen));
                    exhibitorDetails.PutExtra("ExhibitorID", this._exhibitors[e.Position].ID);
                    this.StartActivity(exhibitorDetails);
                };
            }
        }

        protected override void OnResume()
        {
            base.OnResume();

            this._exhibitors = MWC.BL.Managers.ExhibitorManager.GetExhibitors();

            // create our adapter
            this._exhibitorListAdapter = new MWC.Adapters.ExhibitorListAdapter(this, this._exhibitors);

            //Hook up our adapter to our ListView
            this._exhibitorListView.Adapter = this._exhibitorListAdapter;
        }
    }
}