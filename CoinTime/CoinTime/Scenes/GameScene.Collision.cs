using System;
using CocosSharp;

namespace CoinTimeGame.Scenes
{
	public partial class GameScene
	{
		void PerformCollision(float seconds)
		{
			PerformPlayerVsCoinsCollision ();

			PerformPlayerVsDoorCollision ();

			PerformPlayerVsEnvironmentCollision ();

			PlayerVsDamageDealersCollision ();

			PerformEnemiesVsEnvironmentCollision ();
		}

		bool PerformPlayerVsCoinsCollision()
		{
			bool grabbedAnyCoins = false;

			// Reverse loop since items may get removed from the list
			for (int i = coins.Count - 1; i > -1; i--)
			{
				if (player.Intersects (coins [i]))
				{
					var coinToDestroy = coins [i];

					DestroyCoin (coinToDestroy);

					grabbedAnyCoins = true;
				}
			}

			if (grabbedAnyCoins && coins.Count == 0)
			{
				// User got all the coins, so open the door
				if (door != null)
				{
					door.IsOpen = true;
				}
			}


			return grabbedAnyCoins;
		}

		void PerformPlayerVsDoorCollision()
		{
			if (door != null && door.IsOpen && player.Intersects (door))
			{
				DestroyLevel ();
				// handle going to the next level
				currentLevelIndex++;
				GoToLevel(currentLevelIndex);
			}
		}

		void PerformPlayerVsEnvironmentCollision ()
		{
			CCPoint positionBeforeCollision = player.Position;
			CCPoint reposition = CCPoint.Zero;
			if (levelCollision.PerformCollisionAgainst (player))
			{
				reposition = player.Position - positionBeforeCollision;
			}
			player.ReactToCollision (reposition);
		}

		void PlayerVsDamageDealersCollision()
		{
			for (int i = 0; i < damageDealers.Count; i++)
			{
				if (player.BoundingBoxWorld.IntersectsRect (damageDealers [i].BoundingBoxWorld))
				{
					HandlePlayerDeath ();
					break;
				}
			}
		}

		void PerformEnemiesVsEnvironmentCollision()
		{
			for (int i = 0; i < enemies.Count; i++)
			{
				var enemy = enemies [i];
				CCPoint positionBeforeCollision = enemy.Position;
				CCPoint reposition = CCPoint.Zero;

				if (levelCollision.PerformCollisionAgainst (enemy))
				{
					reposition = enemy.Position - positionBeforeCollision;
				}

				enemy.ReactToCollision (reposition);
			}
		}

	}
}

