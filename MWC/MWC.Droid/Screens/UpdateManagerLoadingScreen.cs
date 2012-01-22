using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using MWC.BL;
using MWC;
using System;
using Android.Util;

namespace MWC.Android.Screens
{
    public class UpdateManagerLoadingScreen : BaseScreen
    {
        protected override void OnStart()
        {
            Log.Debug("MWC", "UPDATELOADING OnStart");
            base.OnStart();
            BL.Managers.UpdateManager.UpdateStarted += HandleUpdateStarted;
            BL.Managers.UpdateManager.UpdateFinished += HandleUpdateFinished;
        }
        protected override void OnResume()
        {
            Log.Debug("MWC", "UPDATELOADING OnResume");
            base.OnResume();
            if (BL.Managers.UpdateManager.IsUpdating)
            {
                Log.Debug("MWC", "UPDATELOADING OnResume IsUpdating");
            }
            else
            {
                Log.Debug("MWC", "UPDATELOADING OnResume PopulateTable");
                PopulateTable();
            }
        }
        protected override void OnStop()
        {
            Log.Debug("MWC", "UPDATELOADING OnStop");
            BL.Managers.UpdateManager.UpdateStarted -= HandleUpdateStarted;
            BL.Managers.UpdateManager.UpdateFinished -= HandleUpdateFinished;
            base.OnStop();
        }
        void HandleUpdateStarted(object sender, EventArgs e)
        {
            Log.Debug("MWC", "UPDATELOADING HandleUpdateStarted");
        }
        void HandleUpdateFinished(object sender, EventArgs e)
        {
            Log.Debug("MWC", "UPDATELOADING HandleUpdateFinished");
            RunOnUiThread(() => 
            {
                PopulateTable();
            });
        }
        protected virtual void PopulateTable()
        { 
        }
    }
}
