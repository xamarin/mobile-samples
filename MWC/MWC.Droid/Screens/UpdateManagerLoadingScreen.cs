using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using MWC.BL;
using MWC;
using System;
using Android.Util;
using Android.Views;

namespace MWC.Android.Screens
{
    public class UpdateManagerLoadingScreen : BaseScreen
    {
        ProgressDialog progress;

        protected override void OnStart()
        {
            base.OnStart();
            MWCApp.LogDebug("UPDATELOADING OnStart");

            BL.Managers.UpdateManager.UpdateStarted += HandleUpdateStarted;
            BL.Managers.UpdateManager.UpdateFinished += HandleUpdateFinished;
        }
        protected override void OnResume()
        {
            Console.WriteLine("UPDATELOADING OnResume");
            base.OnResume();
            if (BL.Managers.UpdateManager.IsUpdating) {
                MWCApp.LogDebug("UPDATELOADING OnResume IsUpdating");
                if (progress == null) {
                    progress = ProgressDialog.Show(this, "Loading", "Please Wait...", true); 
                }
            } else {
                MWCApp.LogDebug("UPDATELOADING OnResume PopulateTable");
                if (progress != null)
                    progress.Hide();

                PopulateTable();
            }
        }
        protected override void OnStop()
        {
            MWCApp.LogDebug("UPDATELOADING OnStop");
            if (progress != null)
                progress.Hide();

            BL.Managers.UpdateManager.UpdateStarted -= HandleUpdateStarted;
            BL.Managers.UpdateManager.UpdateFinished -= HandleUpdateFinished;
            base.OnStop();
        }
        void HandleUpdateStarted(object sender, EventArgs e)
        {
            MWCApp.LogDebug("UPDATELOADING HandleUpdateStarted");
        }
        void HandleUpdateFinished(object sender, EventArgs e)
        {
            MWCApp.LogDebug("UPDATELOADING HandleUpdateFinished");
            RunOnUiThread(() => {
                if (progress != null)
                    progress.Hide();
                PopulateTable();
            });
        }
        protected virtual void PopulateTable()
        { 
        }
    }
}
