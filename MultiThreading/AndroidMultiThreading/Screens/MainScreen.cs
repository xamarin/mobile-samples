
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace AndroidMultiThreading.Screens
{
	[Activity (Label = "MultiThreading", MainLauncher = true)]			
	public class MainScreen : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			// Get our button from the layout resource,
			// and attach an event to it
			Button button = FindViewById<Button> (Resource.Id.StartBackgroundTaskUpdateUI);
			
			button.Click += delegate {
				//button.Text = string.Format ("{0} clicks!", count++);
			};
		}
	}
}

