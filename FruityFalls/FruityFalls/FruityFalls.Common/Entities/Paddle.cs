using System;
using CocosSharp;
using FruityFalls.Geometry;

namespace FruityFalls.Entities
{
	public class Paddle : CCNode
	{
		const float width = 118;
		const float height = 18;
        const float speedAtMaxRotation = 400;
        const float maxRotation = 50; // in degrees

        CCPoint desiredLocation;
		public CCPoint Velocity;

		CCSprite graphic;

		CCDrawNode debugGraphic;


		public Polygon Polygon
		{
			get;
			private set;
		}

		float rotation;
		public new float Rotation
		{
			get
			{
				return rotation;
			}
			set
			{
				rotation = value;
				Polygon.Rotation = rotation;
				base.Rotation = rotation;
			}
		}

		Vine leftVine;
		Vine rightVine;

		public Paddle ()
		{
			
			CreateVines ();

			CreateSpriteGraphic ();

			if (GameCoefficients.ShowCollisionAreas)
			{
				CreateDebugGraphic ();
			}


			CreateCollision ();
		}

		private void CreateSpriteGraphic ()
		{
			graphic = new CCSprite ("monkey.png");
			graphic.IsAntialiased = false;
			// offset it so the monkey's paddle lines up with the collision:
			graphic.PositionY = -52;
			this.AddChild (graphic);
		}

		private void CreateDebugGraphic()
		{
			debugGraphic = new CCDrawNode ();

			debugGraphic.DrawRect (
				new CCRect (-width/2, -height/2, width, height),
				fillColor: new CCColor4B(180, 180, 180, 180));

			this.AddChild (debugGraphic);
		}

		private void CreateCollision()
		{
            Polygon = Polygon.CreateRectangle(width, height);

			this.AddChild (Polygon);
		}

		private void CreateVines()
		{
			const int vineOffset = 8;

			leftVine = new Vine ();
			leftVine.PositionX = -width / 2.0f + vineOffset;
			this.AddChild (leftVine);

			rightVine = new Vine ();
			rightVine.PositionX = width / 2.0f - vineOffset;
			this.AddChild (rightVine);
		}

		public void HandleInput(CCPoint touchPoint)
		{
			desiredLocation = touchPoint;
		}

		public void Activity(float frameTimeinSeconds)
		{
			// This code will cause the cursor to lag behind the player's touch 
			// a bit. The higher this value, the tighter the cursor will follow
			const float velocityCoefficient = 4;

			// Get the velocity from current location and touch location
			Velocity = (desiredLocation - this.Position) * velocityCoefficient;


			this.Position += Velocity * frameTimeinSeconds;

			float ratio = Velocity.X / speedAtMaxRotation;
			if (ratio > 1)	ratio = 1;
			if (ratio < -1)	ratio = -1;

			this.Rotation = ratio * maxRotation;

			// We want the vine to only rotate a small amount, it is a cooler
			// effect compared to if it rotates along with the paddle
			float vineAngle = this.Velocity.X / 100.0f;

			leftVine.Rotation = -this.Rotation + vineAngle;
			rightVine.Rotation = -this.Rotation + vineAngle;
		}

        internal void SetDesiredPositionToCurrentPosition()
        {
            desiredLocation.X = this.PositionX;
            desiredLocation.Y = this.PositionY;
        }
    }
}

