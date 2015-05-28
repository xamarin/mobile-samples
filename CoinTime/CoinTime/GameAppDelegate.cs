using System;
using CocosSharp;
using CoinTimeGame.Scenes;


namespace CoinTime
{
	public class GameAppDelegate : CCApplicationDelegate
	{
		static CCScene currentScene;

		public override void ApplicationDidFinishLaunching (CCApplication application, CCWindow mainWindow)
		{
			application.PreferMultiSampling = false;
			application.ContentRootDirectory = "Content";
			application.ContentSearchPaths.Add ("animations");
			application.ContentSearchPaths.Add ("fonts");
			application.ContentSearchPaths.Add ("images");
			application.ContentSearchPaths.Add ("levels");
			application.ContentSearchPaths.Add ("sounds");

			CCSize windowSize = mainWindow.WindowSizeInPixels;

			// Use the SNES resolution:
			float desiredWidth = 256.0f;
			float desiredHeight = 224.0f;
            
			CCScene.SetDefaultDesignResolution (desiredWidth, desiredHeight, CCSceneResolutionPolicy.ShowAll);
            
			var scene = new LevelSelectScene (mainWindow);

			mainWindow.RunWithScene (scene);
		}

		public override void ApplicationDidEnterBackground (CCApplication application)
		{
			application.Paused = true;
		}

		public override void ApplicationWillEnterForeground (CCApplication application)
		{
			application.Paused = false;
		}

		// for this game (with only 2 scenes) we're just going to handle moving between them here
		public static void GoToGameScene()
		{
			DestroyCurrentScene();

		}

		private static DestroyCurrentScene()
		{
			if(currentScene != null)
			{

			}
		}
	}
}
