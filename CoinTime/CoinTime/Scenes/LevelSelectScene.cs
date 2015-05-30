using System;
using CocosSharp;
using Microsoft.Xna.Framework;
using CoinTimeGame.Entities;

namespace CoinTimeGame.Scenes
{
	public class LevelSelectScene : CCScene
	{
		int pageNumber;
		CCLayer mainLayer;
		CCSprite background;

		Button navigateLeftButton;
		Button navigateRightButton;


		public LevelSelectScene (CCWindow mainWindow) : base(mainWindow)
		{
			CreateLayers ();

			CreateBackground ();

			CreateLevelButtons ();

			CreateNavigationButtons ();
		}

		private void CreateBackground()
		{
			background = new CCSprite ("ui/background.png");
			background.PositionX = ContentSize.Center.X;
			background.PositionY = ContentSize.Center.Y;
			background.IsAntialiased = false;
			mainLayer.AddChild (background);
		}

		private void CreateNavigationButtons()
		{
			const float horizontalDistanceFromEdge = 36;
			const float verticalDistanceFromEdge = 28;

			navigateLeftButton = new Button (mainLayer);
			navigateLeftButton.ButtonStyle = ButtonStyle.LeftArrow;
			navigateLeftButton.PositionX = horizontalDistanceFromEdge;
			navigateLeftButton.PositionY = verticalDistanceFromEdge;
			navigateLeftButton.Name = "NavigateLeftButton";
			mainLayer.AddChild(navigateLeftButton);

			navigateRightButton = new Button (mainLayer);
			navigateRightButton.ButtonStyle = ButtonStyle.RightArrow;
			navigateRightButton.PositionX = ContentSize.Width - horizontalDistanceFromEdge;
			navigateRightButton.PositionY = verticalDistanceFromEdge;
			navigateRightButton.Name = "NavigateLeftButton";
			mainLayer.AddChild(navigateRightButton);

			UpdateNavigationButtonVisibility ();
		}

		private void UpdateNavigationButtonVisibility ()
		{
			navigateLeftButton.Visible = pageNumber > 0;

			navigateRightButton.Visible = (1+pageNumber) * 6 < LevelManager.Self.NumberOfLevels;
		}



		private void CreateLayers()
		{
			mainLayer = new CCLayer ();
			this.AddChild (mainLayer);

		}

		private void CreateLevelButtons()
		{
			const int buttonsPerPage = 6;
			int levelIndex0Based = buttonsPerPage * pageNumber;

			int maxLevelExclusive = System.Math.Min (levelIndex0Based + 6, LevelManager.Self.NumberOfLevels);
			int buttonIndex = 0;

			float centerX = this.ContentSize.Center.X;
			float topRowY = this.ContentSize.Center.Y + 20;
			const float spacing = 54;

			for (int i = levelIndex0Based; i < maxLevelExclusive; i++)
			{
				var button = new Button (mainLayer);

				// Make it 1-based for non-programmers
				button.LevelNumber = i + 1;

				button.ButtonStyle = ButtonStyle.LevelSelect;

				button.PositionX = centerX - spacing + (buttonIndex % 3) * spacing;
				button.PositionY = topRowY - spacing * (buttonIndex / 3);
				button.Name = "LevelButton" + i;
				button.Clicked += HandleButtonClicked;

				mainLayer.AddChild (button);

				buttonIndex++;
			}
		}

		private void HandleButtonClicked(object sender, EventArgs args)
		{
			// levelNumber is 1-based, so subtract 1:
			var levelIndex = (sender as Button).LevelNumber - 1;

			LevelManager.Self.CurrentLevel = levelIndex;

			CoinTime.GameAppDelegate.GoToGameScene ();
			// go to game screen
		}

	}
}

