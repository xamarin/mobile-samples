using System;
using System.Collections.Generic;
using UIKit;
using CocosSharp;

namespace RenderTextureExample.iOS
{
	public partial class ViewController : UIViewController
	{
		public ViewController (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			if (GameView != null)
			{
				// Set loading event to be called once game view is fully initialised
				GameView.ViewCreated += LoadGame;
			}
		}

		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);

			if (GameView != null)
				GameView.Paused = true;
		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);

			if (GameView != null)
				GameView.Paused = false;
		}

		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
			// Release any cached data, images, etc that aren't in use.
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

