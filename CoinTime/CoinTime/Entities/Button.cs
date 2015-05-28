using System;
using System.Linq;
using CocosSharp;
using System.Collections.Generic;


namespace CoinTimeGame.Entities
{
	public class Button : CCNode
	{
		CCSprite sprite;
		CCLabel label;
		CCLayer owner;


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

		public Button()
		{
			sprite = new CCSprite ("mastersheet.png");
			sprite.TextureRectInPixels = new CCRect (
				1024, 208, 64, 24
			);
			sprite.ContentSize = sprite.TextureRectInPixels.Size;
			this.AddChild (sprite);

			label = new CCLabel("Test", "fonts/alphbeta.ttf", 16, CCLabelFormat.SystemFont);
			this.AddChild (label);

			var touchListener = new CCEventListenerTouchAllAtOnce ();
			touchListener.OnTouchesBegan = HandleTouchesBegan;
			sprite.AddEventListener (touchListener);

		}

		private void HandleTouchesBegan(List<CCTouch> touches, CCEvent touchEvent)
		{
			// did the user actually click within the CCSprite bounds?
			var firstTouch = touches.FirstOrDefault();

			if (firstTouch != null)
			{
				bool isTouchInside = sprite.BoundingBoxTransformedToWorld.ContainsPoint (firstTouch.Location);

				if(isTouchInside && Clicked != null)
				{
					Clicked (this, null);
				}
			}
		}
	}
}

