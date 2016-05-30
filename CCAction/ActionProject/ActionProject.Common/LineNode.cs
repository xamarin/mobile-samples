using System;
using CocosSharp;

namespace ActionProject
{
	public class LineNode : CCNode
	{
		readonly CCDrawNode line;
		float width;

		public float Width {
			get {
				return width;
			}
			set {
				width = value;

				UpdateLine ();
			}
		}

		public LineNode ()
		{
			line = new CCDrawNode ();
			Width = 10;
			AddChild (line);
		}

		void UpdateLine ()
		{
			line.Clear ();

			var effectiveWidth = Math.Max (0, width);
			line.DrawLine (CCPoint.Zero, new CCPoint (40, 0), effectiveWidth / 2, new CCColor4F (1, 1, 0, 1));
		}
	}
}

