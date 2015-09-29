﻿using System;
using CocosSharp;

namespace CoinTimeGame.Entities
{
	public class Timer : CCNode
	{
		CCLabel label;
		CCSprite sprite;

		float secondsLeft;
		public float SecondsLeft
		{
			get
			{
				return secondsLeft;
			}
			set
			{
				secondsLeft = value;

				// The display should show a "1" even if there is less than 1 second
				// left, so that the timer shows 0 right when the level ends
				int valueToDisplay = (int)secondsLeft + 1;

				label.Text = valueToDisplay.ToString();

				if (secondsLeft < 5)
				{
					label.Color = CCColor3B.Red;
				}
				else
				{
					label.Color = CCColor3B.White;
				}
			}
		}

		public Timer()
		{
			sprite = new CCSprite ("mastersheet.png");
			sprite.TextureRectInPixels = new CCRect (
				1024, 208, 64, 24);
			sprite.ContentSize = sprite.TextureRectInPixels.Size;
			this.AddChild (sprite);

			label = new CCLabel("Test", "fonts/alphbeta.ttf", 22, CCLabelFormat.SystemFont);
			label.IsAntialiased = false;
			this.AddChild (label);

			SecondsLeft = 60;
		}
	}
}

