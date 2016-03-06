using System;
using CocosSharp;

namespace FruityFalls.Entities
{
	public class ScoreText : CCNode
	{
		CCLabel label;

		CCDrawNode background;

        public int Score
        {
            set
            {
                label.Text = "Score: " + value;
            }
        }

		public ScoreText ()
		{
			background = new CCDrawNode ();
			
			const int width = 115;
			const int height = 27;
			
			background.DrawRect (new CCRect (-5, -height ,
				width, height),
				new CCColor4B (100, 100, 100));
			
			this.AddChild (background);


			label = new CCLabel ("Score: 9999", "Arial", 20, CCLabelFormat.SystemFont);
            label.AnchorPoint = new CCPoint(0, 1);
			this.AddChild (label);
		}
	}
}

