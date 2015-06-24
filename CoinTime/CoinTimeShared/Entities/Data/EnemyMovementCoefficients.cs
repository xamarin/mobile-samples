using System;

namespace CoinTimeGame.Entities.Data
{
	public class EnemyMovementCoefficients
	{
		// This should be slightly slower than the player
		public const float MaxHorizontalSpeed = 40;
		public const float GravityAcceleration = -390;
		public const float JumpVelocity = 240;
		public const float MaxFallingSpeed = -160;
	}
}

