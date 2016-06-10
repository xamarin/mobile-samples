using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using DataAccess;

namespace com.xamarin.sample.dataaccess
{
    [Activity(Label = "DataAccess", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
		EditText outputText;
		Button adoButton, ormButton;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.MainActivity);

			adoButton = FindViewById<Button>(Resource.Id.adoButton);
			ormButton = FindViewById<Button>(Resource.Id.ormButton);
			outputText = FindViewById<EditText>(Resource.Id.outputText);

			adoButton.Click += (sender, e) => {
				outputText.Text = AdoExample.DoSomeDataAccess ();
			};

			ormButton.Click += (sender, e) => {
				outputText.Text = OrmExample.DoSomeDataAccess ();
			};
		}
	}
}