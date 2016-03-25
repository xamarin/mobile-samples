using System;
using CocosSharp;
using FruityFalls.Geometry;

namespace FruityFalls.Entities
{
	public class Paddle : CCNode
	{
		const float width = 59;
		const float height = 9;
        const float speedAtMaxRotation = 200;
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
			graphic.PositionY = -24;
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
            // Increasing this value moves the vines closer to the 
            // center of the paddle.
			const int vineDistanceFromEdge = 4;

			leftVine = new Vine ();
            var leftEdge = -width / 2.0f;
            leftVine.PositionX = leftEdge + vineDistanceFromEdge;
			this.AddChild (leftVine);

			rightVine = new Vine ();
            var rightEdge = width / 2.0f;
            rightVine.PositionX = rightEdge - vineDistanceFromEdge;
			this.AddChild (rightVine);
		}

		public void HandleInput(CCPoint touchPoint)
		{
			desiredLocation = touchPoint;
		}

		public void Activity(float frameTimeInSeconds)
		{
            // This code will cause the cursor to lag behind the touch point.
            // Increasing this value reduces how far the paddle lags
            // behind the player's finger.
			const float velocityCoefficient = 4;

			// Get the velocity from current location and touch location
			Velocity = (desiredLocation - this.Position) * velocityCoefficient;

			this.Position += Velocity * frameTimeInSeconds;

			float ratio = Velocity.X / speedAtMaxRotation;
			if (ratio > 1)	ratio = 1;
			if (ratio < -1)	ratio = -1;

			this.Rotation = ratio * maxRotation;

			// This value adds a slight amount of rotation
            // to the vine. Having a small amount of tilt looks nice.
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

