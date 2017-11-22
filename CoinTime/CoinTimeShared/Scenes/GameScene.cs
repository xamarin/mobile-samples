using System;
using CocosSharp;
using CocosDenshion;
using CoinTimeGame.Entities;
using System.Collections.Generic;
using CoinTimeGame.TilemapClasses;
using CoinTime;
using CoinTimeShared;

namespace CoinTimeGame.Scenes
{
	public partial class GameScene : CCScene
	{
		CCLayer gameplayLayer;
		CCLayer hudLayer;

		CCTileMap currentLevel;
		LevelCollision levelCollision;
		CCTileMapLayer backgroundLayer;
			
		TouchScreenInput touchScreen;
		IGameController controller;

		float secondsLeft;

		Player player;
		Door door;

		const int secondsPerLevel = 30;
		Timer timer;

		List<IDamageDealer> damageDealers = new List<IDamageDealer>();
		List<Enemy> enemies = new List<Enemy>();
		List<Coin> coins = new List<Coin>();

		public GameScene (CCWindow mainWindow) : base(mainWindow)
		{
			PlatformInit ();

			CreateLayers ();

			CreateHud ();

			GoToLevel (LevelManager.Self.CurrentLevel);

			Schedule(PerformActivity);
		}

		partial void PlatformInit();

		private void CreateHud()
		{
			timer = new Timer ();
			timer.PositionX = this.ContentSize.Center.X;
			timer.PositionY = this.ContentSize.Height - 20;
			hudLayer.AddChild (timer);

			var backButton = new Button (hudLayer);
			backButton.ButtonStyle = ButtonStyle.LeftArrow;
			backButton.Clicked += HandleBackClicked;
			backButton.PositionX = 30;
			backButton.PositionY = ContentSize.Height - 30;
			hudLayer.AddChild (backButton);

		}

		private void HandleBackClicked(object sender, EventArgs args)
		{
			GameAppDelegate.GoToLevelSelectScene ();
		}

		private void PerformActivity(float seconds)
		{
			player.PerformActivity (seconds);

			for (int i = 0; i < enemies.Count; i++)
			{
				enemies [i].PerformActivity (seconds);
			}

			ApplyInput(seconds);

			PerformCollision(seconds);

			PerformScrolling ();

			PerformTimerActivity (seconds);
		}

		private void PerformTimerActivity(float seconds)
		{
			// This suffers from accumulation error:
			secondsLeft -= seconds;
			timer.SecondsLeft = secondsLeft;

			if (secondsLeft <= 0)
			{
				HandlePlayerDeath ();
			}
		}

		private void PerformScrolling ()
		{
			float effectivePlayerX = player.PositionX;

			// Effective values limit the scorlling beyond the level's bounds
			effectivePlayerX = System.Math.Max (player.PositionX, this.ContentSize.Center.X);
			float levelWidth = currentLevel.TileTexelSize.Width * currentLevel.MapDimensions.Column;
			effectivePlayerX = System.Math.Min (effectivePlayerX, levelWidth - this.ContentSize.Center.X);

			float effectivePlayerY = player.PositionY;
			float levelHeight = currentLevel.TileTexelSize.Height * currentLevel.MapDimensions.Row;
			effectivePlayerY = System.Math.Min(player.Position.Y, levelHeight - this.ContentSize.Center.Y);
			// We don't want to limit the scrolling on Y - instead levels should be large enough
			// so that the view never reaches the bottom. This allows the user to play
			// with their thumbs without them getting in the way of the game.

			float positionX = -effectivePlayerX + this.ContentSize.Center.X;
			float positionY = -effectivePlayerY + this.ContentSize.Center.Y;

			gameplayLayer.PositionX = positionX;
			gameplayLayer.PositionY = positionY;

			// We don't want the background to scroll, 
			// so we'll make it move the opposite direction of the rest of the tilemap:
			if (backgroundLayer != null)
			{
				backgroundLayer.PositionX = -positionX;
				backgroundLayer.PositionY = -positionY;
			}

			currentLevel.TileLayersContainer.PositionX = positionX;
			currentLevel.TileLayersContainer.PositionY = positionY;
		}

		private void CreateLayers()
		{
			gameplayLayer = new CCLayer ();
			this.AddChild (gameplayLayer);

			hudLayer = new CCLayer ();
			this.AddChild (hudLayer);
		}


		private void GoToLevel(int levelNumber)
        {
            LoadLevel(levelNumber);

            CreateCollision();

            ProcessTileProperties();

            secondsLeft = secondsPerLevel;
        }

        private void CreateCollision()
        {
            levelCollision = new LevelCollision();
            levelCollision.PopulateFrom(currentLevel);
        }

