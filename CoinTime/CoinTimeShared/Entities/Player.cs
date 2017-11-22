using System;
using CocosSharp;
using CoinTimeGame.ContentRuntime.Animations;
using System.Collections.Generic;
using CoinTimeGame.Entities.Data;
using System.Linq;
using CocosDenshion;

namespace CoinTimeGame.Entities
{
	public enum LeftOrRight
	{
		Left,
		Right
	}

	public class Player : PhysicsEntity
	{
		Animation walkLeftAnimation;
		Animation walkRightAnimation;

		public bool IsOnGround
		{
			get;
			private set;
		}

		public Player ()
		{
			LoadAnimations ("Content/animations/playeranimations.achx");

			walkLeftAnimation = animations.Find (item => item.Name == "WalkLeft");
			walkRightAnimation = animations.Find (item => item.Name == "WalkRight");

			CurrentAnimation = walkLeftAnimation;
		}


		public void PerformActivity(float seconds)
		{
			ApplyMovementValues (seconds);

			AssignAnimation ();

			this.VelocityY = System.Math.Max (this.VelocityY, PlayerMovementCoefficients.MaxFallingSpeed);
		}

		private void AssignAnimation()
		{
			if (VelocityX > 0)
			{
				CurrentAnimation = walkRightAnimation;
			}
			else if (VelocityX < 0)
			{
				CurrentAnimation = walkLeftAnimation;
			}
			// if 0 do nothing
		}

		public void ApplyInput(float horizontalMovementRatio, bool jumpPressed)
		{
            AccelerationY = PlayerMovementCoefficients.GravityAcceleration;

			VelocityX = horizontalMovementRatio * PlayerMovementCoefficients.MaxHorizontalSpeed;

			if (jumpPressed && IsOnGround)
            {
                PerformJump();
            }
        }

        private void PerformJump()
        {
            CCSimpleAudioEngine.SharedEngine.PlayEffect("Jump");
            VelocityY = PlayerMovementCoefficients.JumpVelocity;
        }

        public void ReactToCollision(CCPoint reposition)
		{
			IsOnGround = reposition.Y > 0;

			ProjectVelocityOnSurface (reposition);
		}
	}
}

