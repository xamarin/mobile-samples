using System;
using CocosSharp;
using CoinTimeGame.Entities;
using System.Collections.Generic;
using CoinTimeGame.TilemapClasses;

namespace CoinTimeGame.Scenes
{
	public partial class GameScene : CCScene
	{
		CCLayer gameplayLayer;
		CCLayer hudLayer;

		CCTileMap currentLevel;
		LevelCollision levelCollision;
		CCTileMapLayer backgroundLayer;
			
		TouchScreenInput input;

		float secondsLeft;

		Player player;
		Door door;
		Timer timer;

		List<IDamageDealer> damageDealers = new List<IDamageDealer>();
		List<Enemy> enemies = new List<Enemy>();
		List<Coin> coins = new List<Coin>();

		public static int LevelIndex
		{
			get;
			set;
		}


		public GameScene (CCWindow mainWindow) : base(mainWindow)
		{
			CreateLayers ();

			CreateHud ();

			GoToLevel (LevelIndex);

			Schedule(PerformActivity);
		}

		private void CreateHud()
		{
			timer = new Timer ();
			timer.PositionX = this.ContentSize.Center.X;
			timer.PositionY = this.ContentSize.Height - 20;


			hudLayer.AddChild (timer);
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
			// To discuss: Scrolling causes the tile map to be culled. Why? Shouldn't culling be done using world coordinates?

			float effectivePlayerX = player.PositionX;
			// Prevents the camera from scrolling beyond the left level's edge.
			effectivePlayerX = System.Math.Max (player.PositionX, this.ContentSize.Center.X);

			float effectivePlayerY = player.PositionY;
			// prevents the camera from scrolling below the level's edge
			effectivePlayerY = System.Math.Max(player.Position.Y, this.ContentSize.Center.Y);

			float positionX = -effectivePlayerX + this.ContentSize.Center.X;
			float positionY = -effectivePlayerY + this.ContentSize.Center.Y;

			gameplayLayer.PositionX = positionX;
			gameplayLayer.PositionY = positionY;

			// background will not scroll, so we'll make it move the opposite direction of the rest of the tilemap:
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
			LoadLevel (levelNumber);

			ProcessTileProperties ();

			secondsLeft = 60;
		}

		private void LoadLevel(int levelNumber)
		{
			currentLevel = new CCTileMap ("level" + levelNumber + ".tmx");
			currentLevel.Antialiased = false;
			backgroundLayer = currentLevel.LayerNamed ("Background");

			// CCTileMap is a CCLayer, so we'll just add it under all entities (for now)
			// To discuss:
			// Why doesent this.Children.Add work but this.AddChild does?
			// Added issue here:
			// https://github.com/mono/CocosSharp/issues/212
			this.AddChild (currentLevel);

			levelCollision = new LevelCollision ();
			levelCollision.PopulateFrom (currentLevel);

			// To discuss:
			// I couldn't get this to work:
//			this.ReorderChild (levelCollision, 1);

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

			input = new TouchScreenInput(gameplayLayer);
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
            input.UpdateInputValues();

            player.ApplyInput(input.HorizontalRatio, input.WasJumpPressed);
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

			input.Dispose ();


			this.RemoveChild (currentLevel);
			currentLevel.Dispose ();

			// don't think we need to do anything with LevelCollision

		}

		private void HandlePlayerDeath()
		{
			DestroyLevel ();
			// player died, so start the level over
			GoToLevel (LevelIndex);
		}
	}
}

