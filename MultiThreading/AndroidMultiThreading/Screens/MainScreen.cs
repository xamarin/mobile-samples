using System;

using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace AndroidMultiThreading.Screens
{
    [Activity(Label = "@string/app_name", MainLauncher = true)]
    public class MainScreen : AppCompatActivity
    {
        static readonly string TAG = "ATM:MainScreen";
        static readonly string IS_PROGRESS_BAR_SHOWING = "show_progress_bar";

        BackgroundTaskRunnerFragment frag;
        ProgressBar progressBar;
        bool showProgressBar;
        Button updateUIButton, noupdateUIButton;

        public void DisplayProgressbar()
        {
            progressBar.Visibility = ViewStates.Visible;
            Log.Debug(TAG, "Should be showing the progress bar.");
        }

        public void InitializeEventHandlers(bool connect = true)
        {
            if (connect)
            {
                frag.TaskARunStatusChangeHandler += FragOnTaskARunStatusChangeHandler;
                noupdateUIButton.Click += NoupdateUiButtonOnClick;
                updateUIButton.Click += UpdateUiButtonOnClick;
            }
            else
            {
                frag.TaskARunStatusChangeHandler -= FragOnTaskARunStatusChangeHandler;
                noupdateUIButton.Click -= NoupdateUiButtonOnClick;
                updateUIButton.Click -= UpdateUiButtonOnClick;
            }
        }

        void FragOnTaskARunStatusChangeHandler(object sender, TaskARunningEventArgs e)
        {
            if (e.IsRunning)
            {
                DisplayProgressbar();
            }
            else
            {
                HideProgessbar();
            }
        }

        public void HideProgessbar()
        {
            progressBar.Visibility = ViewStates.Gone;
            Log.Debug(TAG, $"Should NOT be showing the progress bar. IsTaskARunning = {frag.IsTaskARunning}.");
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

            updateUIButton = FindViewById<Button>(Resource.Id.StartBackgroundTaskUpdateUI);
            noupdateUIButton = FindViewById<Button>(Resource.Id.StartBackgroundTaskNoUpdate);
            progressBar = FindViewById<ProgressBar>(Resource.Id.progressBar);

            frag = FragmentManager.FindFragmentByTag<BackgroundTaskRunnerFragment>(BackgroundTaskRunnerFragment.FRAGMENT_TAG);
            if (frag == null)
            {
                frag = BackgroundTaskRunnerFragment.NewInstance();
                FragmentManager.BeginTransaction()
                               .Add(frag, BackgroundTaskRunnerFragment.FRAGMENT_TAG)
                               .Commit();
                Log.Debug(TAG, "Instantiated a new TaskHelperFragment.");
            }
            else
            {
                Log.Debug(TAG, "Using the pre-existing TaskHelperFragment.");
            }
        }

        async void UpdateUiButtonOnClick(object sender, EventArgs e)
        {
            await frag.StartTaskUpdateUI();
        }

        async void NoupdateUiButtonOnClick(object sender, EventArgs e)
        {
            await frag.StartTaskNoUpdateUI();
        }

        protected override void OnResume()
        {
            base.OnResume();
            InitializeEventHandlers();
            if (showProgressBar)
            {
                DisplayProgressbar();
            }
            else
            {
                HideProgessbar();
            }
        }

        protected override void OnPause()
        {
            InitializeEventHandlers(false);
            base.OnPause();
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            outState.PutBoolean(IS_PROGRESS_BAR_SHOWING, frag.IsTaskARunning);
            base.OnSaveInstanceState(outState);
        }

        protected override void OnRestoreInstanceState(Bundle savedInstanceState)
        {
            base.OnRestoreInstanceState(savedInstanceState);
            showProgressBar = savedInstanceState.GetBoolean(IS_PROGRESS_BAR_SHOWING, false);
        }
    }
}
