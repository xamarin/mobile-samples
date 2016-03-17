using System;
using System.Linq;
using CocosSharp;
using Microsoft.Xna.Framework;
using CoinTimeGame.Entities;
using System.Collections.Generic;
using CoinTimeShared;

namespace CoinTimeGame.Scenes
{
	public partial class LevelSelectScene : CCScene
	{
		int pageNumber;
		CCLayer mainLayer;
		CCSprite background;
		CCSprite controllerHighlight;
		Button highlightTarget;

		Button navigateLeftButton;
		Button navigateRightButton;
		Button howToButton;

		IMenuController menuController;

		List<Button> levelButtons = new List<Button> ();
		List<Button> highlightableObjects = new List<Button>();

		const float levelButtonSpacing = 54;

		public LevelSelectScene (CCWindow mainWindow) : base(mainWindow)
		{
			PlatformInit ();

			CreateLayer ();

			CreateBackground ();

			CreateLogo ();

			CreateLevelButtons ();

			CreateNavigationButtons ();

			CreateHowToPlayButton ();

			CreateControllerHighlight ();

			RefreshHighlightableObjects ();

			Schedule(PerformActivity);
		}

		private void CreateHowToPlayButton()
		{
			howToButton = new Button (mainLayer);
			howToButton.ButtonStyle = ButtonStyle.LevelSelect;
			howToButton.PositionX = ContentSize.Center.X;
			howToButton.PositionY = 22;
			howToButton.Name = "HelpButton";
			howToButton.Text = "?";
			howToButton.Clicked += HandleHelpClicked;

			mainLayer.AddChild (howToButton);

		}

		private void HandleHelpClicked(object sender, EventArgs args)
		{

			CoinTime.GameAppDelegate.GoToHowToScene ();
		}

		private void RefreshHighlightableObjects()
		{
			highlightableObjects.Clear ();

			var visibleButtons = levelButtons.Where (item => item.Visible);

			highlightableObjects.AddRange (visibleButtons);

			if (navigateLeftButton.Visible)
			{
				highlightableObjects.Add (navigateLeftButton);
			}

			highlightableObjects.Add (howToButton);

			if (navigateRightButton.Visible)
			{
				highlightableObjects.Add (navigateRightButton);
			}
			highlightTarget = null;

		}

		partial void PlatformInit();

		private void CreateBackground()
		{
			background = new CCSprite ("ui/background.png");
			background.PositionX = ContentSize.Center.X;
			background.PositionY = ContentSize.Center.Y;
			background.IsAntialiased = false;
			mainLayer.AddChild (background);
		}

		private void CreateLogo()
		{
			background = new CCSprite ("ui/logo.png");
			background.PositionX = ContentSize.Center.X;
			const float offsetFromMiddle = 72;
			background.PositionY = ContentSize.Center.Y + offsetFromMiddle;
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
			navigateLeftButton.Clicked += HandleNavigateLeft;
			mainLayer.AddChild(navigateLeftButton);

			navigateRightButton = new Button (mainLayer);
			navigateRightButton.ButtonStyle = ButtonStyle.RightArrow;
			navigateRightButton.PositionX = ContentSize.Width - horizontalDistanceFromEdge;
			navigateRightButton.PositionY = verticalDistanceFromEdge;
			navigateRightButton.Name = "NavigateLeftButton";
			navigateRightButton.Clicked += HandleNavigateRight;

			mainLayer.AddChild(navigateRightButton);

			UpdateNavigationButtonVisibility ();
		}

		private void CreateControllerHighlight()
		{
			controllerHighlight = new CCSprite ("ui/controllerhighlight.png");
			controllerHighlight.IsAntialiased = false;
			// make this invisible, it will be turned on if any controllers are connected:
			controllerHighlight.Visible = false;


			mainLayer.AddChild (controllerHighlight);
			controllerHighlight.ZOrder = 1;
		}

		private void PerformActivity(float seconds)
		{
			if (menuController != null)
			{
				menuController.UpdateInputValues ();
				controllerHighlight.Visible = menuController.IsConnected;
			}


			if (controllerHighlight.Visible)
			{
				if (highlightTarget == null)
				{
					MoveHighlightTo (highlightableObjects [0]);
				}

				// for simplicity we'll just allow left/right movement, no up/down movement
				if (menuController.MovedLeft)
				{
					if (highlightTarget == highlightableObjects [0])
					{
						MoveHighlightTo (highlightableObjects.Last ());
					}
					else
					{
						var index = highlightableObjects.IndexOf (highlightTarget);
						MoveHighlightTo (highlightableObjects [index - 1]);
					}
				}
				if (menuController.MovedRight)
				{
					if (highlightTarget == highlightableObjects.Last ())
					{
						MoveHighlightTo (highlightableObjects [0]);
					}
					else
					{
						var index = highlightableObjects.IndexOf (highlightTarget);
						MoveHighlightTo (highlightableObjects [index + 1]);
					}
				}

				if (menuController.SelectPressed && highlightTarget != null)
				{
					highlightTarget.OnClicked ();
				}
			}
		}

		private void MoveHighlightTo(Button node)
		{
			controllerHighlight.Position = node.Position;
			highlightTarget = node;
		}

		private void UpdateNavigationButtonVisibility ()
		{
			navigateLeftButton.Visible = pageNumber > 0;

			navigateRightButton.Visible = (1+pageNumber) * 6 < LevelManager.Self.NumberOfLevels;
		}


		private void HandleNavigateLeft(object sender, EventArgs args)
		{
			pageNumber--;
			UpdateNavigationButtonVisibility ();

			DestroyLevelButtons ();
			CreateLevelButtons ();
			RefreshHighlightableObjects ();
		}


		private void HandleNavigateRight(object sender, EventArgs args)
		{
			pageNumber++;
			UpdateNavigationButtonVisibility ();

			DestroyLevelButtons ();
			CreateLevelButtons ();
			RefreshHighlightableObjects ();
		}


		private void CreateLayer()
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
			const float topRowOffsetFromCenter = 16;
			float topRowY = this.ContentSize.Center.Y + topRowOffsetFromCenter;

			for (int i = levelIndex0Based; i < maxLevelExclusive; i++)
			{
				var button = new Button (mainLayer);

				// Make it 1-based for non-programmers
				button.LevelNumber = i + 1;

				button.ButtonStyle = ButtonStyle.LevelSelect;

				button.PositionX = centerX - levelButtonSpacing + (buttonIndex % 3) * levelButtonSpacing;
				button.PositionY = topRowY - levelButtonSpacing * (buttonIndex / 3);
				button.Name = "LevelButton" + i;
				button.Clicked += HandleButtonClicked;
				levelButtons.Add (button);
				mainLayer.AddChild (button);

				buttonIndex++;
			}
		}

		private void DestroyLevelButtons()
		{
			for (int i = levelButtons.Count - 1; i > -1; i--)
			{
				mainLayer.RemoveChild (levelButtons [i]);
				levelButtons [i].Dispose ();
			}

			levelButtons.Clear ();
		}

		private void HandleButtonClicked(object sender, EventArgs args)
		{
			// levelNumber is 1-based, so subtract 1:
			var levelIndex = (sender as Button).LevelNumber - 1;

			LevelManager.Self.CurrentLevel = levelIndex;

			CoinTime.GameAppDelegate.GoToGameScene ();
		}

	}
}

