using System;
using CocosSharp;
using System.Collections.Generic;

namespace CoinTimeGame.Entities
{
	public class TouchScreenInput : IDisposable
	{
		CCEventListenerTouchAllAtOnce touchListener;
        CCLayer owner;
		int horizontalMovementTouchId = -1;
		bool touchedOnRightSide = false;

        public float HorizontalRatio
        {
            get;
            private set;
        }

		public bool WasJumpPressed
		{
			get;
			private set;
		}


		public TouchScreenInput(CCLayer owner)
		{
            this.owner = owner;

			touchListener = new CCEventListenerTouchAllAtOnce ();
			touchListener.OnTouchesMoved = HandleTouchesMoved;
			touchListener.OnTouchesBegan = HandleTouchesBegan;
			// todo - need to destroy this:
			owner.AddEventListener (touchListener);

		}

		void HandleTouchesBegan(List<CCTouch> touches, CCEvent touchEvent)
		{
			foreach (var item in touches)
			{
				if (item.Location.X > owner.ContentSize.Center.X)
				{
					touchedOnRightSide = true;
				}
			}
		}

		void HandleTouchesMoved (List<CCTouch> touches, CCEvent touchEvent)
		{
			DetermineHorizontalRatio (touches);

		}

		public void Dispose()
		{
			owner.RemoveEventListener (touchListener);
		}

		void DetermineHorizontalRatio(List<CCTouch> touches)
		{
			CCTouch horizontalMovementTouch = null;

			if (horizontalMovementTouchId != -1)
			{
				foreach (var item in touches)
				{
					if (item.Id == horizontalMovementTouchId)
					{
						horizontalMovementTouch = item;
						break;
					}
				}
			}

			if (horizontalMovementTouch == null)
			{
				// Couldn't find one or we have a -1 ID. Let's set
				// the ID to -1 to indicate we don't have a valid touch:
				horizontalMovementTouchId = -1;
			}

			if (horizontalMovementTouch == null)
			{
				// let's see if we can find one that is to the left of the screen
				foreach (var item in touches)
				{
					if (item.Location.X < owner.ContentSize.Center.X)
					{
						horizontalMovementTouch = item;
						horizontalMovementTouchId = item.Id;
					}
				}
			}

			if (horizontalMovementTouch != null)
			{
				float quarterWidth = owner.ContentSize.Width / 4;
				HorizontalRatio = (horizontalMovementTouch.Location.X - quarterWidth) / quarterWidth ;

				HorizontalRatio = Math.Min (1, HorizontalRatio);
				HorizontalRatio = Math.Max (-1, HorizontalRatio);

				const float deadZone = .15f;
				if (Math.Abs (HorizontalRatio) < deadZone)
				{
					HorizontalRatio = 0;
				}
			}
			else
			{
				// for emulator testing, we'll turn this off, but we need it on eventually on device:
//				HorizontalRatio = 0;
			}
		}

        public void UpdateInputValues()
        {
			WasJumpPressed = touchedOnRightSide;
			touchedOnRightSide = false;
            // todo: process "jump"
            //throw new NotImplementedException();
        }
    }
}

