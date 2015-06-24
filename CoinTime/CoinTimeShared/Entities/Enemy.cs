using System;
using System.Linq;
using CocosSharp;
using CoinTimeGame.Entities.Data;
using CoinTimeGame.ContentRuntime.Animations;

namespace CoinTimeGame.Entities
{


	public class Enemy : PhysicsEntity, IDamageDealer
	{
		LeftOrRight directionFacing = LeftOrRight.Left;


		Animation walkLeftAnimation;
		Animation walkRightAnimation;

		public Enemy ()
		{
			LoadAnimations ("Content/animations/blueenemyanimations.achx");

			CurrentAnimation = animations [0];

			walkLeftAnimation = animations.Find (item => item.Name == "WalkLeft");
			walkRightAnimation = animations.Find (item => item.Name == "WalkRight");

			this.VelocityX = 0;

			this.AccelerationY = PlayerMovementCoefficients.GravityAcceleration;
		}

		public void PerformActivity(float seconds)
		{
			PerformAi ();

			AssignAnimation ();

			ApplyMovementValues (seconds);
		}

		private void AssignAnimation()
		{
			switch (directionFacing)
			{
			case LeftOrRight.Left:
				this.CurrentAnimation = walkLeftAnimation;
				break;
			case LeftOrRight.Right:
				this.CurrentAnimation = walkRightAnimation;
				break;
			}
		}

		private void PerformAi()
		{
			switch (directionFacing)
			{
			case LeftOrRight.Left:
				this.VelocityX = -EnemyMovementCoefficients.MaxHorizontalSpeed;
				break;
			case LeftOrRight.Right:
				this.VelocityX = EnemyMovementCoefficients.MaxHorizontalSpeed;
				break;
			}
		}

		public void ReactToCollision(CCPoint reposition)
		{
			ProjectVelocityOnSurface (reposition);

			// account for floating point error:
			const float epsilon = .0001f;

			if (System.Math.Abs (reposition.X) > epsilon)
			{
				if (reposition.X > 0 && directionFacing == LeftOrRight.Left)
				{
					directionFacing = LeftOrRight.Right;
				}
				else if (reposition.X < 0 && directionFacing == LeftOrRight.Right)
				{
					directionFacing = LeftOrRight.Left;
				}
			}
		}
	}
}

