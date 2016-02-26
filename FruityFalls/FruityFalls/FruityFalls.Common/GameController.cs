using System;
using CocosSharp;
using System.Collections.Generic;
using FruityFalls.Scenes;

namespace FruityFalls
{
	public static class GameController
	{
        public static CCGameView GameView
        {
            get;
            private set;
        }

		public static void Initialize (CCGameView gameView)
		{
            GameView = gameView;

            var contentSearchPaths = new List<string> () { "Fonts", "Sounds" };
			CCSizeI viewSize = GameView.ViewSize;

			int width = 768;
			int height = 1024;

			// Set world dimensions
			GameView.DesignResolution = new CCSizeI (width, height);

            // Determine whether to use the high or low def versions of our images
            // Make sure the default texel to content size ratio is set correctly
            // Of course you're free to have a finer set of image resolutions e.g (ld, hd, super-hd)

            CCSprite.DefaultTexelToContentSizeRatio = 0.5f;
            contentSearchPaths.Add ("Images");

			GameView.ContentManager.SearchPaths = contentSearchPaths;
			var scene = new TitleScene (GameView);
			GameView.Director.RunWithScene (scene);
		}

        public static void GoToScene(CCScene scene)
        {
            GameView.Director.ReplaceScene(scene);
        }
	}
}

