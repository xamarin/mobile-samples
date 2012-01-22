using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using MWC.BL;
using MWC;
using MWC.SAL;
using Android.Util;
using System;

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
            Log.Debug("MWC", "EXHIBITORS OnCreate");

            // set our layout to be the home screen
            this.SetContentView(Resource.Layout.ExhibitorsScreen);

            //Find our controls
            this._exhibitorListView = FindViewById<ListView>(Resource.Id.ExhibitorList);

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

        void PopulateTable()
        {
            try
            {
                Log.Debug("MWC", "EXHIBITORS PopulateTable");

                if (_exhibitors == null || _exhibitors.Count == 0)
                {
                    Log.Debug("MWC", "EXHIBITORS PopulateTable GetExhibitors");
                    this._exhibitors = MWC.BL.Managers.ExhibitorManager.GetExhibitors();

                    if (this._exhibitors.Count > 0)
                    {
                        // create our adapter
                        this._exhibitorListAdapter = new MWC.Adapters.ExhibitorListAdapter(this, this._exhibitors);

                        //Hook up our adapter to our ListView
                        this._exhibitorListView.Adapter = this._exhibitorListAdapter;
                    }
                }
            }
            catch (Exception e)
            {
                Log.Wtf("MWC", e.ToString());
            }
        }

        #region UpdateManagerLoadingScreen copied here, for Exhibitor-specific behaviour
        protected override void OnStart()
        {
            base.OnStart();
            Log.Debug("MWC", "EXHIBITORS OnStart");
            
            BL.Managers.UpdateManager.UpdateExhibitorsStarted += HandleUpdateStarted;
            BL.Managers.UpdateManager.UpdateExhibitorsFinished += HandleUpdateFinished;
        }
        protected override void OnResume()
        {
            Log.Debug("MWC", "EXHIBITORS OnResume");
            base.OnResume();
            if (BL.Managers.UpdateManager.IsUpdatingExhibitors)
            {
                Log.Debug("MWC", "EXHIBITORS OnResume IsUpdating");
            }
            else
            {
                Log.Debug("MWC", "EXHIBITORS OnResume PopulateTable");
                PopulateTable();
            }
        }
        protected override void OnStop()
        {
            Log.Debug("MWC", "EXHIBITORS OnStop");
            BL.Managers.UpdateManager.UpdateExhibitorsStarted -= HandleUpdateStarted;
            BL.Managers.UpdateManager.UpdateExhibitorsFinished -= HandleUpdateFinished;
            base.OnStop();
        }
        void HandleUpdateStarted(object sender, EventArgs e)
        {
            Log.Debug("MWC", "EXHIBITORS HandleUpdateStarted");
        }
        void HandleUpdateFinished(object sender, EventArgs e)
        {
            Log.Debug("MWC", "EXHIBITORS HandleUpdateFinished");
            RunOnUiThread(() =>
            {
                PopulateTable();
            });
        }
        #endregion
    }
}