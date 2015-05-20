using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Microsoft.Xna.Framework;

using CocosSharp;

namespace CoinTime
{
	[Activity (
		Label = "CoinTime",
		AlwaysRetainTaskState = true,
		Icon = "@drawable/icon",
		Theme = "@android:style/Theme.NoTitleBar",
		ScreenOrientation = ScreenOrientation.Landscape | ScreenOrientation.ReverseLandscape,
		LaunchMode = LaunchMode.SingleInstance,
		MainLauncher = true,
		ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden)
    ]
	public class MainActivity : AndroidGameActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			CoinTimeGame.ContentLoading.XmlDeserializer.Self.Activity = this;

			var application = new CCApplication ();
			application.ApplicationDelegate = new GameAppDelegate ();
			SetContentView (application.AndroidContentView);
			application.StartGame ();
		}
	}
}


