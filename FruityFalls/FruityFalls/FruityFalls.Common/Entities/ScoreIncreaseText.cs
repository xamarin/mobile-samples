using System;
using CocosSharp;

namespace FruityFalls.Entities
{
	public class ScoreIncreaseText : CCNode
	{
		CCLabel label;

		public ScoreIncreaseText ()
		{
			label = new CCLabel ("+1", "Arial", 20, CCLabelFormat.SystemFont);

			this.AddChild (label);
		}
	}
}

