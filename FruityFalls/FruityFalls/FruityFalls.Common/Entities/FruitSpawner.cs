using CocosSharp;
using System;
using System.Collections.Generic;

namespace FruityFalls.Entities
{
	public class FruitSpawner
	{
		float timeSinceLastSpawn;
		public float TimeInbetweenSpawns
		{
			get;
			set;
		}

        public string DebugInfo
        {
            get
            {
                string toReturn =
					"Fruit per second: " + (1 / TimeInbetweenSpawns);

                return toReturn;
            }
        }

        public CCLayer Layer
        {
            get;
            set;
        }

		public Action<Fruit> FruitSpawned;

        public bool IsSpawning
        {
            get;
            set;
        }

		public FruitSpawner ()
		{
            IsSpawning = true;
            TimeInbetweenSpawns = 1 / GameCoefficients.StartingFruitPerSecond;
            // So that spawning starts immediately:
            timeSinceLastSpawn = TimeInbetweenSpawns;
		}

		public void Activity(float frameTime)
		{
            if (IsSpawning)
            {
                SpawningActivity(frameTime);

                SpawnReductionTimeActivity(frameTime);
            }
		}

        private void SpawningActivity(float frameTime)
        {
            timeSinceLastSpawn += frameTime;

            if (timeSinceLastSpawn > TimeInbetweenSpawns)
            {
                timeSinceLastSpawn -= TimeInbetweenSpawns;

                Spawn();
            }
        }

        private void SpawnReductionTimeActivity(float frameTime)
        {
            // This logic should increase how frequently fruit appear, but it should do so
            // such that the # of fruit/minute increases at a decreasing rate, otherwise the
            // game becomes impossibly difficult very quickly.
            var currentFruitPerSecond = 1 / TimeInbetweenSpawns;

            var amountToAdd = frameTime / GameCoefficients.TimeForExtraFruitPerSecond;

            var newFruitPerSecond = currentFruitPerSecond + amountToAdd;

            TimeInbetweenSpawns = 1 / newFruitPerSecond;

        }

        // made public for debugging, may make it private later:
        private void Spawn()
		{
			var fruit = new Fruit ();

            if(Layer == null)
            {
                throw new InvalidOperationException("Need to set Layer before spawning");
            }

            fruit.PositionX = CCRandom.GetRandomFloat(0 + fruit.Radius, Layer.ContentSize.Width - fruit.Radius);
            fruit.PositionY = Layer.ContentSize.Height + fruit.Radius;

            if(CCRandom.Float_0_1() > .5f)
            {
                fruit.FruitColor = FruitColor.Red;
            }
            else
            {
                fruit.FruitColor = FruitColor.Yellow;
            }


			if(FruitSpawned != null)
			{
				FruitSpawned(fruit);
			}

		}



	}
}

