namespace Notifications
{
	using System;
	using Android.App;
	using Android.Content;
	using Android.OS;
	using Android.Support.V4.App;
	using Android.Widget;
	using Java.Lang;
	using String = System.String;

	[Activity(Label = "Notifications", MainLauncher = true, Icon = "@drawable/ic_launcher")]
	public class MainActivity : Activity
	{
		private static readonly int ButtonClickNotificationId = 1000;
		private int _count = 1;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.Main);

			Button button = FindViewById<Button> (Resource.Id.MyButton);

			button.Click += ButtonOnClick;
		}

		private void ButtonOnClick (object sender, EventArgs eventArgs)
		{
			// These are the values that we want to pass to the next activity
			Bundle valuesForActivity = new Bundle ();
			valuesForActivity.PutInt ("count", _count);

			// Create the PendingIntent with the back stack             
			// When the user clicks the notification, SecondActivity will start up.
			Intent resultIntent = new Intent (this, typeof(SecondActivity));
			resultIntent.PutExtras (valuesForActivity); // Pass some values to SecondActivity.

			TaskStackBuilder stackBuilder = TaskStackBuilder.Create (this);
			stackBuilder.AddParentStack (Class.FromType (typeof(SecondActivity)));
			stackBuilder.AddNextIntent (resultIntent);

			PendingIntent resultPendingIntent = stackBuilder.GetPendingIntent (0, (int)PendingIntentFlags.UpdateCurrent);

			// Build the notification
			NotificationCompat.Builder builder = new NotificationCompat.Builder (this)
                .SetAutoCancel (true) // dismiss the notification from the notification area when the user clicks on it
                .SetContentIntent (resultPendingIntent) // start up this activity when the user clicks the intent.
                .SetContentTitle ("Button Clicked") // Set the title
                .SetNumber (_count) // Display the count in the Content Info
                .SetSmallIcon (Resource.Drawable.ic_stat_button_click) // This is the icon to display
                .SetContentText (String.Format ("The button has been clicked {0} times.", _count)); // the message to display.

			// Finally publish the notification
			NotificationManager notificationManager = (NotificationManager)GetSystemService (NotificationService);
			notificationManager.Notify (ButtonClickNotificationId, builder.Build ());

			_count++;
		}
	}
}
