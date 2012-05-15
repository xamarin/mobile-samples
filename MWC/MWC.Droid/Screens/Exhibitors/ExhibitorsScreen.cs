using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Util;
using Android.Widget;
using MWC.BL;

namespace MWC.Android.Screens
{
    [Activity(Label = "Exhibitors", ScreenOrientation = ScreenOrientation.Portrait)]
    public class ExhibitorsScreen : BaseScreen
    {
        protected MWC.Adapters.ExhibitorListAdapter exhibitorListAdapter;
        protected IList<Exhibitor> exhibitors;
        protected ListView exhibitorListView = null;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            Log.Debug("MWC", "EXHIBITORS OnCreate");

            // set our layout to be the home screen
            this.SetContentView(Resource.Layout.ExhibitorsScreen);

            //Find our controls
            this.exhibitorListView = FindViewById<ListView>(Resource.Id.ExhibitorList);

            // wire up task click handler
            if (this.exhibitorListView != null)
            {
                this.exhibitorListView.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) =>
                {
                    var exhibitorDetails = new Intent(this, typeof(ExhibitorDetailsScreen));
                    exhibitorDetails.PutExtra("ExhibitorID", this.exhibitors[e.Position].ID);
                    this.StartActivity(exhibitorDetails);
                };
            }
        }

        void PopulateTable()
        {
            try
            {
                Log.Debug("MWC", "EXHIBITORS PopulateTable");

                if (exhibitors == null || exhibitors.Count == 0)
                {
                    Log.Debug("MWC", "EXHIBITORS PopulateTable GetExhibitors");
                    this.exhibitors = MWC.BL.Managers.ExhibitorManager.GetExhibitors();

                    if (this.exhibitors.Count > 0)
                    {
                        // create our adapter
                        this.exhibitorListAdapter = new MWC.Adapters.ExhibitorListAdapter(this, this.exhibitors);

                        //Hook up our adapter to our ListView
                        this.exhibitorListView.Adapter = this.exhibitorListAdapter;
                    }
                }
            }
            catch (Exception e)
            {
                Log.Wtf("MWC", e.ToString());
            }
        }

        #region UpdateManagerLoadingScreen copied here, for Exhibitor-specific behaviour

        ProgressDialog progress;

        protected override void OnStart()
        {
            base.OnStart();
            MWCApp.LogDebug("EXHIBITORS OnStart");
            
            BL.Managers.UpdateManager.UpdateExhibitorsStarted += HandleUpdateStarted;
            BL.Managers.UpdateManager.UpdateExhibitorsFinished += HandleUpdateFinished;
        }
        protected override void OnResume()
        {
            MWCApp.LogDebug("EXHIBITORS OnResume");
            base.OnResume();
            if (BL.Managers.UpdateManager.IsUpdatingExhibitors) {
                MWCApp.LogDebug("EXHIBITORS OnResume IsUpdating");
                if (progress == null) {
                    progress = ProgressDialog.Show(this, "Loading", "Please Wait...", true);
                }
            } else {
                MWCApp.LogDebug("EXHIBITORS OnResume PopulateTable");
                if (progress != null)
                    progress.Hide(); 
                
                PopulateTable();
            }
        }
        protected override void OnStop()
        {
            MWCApp.LogDebug("EXHIBITORS OnStop");
            if (progress != null)
                progress.Hide();
            BL.Managers.UpdateManager.UpdateExhibitorsStarted -= HandleUpdateStarted;
            BL.Managers.UpdateManager.UpdateExhibitorsFinished -= HandleUpdateFinished;
            base.OnStop();
        }
        void HandleUpdateStarted(object sender, EventArgs e)
        {
            MWCApp.LogDebug("EXHIBITORS HandleUpdateStarted");
        }
        void HandleUpdateFinished(object sender, EventArgs e)
        {
            MWCApp.LogDebug("EXHIBITORS HandleUpdateFinished");
            RunOnUiThread(() => {
                if (progress != null)
                    progress.Hide();
                PopulateTable();
            });
        }
        #endregion
    }
}