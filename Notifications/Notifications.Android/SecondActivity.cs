namespace Notifications
{
	using System;
	using Android.App;
	using Android.OS;
	using Android.Widget;

	[Activity(Label = "Second Activity")]
	public class SecondActivity : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			int count = Intent.Extras.GetInt ("count", -1);
			if (count <= 0) {
				return;
			}
			SetContentView (Resource.Layout.Second);
			TextView txtView = FindViewById<TextView> (Resource.Id.textView1);
			txtView.Text = String.Format ("You clicked the button {0} times in the previous activity.", count);
		}
	}
}
