using System;
using System.Collections.Generic;
using CocosSharp;

namespace CustomRendering
{
	public class GameLayer : CCLayer
	{
		CCDrawNode drawNode;

		public GameLayer ()
		{
			drawNode = new CCDrawNode ();
			this.AddChild (drawNode);

			drawNode.PositionX = 100;
			drawNode.PositionY = 100;

			// Uncomment one of the following to see 
			// an example of a given Draw call:

			CardinalSpline ();

//			CatmullRom ();

//			Circle ();

//			CubicBezier ();

//			Ellipse ();

//			Line ();

//			LineList ();

//			Polygon ();

//			QuadBezier ();

//			Rect ();

//			Segment ();

//			SolidArc ();

//			SolidCircle ();

//			TriangleList ();
		}




		void CardinalSpline ()
		{
			var splinePoints = new List<CCPoint> ();
			splinePoints.Add (new CCPoint (0, 0));
			splinePoints.Add (new CCPoint (50, 70));
			splinePoints.Add (new CCPoint (0, 140));
			splinePoints.Add (new CCPoint (100, 210));
			drawNode.DrawCardinalSpline (
				config: splinePoints,
				tension: 0,
				segments: 64,
				color: CCColor4B.Red);
		}

		void CatmullRom ()
		{
			var splinePoints = new List<CCPoint> ();
			splinePoints.Add (new CCPoint (0, 0));
			splinePoints.Add (new CCPoint (80, 90));
			splinePoints.Add (new CCPoint (100, 0));
			splinePoints.Add (new CCPoint (0, 130));

			drawNode.DrawCatmullRom (
				points: splinePoints,
				segments: 64,
				color: CCColor4B.Green);
			
		}

		void Circle ()
		{
			drawNode.DrawCircle (
				center: new CCPoint (0, 0),
				radius: 20,
				color: CCColor4B.Yellow);
		}

		void CubicBezier ()
		{
// fantastic vid:
// https://vimeo.com/106757336
			drawNode.DrawCubicBezier (
				origin: new CCPoint (0, 0),
				control1: new CCPoint (50, 150),
				control2: new CCPoint (250, 150),
				destination: new CCPoint (170, 0),
				segments: 64,
				lineWidth: 1,
				color: CCColor4B.Green);
		}

		void Ellipse ()
		{
			drawNode.DrawEllipse (
				rect: new CCRect (0, 0, 130, 90),
				lineWidth: 2,
				color: CCColor4B.Gray);
		}

		void Line ()
		{
			drawNode.DrawLine (
				from: new CCPoint (0, 0),
				to: new CCPoint (150, 30),
				lineWidth: 5,
				color: CCColor4B.Orange);
		}

		void LineList ()
		{
			CCV3F_C4B[] verts = new CCV3F_C4B[] {
				// First line:
				new CCV3F_C4B (new CCPoint (0, 0), CCColor4B.White),
				new CCV3F_C4B (new CCPoint (30, 60), CCColor4B.White),
				// second line, will blend from white to red:
				new CCV3F_C4B (new CCPoint (60, 0), CCColor4B.White),
				new CCV3F_C4B (new CCPoint (120, 120), CCColor4B.Red)
			};

			drawNode.DrawLineList (verts);
		}

		void Polygon ()
		{
			CCPoint[] verts = new CCPoint[] {
				new CCPoint (0, 0),
				new CCPoint (0, 100),
				new CCPoint (50, 150),
				new CCPoint (100, 100),
				new CCPoint (100, 0)
			};

			drawNode.DrawPolygon (verts,
				count: verts.Length,
				fillColor: CCColor4B.White,
				borderWidth: 5,
				borderColor: CCColor4B.Red,
				closePolygon: true);

		}

		void QuadBezier ()
		{
			drawNode.DrawQuadBezier (
				origin: new CCPoint (0, 0),
				control: new CCPoint (200, 0),
				destination: new CCPoint (0, 300),
				segments: 64,
				lineWidth: 1,
				color: CCColor4B.White);
		}

		void Rect ()
		{
			var shape = new CCRect (
				         0, 0, 100, 200);
			drawNode.DrawRect (shape,
				fillColor: CCColor4B.Blue,
				borderWidth: 4,
				borderColor: CCColor4B.White);
		}

		void Segment ()
		{
			drawNode.DrawSegment (from: new CCPoint (0, 0),
				to: new CCPoint (100, 200),
				radius: 5,
				color: new CCColor4F (1, 1, 1, 1));
		}

		void SolidArc ()
		{
			drawNode.DrawSolidArc (
				pos: new CCPoint (100, 100),
				radius: 200,
				startAngle: 0,
				sweepAngle: CCMathHelper.Pi / 2, // this is in radians, clockwise
				color: CCColor4B.White);
		}

		void SolidCircle ()
		{
			drawNode.DrawSolidCircle (
				pos: new CCPoint (100, 100),
				radius: 50,
				color: CCColor4B.Yellow);
		}

		void TriangleList ()
		{
			CCV3F_C4B[] verts = new CCV3F_C4B[] {
				// First triangle:
				new CCV3F_C4B (new CCPoint (0, 0), CCColor4B.White),
				new CCV3F_C4B (new CCPoint (30, 60), CCColor4B.White),
				new CCV3F_C4B (new CCPoint (60, 0), CCColor4B.White),
				// second triangle, each point has different colors:
				new CCV3F_C4B (new CCPoint (90, 0), CCColor4B.Yellow),
				new CCV3F_C4B (new CCPoint (120, 60), CCColor4B.Red),
				new CCV3F_C4B (new CCPoint (150, 0), CCColor4B.Blue)
			};

			drawNode.DrawTriangleList (verts);

		}

	}
}
