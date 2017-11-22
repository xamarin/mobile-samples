using System;
using CocosSharp;
using FruityFalls.Geometry;

namespace FruityFalls.Entities
{
	public class Fruit : CCNode
	{
		CCSprite graphic;
		CCDrawNode debugGrahic;
        CCLabel extraPointsLabel;

		public CCPoint Velocity;
		public CCPoint Acceleration;

        // This prevents the user from holding the
        // paddle under a fruit, earning extra points every frame
        float timeUntilExtraPointsCanBeAdded;

        int extraPointValueButPleaseUseProperty;
        public int ExtraPointValue
        {
            get
            {
                return extraPointValueButPleaseUseProperty;
            }
            private set
            {
                extraPointValueButPleaseUseProperty = value;
                timeUntilExtraPointsCanBeAdded = GameCoefficients.MinPointAwardingFrequency;
                extraPointsLabel.Text = "+" + (extraPointValueButPleaseUseProperty + 1);
            }
        }

        // Putting this here just for convenience
        public float Radius
        {
            get
            {
                return GameCoefficients.FruitRadius;
            }
        }

		public Circle Collision
		{
			get;
			private set;
		}

        FruitColor fruitColor;
        public FruitColor FruitColor
        {
            get
            {
                return fruitColor;
            }
            set
            {
                fruitColor = value;
                UpdateGraphics();
            }
        }

        public Fruit ()
        {
	        CreateFruitGraphic ();

	        if (GameCoefficients.ShowCollisionAreas)
	        {
		        CreateDebugGraphic ();
	        }

	        CreateCollision ();

	        CreateExtraPointsLabel ();

	        Acceleration.Y = GameCoefficients.FruitGravity;
        }

		private void CreateFruitGraphic()
		{
			graphic = new CCSprite ("cherry.png");
            graphic.IsAntialiased = false;
			this.AddChild (graphic);
		}

		private void CreateDebugGraphic()
		{
			debugGrahic = new CCDrawNode ();
			this.AddChild (debugGrahic);
		}

		private void CreateCollision()
		{
			Collision = new Circle ();
			Collision.Radius = GameCoefficients.FruitRadius;
			this.AddChild (Collision);
		}

		private void CreateExtraPointsLabel()
		{
			extraPointsLabel = new CCLabel("", "Arial", 12, CCLabelFormat.SystemFont);
			extraPointsLabel.IsAntialiased = false;
			extraPointsLabel.Color = CCColor3B.Black;
			this.AddChild(extraPointsLabel);
		}

        private void UpdateGraphics()
        {
			if (GameCoefficients.ShowCollisionAreas)
			{
				debugGrahic.Clear ();
				const float borderWidth = 4;
				debugGrahic.DrawSolidCircle (
					CCPoint.Zero,
					GameCoefficients.FruitRadius,
					CCColor4B.Black);
				debugGrahic.DrawSolidCircle (
					CCPoint.Zero,
					GameCoefficients.FruitRadius - borderWidth,
					fruitColor.ToCCColor ());
			}
			if (this.FruitColor == FruitColor.Yellow)
			{
				graphic.Texture = CCTextureCache.SharedTextureCache.AddImage ("lemon.png");
				extraPointsLabel.Color = CCColor3B.Black;
				extraPointsLabel.PositionY = 0;

			}
			else
			{
				graphic.Texture = CCTextureCache.SharedTextureCache.AddImage ("cherry.png");
				extraPointsLabel.Color = CCColor3B.White;
				extraPointsLabel.PositionY = -8;
			}
        }

        public void Activity(float frameTimeInSeconds)
		{
            timeUntilExtraPointsCanBeAdded -= frameTimeInSeconds;

			// linear approximation:
			this.Velocity += Acceleration * frameTimeInSeconds;

            // This is a linera approximation to drag. It's used to
            // keep the object from falling too fast, and eventually
            // to slow its horizontal movement. This makes the game easier
            this.Velocity -= Velocity * GameCoefficients.FruitDrag * frameTimeInSeconds;

			this.Position += Velocity * frameTimeInSeconds;

		}

        public bool TryAddExtraPointValue()
        {
            bool didAddPoint = false;
            if(timeUntilExtraPointsCanBeAdded <= 0)
            {
                ExtraPointValue++;
                didAddPoint = true;
            }

            return didAddPoint;
        }
	}
}

