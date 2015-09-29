using System;
using CocosSharp;

namespace ActionProject
{
	public class LineNode : CCNode
	{
		CCDrawNode line;

		float width;

		public float Width
		{
			get
			{
				return width;
			}
			set
			{
				width = value;

				UpdateLine ();
			}
		}

		public LineNode ()
		{
			line = new CCDrawNode ();
			Width = 10;
			this.AddChild (line);
		}

		private void UpdateLine()
		{
			line.Clear ();

			var effectiveWidth = System.Math.Max (0, width);

			line.DrawSegment (CCPoint.Zero, new CCPoint (40, 0), effectiveWidth/2, 
				new CCColor4F (1, 1, 0, 1));
			
		}
	}
}

