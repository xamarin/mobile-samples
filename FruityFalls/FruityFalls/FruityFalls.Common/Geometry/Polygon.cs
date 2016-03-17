using System;
using CocosSharp;

namespace FruityFalls.Geometry
{
	public class Polygon : CCNode
	{
		CCPoint[] points;
		CCPoint[] absolutePoints;
		public CCPoint LastCollisionPoint;
		float boundingRadius;


		float rotation;
		// We have to "new" it because the base class has no support for this. Yuck...
		public new float Rotation
		{
			get
			{
				return rotation;
			}
			set
			{
				rotation = value;
				base.Rotation = rotation;
			}
		}

		public CCPoint[] Points
		{
			get
			{
				return points;
			}
			set
			{
				points = value;
				absolutePoints = new CCPoint[points.Length];
				ReactToPointsSet ();
			}
		}

		public Polygon ()
		{
		}

        public static Polygon CreateRectangle(float width, float height)
        {
            var polygon = new Polygon();

            var points = new CCPoint[] {
                new CCPoint(-width/2, -height/2),
                new CCPoint(-width/2, height/2),
                new CCPoint(width/2, height/2),
                new CCPoint(width/2, -height/2),
                new CCPoint(-width/2, -height/2)
            };

            polygon.Points = points;

            return polygon;
        }

		public bool CollideAgainst(Circle circle)
		{
			UpdateAbsolutePoints ();

			// This method will test the following things:
			//  * Is the circle's center inside the polygon?
			//  * Are any of the polygon's points inside the circle?
			//  * Is the circle within Radius distance from any of the polygon's edges?
			// Of course none of this is done if the radius check fails

			if ((circle.Radius + boundingRadius) * (circle.Radius + boundingRadius) >

				(circle.PositionX - PositionX) * (circle.Position.X - Position.X) +
				(circle.PositionY - PositionY) * (circle.Position.Y - Position.Y))
			{
				// First see if the circle is inside the polygon.
				if (IsPointInside(circle.PositionWorldspace.X, circle.PositionWorldspace.Y))
				{
					LastCollisionPoint.X = circle.Position.X;
					LastCollisionPoint.Y = circle.Position.Y;
					return true;
				}

				int i;
				// Next see if any of the Polygon's points are inside the circle
				for (i = 0; i < absolutePoints.Length; i++)
				{
					if (circle.IsPointInside(absolutePoints[i]))
					{
						LastCollisionPoint.X = absolutePoints[i].X;
						LastCollisionPoint.Y = absolutePoints[i].Y;
						return true;
					}
				}
				int k;

				Segment s1 = new Segment();
				Segment s2 = new Segment();

				// Next check if the circle is within Radius units of any segment.
				for (i = 0; i < absolutePoints.Length; i++)
				{
					k = i + 1 < absolutePoints.Length ? i + 1 : 0;
					s1.Point1 = absolutePoints [i];
					s1.Point2 = absolutePoints[k];

					var position = circle.PositionWorldspace;

					var segmentDistance = s1.DistanceTo (ref position, out s2);

					if (segmentDistance < circle.Radius)
					{
						LastCollisionPoint.X = s2.Point2.X;
						LastCollisionPoint.Y = s2.Point2.Y;

						return true;
					}
				}
			}
			return false;
		}

		private void ReactToPointsSet()
		{
			// calculate the boundingRadius which is used in collisions.
			float boundingRadiusSquared = 0;
			for (int i = 0; i < points.Length; i++)
			{
				var p = points[i];
				boundingRadiusSquared =
					System.Math.Max(boundingRadiusSquared, (float)(p.X * p.X + p.Y * p.Y));
			}

			boundingRadius = (float)System.Math.Sqrt(boundingRadiusSquared);
		}

