using System;
using CocosSharp;
using CoinTimeGame.Entities;

namespace CoinTimeGame.Scenes
{
	public class HowToPlayScene : CCScene
	{
		CCLayer mainLayer;
		CCSprite background;
		CCSprite howToImage;

		CCDrawNode labelBackground;
		CCLabel howToLabel;
		Button backButton;

		public HowToPlayScene (CCWindow mainWindow) : base(mainWindow)
		{
			CreateLayer ();

			CreateBackground ();

			CreateHowToImage ();

			CreateHowtoLabel ();

			CreateBackButton ();
		}

		private void CreateLayer()
		{
			mainLayer = new CCLayer ();
			this.AddChild (mainLayer);
		}

		private void CreateBackground()
		{
			background = new CCSprite ("ui/background.png");
			background.PositionX = ContentSize.Center.X;
			background.PositionY = ContentSize.Center.Y;
			background.IsAntialiased = false;
			mainLayer.AddChild (background);
		}

		private void CreateHowToImage()
		{
			howToImage = new CCSprite ("ui/howtoscreenshot.png");
			// Make this positioned by its center:
			howToImage.AnchorPoint = new CCPoint (.5f, .5f);

			howToImage.PositionX = ContentSize.Center.X;
			howToImage.PositionY = ContentSize.Center.Y + 20;
			howToImage.Scale = .5f;
			howToImage.IsAntialiased = false;
			mainLayer.AddChild (howToImage);

		}

		private void CreateHowtoLabel()
		{
			float backgroundWidth = howToImage.ScaledContentSize.Width;
			const float backgroundHeight = 36;

			labelBackground = new CCDrawNode ();

			var rect = new CCRect (
				-backgroundWidth / 2.0f, -backgroundHeight / 2.0f, 
				backgroundWidth , backgroundHeight );
			labelBackground.DrawRect (
				rect, CCColor4B.Black);
			labelBackground.PositionX = ContentSize.Center.X;
			labelBackground.PositionY = 74;
			mainLayer.AddChild (labelBackground);


			howToLabel = new CCLabel(
				"Touch and move on the left side of the screen to move.\nTap on the right side to jump.", 
				"fonts/Aldrich-Regular.ttf", 12, CCLabelFormat.SystemFont);
			howToLabel.PositionX = ContentSize.Center.X;
			howToLabel.Scale = .5f;
			howToLabel.PositionY = 74;
			howToLabel.HorizontalAlignment = CCTextAlignment.Center;
			howToLabel.VerticalAlignment = CCVerticalTextAlignment.Center;
			howToLabel.IsAntialiased = false;

			mainLayer.AddChild (howToLabel);
		}

		private void CreateBackButton()
		{
			const float horizontalDistanceFromEdge = 36;
			const float verticalDistanceFromEdge = 28;

			backButton = new Button (mainLayer);
			backButton.ButtonStyle = ButtonStyle.LeftArrow;
			backButton.PositionX = verticalDistanceFromEdge;
			backButton.PositionY = ContentSize.Height - verticalDistanceFromEdge;
			backButton.Name = "backButton";
			backButton.Clicked += HandleBackClicked;
			mainLayer.AddChild (backButton);

		}

		private void HandleBackClicked(object sender, EventArgs args)
		{
			CoinTime.GameAppDelegate.GoToLevelSelectScene ();
		}
	}
}

