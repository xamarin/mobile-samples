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

namespace RenderTextureExample.Android
{
	[Activity (Label = "RenderTextureExample.Android", MainLauncher = true, Icon = "@drawable/icon", 
		AlwaysRetainTaskState = true,
		LaunchMode = LaunchMode.SingleInstance,
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
                var contentSearchPaths = new List<string>() { "Fonts", "Sounds", "Images" };

                int width = 512;
                int height = 384;

                // Set world dimensions
                gameView.DesignResolution = new CCSizeI(width, height);

                gameView.ContentManager.SearchPaths = contentSearchPaths;

                CCScene gameScene = new CCScene(gameView);
                gameScene.AddLayer(new GameLayer());
                gameView.RunWithScene(gameScene);
            }
		}
	}
}