		void UpdateAbsolutePoints()
		{
			var absolutePosition = this.PositionWorldspace;

			var rotationInClockwiseRadians = CCMathHelper.ToRadians (this.Rotation);

            // In this context, CocosSharp uses clockwise rotation, the opposite of mathematical rotation
            // So let's invert it so we get rotation in counterclockwise units:
            var rotationInRadians = -rotationInClockwiseRadians;

			CCPoint rotatedXAxis = new CCPoint((float) System.Math.Cos (rotationInRadians), (float) System.Math.Sin (rotationInRadians));
			CCPoint rotatedYAxis = new CCPoint (-rotatedXAxis.Y, rotatedXAxis.X);

			for (int i = 0; i < points.Length; i++)
			{
				
				absolutePoints [i] =  
					absolutePosition +
					(rotatedXAxis * points [i].X) +
					(rotatedYAxis * points [i].Y);


			}
		}

		public bool IsPointInside(float x, float y)
		{
			bool b = false;

			for (int i = 0, j = absolutePoints.Length - 1; i < absolutePoints.Length; j = i++)
			{
				if ((((absolutePoints[i].Y <= y) && (y < absolutePoints[j].Y)) || ((absolutePoints[j].Y <= y) && (y < absolutePoints[i].Y))) &&
					(x < (absolutePoints[j].X - absolutePoints[i].X) * (y - absolutePoints[i].Y) / (absolutePoints[j].Y - absolutePoints[i].Y) + absolutePoints[i].X)) b = !b;
			}

			return b;
		}

		public CCPoint GetNormalClosestTo(CCPoint point, out float distance)
		{
			// We need the point before to potentially calculate the normal
			int pointBefore;

			var fromCircleToThis = VectorFrom(point.X, point.Y, absolutePoints, out pointBefore);

			// The fromCircleToThis will be less than circle.Radius units in length.
			// However much less it is is how far the objects should be moved.

			distance = fromCircleToThis.Length;

			double amountToMoveOnX;
			double amountToMoveOnY;

			// If length is equal to 0, 
			// that means that the circle 
			// falls directly on the polygon's 
			// edge.  When this occurrs, the direction 
			// to move is unknown.  So we need to find the 
			// normal of the surface to know the direction to
			// move.  To get the normal we'll first look at the
			// point before on the polygon, and figure out the normal
			// from that
			if (distance == 0)
			{
				var direction = absolutePoints[pointBefore + 1] - absolutePoints[pointBefore];

				// now rotate it 90 degrees:
				direction = new CCPoint(-direction.Y, direction.X);

				// We need to move the point along the normal a little bit to see if the normal
				// is pointing inside or out. 
				if (!IsPointInside(point.X + direction.X * .01f, point.Y + direction.Y * .01f))
				{
					direction.X = -direction.X;
					direction.Y = -direction.Y;
				}

				direction.Normalize();


				fromCircleToThis.X = direction.X;
				fromCircleToThis.Y = direction.Y;
			}
			else
			{
				// normalize and invert:
				fromCircleToThis /= -fromCircleToThis.Length ;

				fromCircleToThis.Normalize ();
			}

			return fromCircleToThis;

		}

		public static CCPoint VectorFrom(float x, float y, CCPoint[] vertices, out int pointIndexBefore)
		{
			pointIndexBefore = -1;

			double minDistance = double.PositiveInfinity;
			double tempMinDistance;
			CCPoint vectorToReturn = new CCPoint();

			if (vertices.Length == 1)
			{
				return new CCPoint(
					vertices[0].X - x,
					vertices[0].Y - y);
			}

			Segment connectingSegment = new Segment();
			Segment segment = new Segment();

			for (int i = 0; i < vertices.Length - 1; i++)
			{
				int afterI = i + 1;
				segment.Point1 = new CCPoint(vertices[i].X, vertices[i].Y);
				segment.Point2 = new CCPoint(vertices[afterI].X, vertices[afterI].Y);

				var point = new CCPoint (x, y);
				tempMinDistance = segment.DistanceTo(ref point, out connectingSegment);
				if (tempMinDistance < minDistance)
				{
					pointIndexBefore = i;
					minDistance = tempMinDistance;
					vectorToReturn = new CCPoint(
						connectingSegment.Point2.X - connectingSegment.Point1.X,
						connectingSegment.Point2.Y - connectingSegment.Point1.Y);
				}
			}
			return vectorToReturn;
		}
	}
}

