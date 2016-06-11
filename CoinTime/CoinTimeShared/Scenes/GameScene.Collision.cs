using System;
using CocosSharp;
using CocosDenshion;
using CoinTime;

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

                    CCSimpleAudioEngine.SharedEngine.PlayEffect("CoinPickup");

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
                CCSimpleAudioEngine.SharedEngine.PlayEffect("EnteringDoor");
				try
				{
					bool isLastLevel = (LevelManager.Self.CurrentLevel + 1 == LevelManager.Self.NumberOfLevels);

					if(isLastLevel)
					{
						GameAppDelegate.GoToLevelSelectScene();
					}
					else
					{
						DestroyLevel ();
						LevelManager.Self.CurrentLevel++;
						GoToLevel(LevelManager.Self.CurrentLevel);
					}
				}
				catch(Exception e)
				{
					int m = 3;
				}
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

