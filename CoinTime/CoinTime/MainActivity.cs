using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Microsoft.Xna.Framework;

using CocosSharp;
using CoinTimeGame.Input;

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
			application.AndroidContentView.RequestFocus ();
			application.AndroidContentView.KeyPress += HandleKeyPress;
		}

		void HandleKeyPress(object sender, View.KeyEventArgs args)
		{
			Console.WriteLine (args.Event + " " + args.KeyCode + " ");

			// Check if the down times equals the event time. If not, then it's a long press:
			if (args.Event.Action == KeyEventActions.Down && args.Event.EventTime == args.Event.DownTime)
			{
				AmazonFireGameController.HandlePush (args.KeyCode);
			}
			else if (args.Event.Action == KeyEventActions.Up)
			{
				// CoinTime doesn't currently use it currently, but maybe in the future?
				//AmazonFireGameController.HandleRelease (args.KeyCode);
			}
		}

		public override bool OnGenericMotionEvent (MotionEvent e)
		{
			if (e.Source != InputSourceType.Touchscreen)
			{
				AmazonFireGameController.SetDPad (e.GetAxisValue (Axis.HatX));
				AmazonFireGameController.SetLeftAnalogStick (e.GetAxisValue (Axis.X));

				return true;
			}
			else
			{
				return false;
			}
		}
	}


}


