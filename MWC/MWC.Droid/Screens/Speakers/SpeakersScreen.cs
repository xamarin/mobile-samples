using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using MWC;
using MWC.BL;
using Android.Util;

namespace MWC.Android.Screens {
    [Activity(Label = "Speakers")]
    public class SpeakersScreen : UpdateManagerLoadingScreen {
        protected MWC.Adapters.SpeakerListAdapter speakerList;
        protected IList<Speaker> speakers;
        protected ListView speakerListView = null;
        TextView titleTextView;

        protected override void OnCreate(Bundle bundle)
        {
            Log.Debug("MWC", "SPEAKERS OnCreate");
            base.OnCreate(bundle);

            // set our layout to be the home screen
            SetContentView(Resource.Layout.SpeakersScreen);

            //Find our controls
            speakerListView = FindViewById<ListView>(Resource.Id.SpeakerList);
            titleTextView = FindViewById<TextView>(Resource.Id.TitleTextView);

            // wire up task click handler
            if (speakerListView != null) {
                speakerListView.ItemClick += (object sender, ItemEventArgs e) => {
                    var speakerDetails = new Intent(this, typeof(SpeakerDetailsScreen));
                    speakerDetails.PutExtra("SpeakerID", speakers[e.Position].ID);
                    StartActivity(speakerDetails);
                };
            }
        }

        protected override void PopulateTable()
        {
            Log.Debug("MWC", "SPEAKERS PopulateTable");
            if (speakers == null || speakers.Count == 0) {
                titleTextView.Text = "Speakers";
                speakers = MWC.BL.Managers.SpeakerManager.GetSpeakers();

                // create our adapter
                speakerList = new MWC.Adapters.SpeakerListAdapter(this, speakers);

                //Hook up our adapter to our ListView
                speakerListView.Adapter = this.speakerList;
            }
        }
    }
}