using System;
using CocosSharp;
using CocosDenshion;

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
				var valueToDisplay = ((int)secondsLeft + 1).ToString();

                if (valueToDisplay != label.Text)
                {

                    label.Text = valueToDisplay;

                    if (secondsLeft < 5)
                    {
                        label.Color = CCColor3B.Red;
                        CCSimpleAudioEngine.SharedEngine.PlayEffect("TimerTick");
                    }
                    else
                    {
                        label.Color = CCColor3B.White;
                    }
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

			label = new CCLabel("Test", "fonts/Aldrich-Regular.ttf", 22, CCLabelFormat.SystemFont);
			label.IsAntialiased = false;
			label.PositionY = -2;
			this.AddChild (label);

			SecondsLeft = 60;
		}
	}
}

