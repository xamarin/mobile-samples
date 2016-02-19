using System;
using CocosSharp;
using System.Collections.Generic;

namespace CoinTimeGame.Entities
{
	public class TouchScreenInput : IDisposable
	{
		CCEventListenerTouchAllAtOnce touchListener;
        CCLayer owner;
		bool touchedOnRightSide = false;

		TouchScreenAnalogStick analogStick;

        public float HorizontalRatio
        {
            get
			{
				return analogStick.HorizontalRatio;
			}
        }

		public bool WasJumpPressed
		{
			get;
			private set;
		}


		public TouchScreenInput(CCLayer owner)
		{
            this.owner = owner;

			analogStick = new TouchScreenAnalogStick ();
			analogStick.Owner = owner;

			touchListener = new CCEventListenerTouchAllAtOnce ();
			touchListener.OnTouchesMoved = HandleTouchesMoved;
			touchListener.OnTouchesBegan = HandleTouchesBegan;
			owner.AddEventListener (touchListener);

		}

		private void HandleTouchesBegan(List<CCTouch> touches, CCEvent touchEvent)
		{
			foreach (var item in touches)
			{
				if (item.Location.X > owner.ContentSize.Center.X)
				{
					touchedOnRightSide = true;
				}
			}
		}

		private void HandleTouchesMoved (List<CCTouch> touches, CCEvent touchEvent)
		{
			analogStick.DetermineHorizontalRatio (touches);

		}

		public void Dispose()
		{
			owner.RemoveEventListener (touchListener);
		}



        public void UpdateInputValues()
        {
			WasJumpPressed = touchedOnRightSide;
			touchedOnRightSide = false;
        }
    }
}

