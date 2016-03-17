using System;
using System.Collections.Generic;
using CocosSharp;

namespace CoinTimeGame.Entities
{
	public class TouchScreenAnalogStick
	{
		int horizontalMovementTouchId = -1;

		float neutralPositionX;
		float distanceToMaxSpeed = 100;

		CCLayer owner;
		public CCLayer Owner
		{
			get
			{
				return owner;
			}
			set
			{
				owner = value;
				// If the user moves 1/6 of the screen from the initial touch location, the
				// character will move full speed.
				distanceToMaxSpeed = owner.ContentSize.Width / 6;
			}
		}



		public float HorizontalRatio
		{
			get;
			set;
		}

		public TouchScreenAnalogStick ()
		{
			
		}

		public void DetermineHorizontalRatio(List<CCTouch> touches)
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
					if (item.Location.X < Owner.ContentSize.Center.X)
					{
						horizontalMovementTouch = item;
						horizontalMovementTouchId = item.Id;

						neutralPositionX = item.Location.X;
					}
				}
			}

			if (horizontalMovementTouch != null)
			{
				HorizontalRatio = (horizontalMovementTouch.Location.X - neutralPositionX) / distanceToMaxSpeed ;

				// If the user moved further than distanceToMaxSpeed from the original touch location, then adjust
				// the touch location so that the user doesn't have to go back the opposite direction too far to return
				// back to neutral:
				neutralPositionX = System.Math.Max(neutralPositionX, horizontalMovementTouch.Location.X - distanceToMaxSpeed);
				neutralPositionX = System.Math.Min (neutralPositionX, horizontalMovementTouch.Location.X + distanceToMaxSpeed);


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
				HorizontalRatio = 0;
			}
		}
	}
}