        private void LoadLevel(int levelNumber)
		{
			currentLevel = new CCTileMap ("level" + levelNumber + ".tmx");
			currentLevel.Antialiased = false;
			backgroundLayer = currentLevel.LayerNamed ("Background");

			// CCTileMap is a CCLayer, so we'll just add it under all entities
			this.AddChild (currentLevel);

			// put the game layer after
			this.RemoveChild(gameplayLayer);
			this.AddChild(gameplayLayer);

			this.RemoveChild (hudLayer);
			this.AddChild (hudLayer);
		}

		private void ProcessTileProperties()
		{
			TileMapPropertyFinder finder = new TileMapPropertyFinder (currentLevel);
			foreach (var propertyLocation in finder.GetPropertyLocations())
			{
				var properties = propertyLocation.Properties;
				if (properties.ContainsKey ("EntityType"))
				{
					float worldX = propertyLocation.WorldX;
					float worldY = propertyLocation.WorldY;

					if (properties.ContainsKey ("YOffset"))
					{
						string yOffsetAsString = properties ["YOffset"];
						float yOffset = 0;
						float.TryParse (yOffsetAsString, out yOffset);
						worldY += yOffset;
					}

					bool created = TryCreateEntity (properties ["EntityType"], worldX, worldY);

					if (created)
					{
						propertyLocation.Layer.RemoveTile (propertyLocation.TileCoordinates);
					}
				}
				else if (properties.ContainsKey ("RemoveMe"))
				{
					propertyLocation.Layer.RemoveTile (propertyLocation.TileCoordinates);
				}
			}

			touchScreen = new TouchScreenInput(gameplayLayer);
		}

		private bool TryCreateEntity(string entityType, float worldX, float worldY)
		{
			CCNode entityAsNode = null;

			switch (entityType)
			{
			case "Player":
				player = new Player ();
				entityAsNode = player;
				break;
			case "Coin":
				Coin coin = new Coin ();
				entityAsNode = coin;
				coins.Add (coin);
				break;
			case "Door":
				door = new Door ();
				entityAsNode = door;
				break;
			case "Spikes":
				var spikes = new Spikes ();
				this.damageDealers.Add (spikes);
				entityAsNode = spikes;
				break;
			case "Enemy":
				var enemy = new Enemy ();
				this.damageDealers.Add (enemy);
				this.enemies.Add (enemy);
				entityAsNode = enemy;
				break;
			}

			if(entityAsNode != null)
			{
				entityAsNode.PositionX = worldX;
				entityAsNode.PositionY = worldY;
				gameplayLayer.AddChild (entityAsNode);
			}

			return entityAsNode != null;
		}

		private void ApplyInput(float seconds)
		{
			if (controller != null && controller.IsConnected)
			{
				controller.UpdateInputValues ();
				player.ApplyInput (controller.HorizontalRatio, controller.JumpPressed);

				if (controller.BackPressed)
				{
					GameAppDelegate.GoToLevelSelectScene ();
				}
			}
			else
			{
				touchScreen.UpdateInputValues ();
				player.ApplyInput (touchScreen.HorizontalRatio, touchScreen.WasJumpPressed);
			}
		}

		private void DestroyCoin(Coin coinToDestroy)
		{
			coins.Remove (coinToDestroy);
			gameplayLayer.RemoveChild (coinToDestroy);
			coinToDestroy.Dispose ();
		}

		private void DestroyDamageDealer(IDamageDealer damageDealer)
		{
			damageDealers.Remove (damageDealer);
			if (damageDealer is CCNode)
			{
				var asNode = damageDealer as CCNode;
				gameplayLayer.RemoveChild (asNode);
				asNode.Dispose ();
			}
		}

		private void DestroyLevel()
		{
			gameplayLayer.RemoveChild (player);
			player.Dispose ();

			gameplayLayer.RemoveChild (door);
			door.Dispose ();

			for (int i = coins.Count - 1; i > -1; i--)
			{
				var coinToDestroy = coins [i];
				DestroyCoin (coinToDestroy);
			}

			for (int i = damageDealers.Count - 1; i > -1; i--)
			{
				var damageDealer = damageDealers [i];

				DestroyDamageDealer (damageDealer);
			}

			// We can just clear the list - the contained entities are destroyed as damage dealers:
			enemies.Clear ();

			touchScreen.Dispose ();


			this.RemoveChild (currentLevel);
			currentLevel.Dispose ();

			// don't think we need to do anything with LevelCollision

		}

		private void HandlePlayerDeath()
		{
            CCSimpleAudioEngine.SharedEngine.PlayEffect("Death");
			DestroyLevel ();
			// player died, so start the level over
			GoToLevel (LevelManager.Self.CurrentLevel);
		}
	}
}

