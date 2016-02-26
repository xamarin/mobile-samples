using System;
using CocosSharp;

namespace FruityFalls.Geometry
{
	public class Segment
	{
		public CCPoint Point1;
		public CCPoint Point2;

		public Segment ()
		{
		}

		public float DistanceTo(ref CCPoint vector, out Segment connectingSegment)
		{
			float segmentLength = (float)this.GetLength();

			CCPoint normalizedLine = new CCPoint(
				(float)(Point2.X - Point1.X) / segmentLength,
				(float)(Point2.Y - Point1.Y) / segmentLength);

			CCPoint pointVector = new CCPoint((float)(vector.X - Point1.X), (float)(vector.Y - Point1.Y));

			float length = CCPoint.Dot(pointVector, normalizedLine);
			connectingSegment = new Segment ();
			if (length < 0)
			{
				connectingSegment.Point1 = vector;
				connectingSegment.Point2 = Point1;

				return (float) connectingSegment.GetLength();
			}
			else if (length > segmentLength)
			{
				connectingSegment.Point1 = vector;
				connectingSegment.Point2 = Point2;

				return (float) connectingSegment.GetLength();
			}
			else
			{
				connectingSegment.Point1 = vector;
				connectingSegment.Point2 = new CCPoint(Point1.X + length * normalizedLine.X,
					Point1.Y + length * normalizedLine.Y);

				return (float)connectingSegment.GetLength();
			}
		}

		public double GetLength()
		{
			return System.Math.Sqrt((Point2.X - Point1.X) * (Point2.X - Point1.X) + (Point2.Y - Point1.Y) * (Point2.Y - Point1.Y));
		}
	}
}

