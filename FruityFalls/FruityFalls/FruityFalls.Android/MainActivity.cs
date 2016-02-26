using System;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using CocosSharp;
using FruityFalls.Scenes;

namespace FruityFalls.Android
{
	[Activity (
		Label = "FruityFalls.Android", 
		MainLauncher = true, Icon = "@drawable/icon", 
		AlwaysRetainTaskState = true,
		Theme = "@android:style/Theme.NoTitleBar",
		LaunchMode = LaunchMode.SingleInstance,
		ScreenOrientation = ScreenOrientation.Portrait,
		ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden)]
	public class MainActivity : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			// Get our game view from the layout resource,
			// and attach the view created event to it
			CCGameView gameView = (CCGameView)FindViewById (Resource.Id.GameView);
			gameView.ViewCreated += LoadGame;
		}

		void LoadGame (object sender, EventArgs e)
		{
			CCGameView gameView = sender as CCGameView;

			if (gameView != null)
			{
				GameController.Initialize (gameView);
			}
		}
	}
}


