using System;
using System.Linq;
using CocosSharp;
using System.Collections.Generic;


namespace CoinTimeGame.Entities
{
	public enum ButtonStyle
	{
		LevelSelect,
		LeftArrow,
		RightArrow
	}


	public class Button : CCNode
	{
		CCSprite sprite;
		CCLabel label;
		CCLayer owner;

		ButtonStyle buttonStyle;
		public ButtonStyle ButtonStyle
		{
			get
			{
				return buttonStyle;
			}
			set
			{
				buttonStyle = value;

				switch (buttonStyle)
				{
				case ButtonStyle.LevelSelect:
					sprite.Texture = new CCTexture2D ("ui/buttonup.png");
					sprite.FlipX = false;
					break;
	
				case ButtonStyle.LeftArrow:
					sprite.Texture = new CCTexture2D ("ui/arrowup.png");
					sprite.FlipX = true;
					break;
				case ButtonStyle.RightArrow:
					sprite.Texture = new CCTexture2D ("ui/arrowup.png");

					sprite.FlipX = false;
					break;
				}

				sprite.IsAntialiased = false;
				sprite.TextureRectInPixels = 
					new CCRect (0, 0,
					sprite.Texture.PixelsWide,
					sprite.Texture.PixelsHigh);
			}
		}

		int levelNumber;

		public event EventHandler Clicked;

		public int LevelNumber
		{
			get
			{
				return levelNumber;
			}
			set
			{
				levelNumber = value;

				label.Text = levelNumber.ToString ();
			}
		}

		public string Text
		{
			get
			{
				return label.Text;
			}
			set
			{
				label.Text = value;
			}
		}

		public Button(CCLayer layer)
		{
			// Give it a default texture, may get changed in ButtonStyle
			sprite = new CCSprite ("ui/buttonup.png");
			sprite.IsAntialiased = false;
			this.AddChild (sprite);

			label = new CCLabel("", "fonts/Aldrich-Regular.ttf", 24, CCLabelFormat.SystemFont);
			label.IsAntialiased = false;
			this.AddChild (label);

			var touchListener = new CCEventListenerTouchAllAtOnce ();
			touchListener.OnTouchesBegan = HandleTouchesBegan;
			layer.AddEventListener (touchListener);

		}

		private void HandleTouchesBegan(List<CCTouch> touches, CCEvent touchEvent)
		{
			if (this.Visible)
			{
				// did the user actually click within the CCSprite bounds?
				var firstTouch = touches.FirstOrDefault ();

				if (firstTouch != null)
				{
				
					bool isTouchInside = sprite.BoundingBoxTransformedToWorld.ContainsPoint (firstTouch.Location);

					if (isTouchInside && Clicked != null)
					{
						Clicked (this, null);
					}
				}
			}
		}
	
		public void OnClicked()
		{
			if (Clicked != null)
			{
				Clicked (this, null);
			}
		}
	}
}

