using System;
using CocosSharp;
using Microsoft.Xna.Framework;
using CoinTimeGame.Entities;

namespace CoinTimeGame.Scenes
{
	public class LevelSelectScene : CCScene
	{

		CCLayer mainLayer;

		public LevelSelectScene (CCWindow mainWindow) : base(mainWindow)
		{
			CreateLayers ();

			CreateButtons ();

			RegisterTouchListeners();
		}

		private void CreateLayers()
		{
			mainLayer = new CCLayer ();
			this.AddChild (mainLayer);

		}

		private void CreateButtons()
		{
			int currentIndex;

			// This game relies on levels being named "levelx.tmx" where x is an integer beginning with
			// 1. We have to rely on XNA's TitleContainer which doesn't give us a GetFiles method - we simply
			// have to check if a file exists, and if we get an exception on the call then we know the file doesn't
			// exist. 
			int levelIndex = 0;

			while (true)
			{
				bool fileExists = false;

				try
				{
					using(var stream = TitleContainer.OpenStream("Content/levels/level" + levelIndex + ".tmx"))
					{
						
					}
					// if we got here then the file exists!
					fileExists = true;
				}
				catch
				{
					// do nothing, fileExists will remain false
				}

				if (!fileExists)
				{
					break;
				}
				else
				{
					levelIndex++;
				}
			}

			for (int i = 0; i < levelIndex; i++)
//			for (int i = 0; i < 1; i++)
			{
				var button = new Button ();

				// Make it 1-based for non-programmers
				button.LevelNumber = i + 1;

				button.PositionX = 20 + i * 70;
				button.PositionY = 150;

				button.Clicked += HandleButtonClicked;

				mainLayer.AddChild (button);

			}
		}

		private void HandleButtonClicked(object sender, EventArgs args)
		{
			// levelNumber is 1-based, so subtract 1:
			var levelIndex = (sender as Button).LevelNumber - 1;

			GameScene.LevelIndex = levelIndex;

			CoinTime.GameAppDelegate.GoToGameScene ();
			// go to game screen
		}

		private void RegisterTouchListeners()
		{
			// todo
		}

	}
}

