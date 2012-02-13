using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using MWC.BL;
using MWC;

namespace MWC.Android.Screens {
    [Activity(Label = "Home")]
    public class HomeScreen : BaseScreen {
        protected MWC.Adapters.DaysListAdapter dayList;
        protected ListView dayListView = null;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // set our layout to be the home screen
            SetContentView(Resource.Layout.HomeScreen);

            //Find our controls
            dayListView = FindViewById<ListView>(Resource.Id.DayList);

            // wire up task click handler
            if (dayListView != null) {
                dayListView.ItemClick += (object sender, ItemEventArgs e) => {
                    var sessionDetails = new Intent(this, typeof(SessionsScreen));
                    sessionDetails.PutExtra("DayID", e.Position + 1);
                    StartActivity(sessionDetails);
                };
            }
        }

        protected override void OnResume()
        {
            base.OnResume();

            // create our adapter
            dayList = new MWC.Adapters.DaysListAdapter(this);

            //Hook up our adapter to our ListView
            dayListView.Adapter = this.dayList;
        }
    }
}
