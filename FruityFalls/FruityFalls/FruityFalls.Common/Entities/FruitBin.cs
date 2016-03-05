using System;
using CocosSharp;
using FruityFalls.Geometry;

namespace FruityFalls.Entities
{
    public enum FruitColor
    {
        Yellow,
        Red
    }

    public static class FruitColorExtensions
    {


        public static CCColor4B ToCCColor(this FruitColor color)
        {
            switch(color)
            {
                case FruitColor.Yellow:
					return new CCColor4B(150, 150, 0, 150);
                case FruitColor.Red:
					return new CCColor4B(150,0,0,150);
            }
            throw new ArgumentException("Unknown color " + color);
        }

    }

	public class FruitBin : CCNode
	{
		float width = 200;
        const float collisionHeight = 100;
		CCDrawNode graphic;

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
				UpdateGraphics ();
			}
		}

        public Polygon Polygon
        {
            get;
            private set;
        }

		public float Width
		{
			get
			{
				return width;
			}
			set
			{
				width = value;
				UpdateGraphics ();

                // being lazy here:
                RemoveChild(Polygon);
                Polygon = Polygon.CreateRectangle(width, collisionHeight);
                // bin graphics are bottom-left aligned, so let's do the same with the polygon.
                Polygon.PositionX = width / 2.0f;
                Polygon.PositionY = GameCoefficients.BinHeight - (collisionHeight/2.0f);
                AddChild(Polygon);
            }
        }

		public FruitBin ()
		{
			graphic = new CCDrawNode ();
			this.AddChild (graphic);
		}

		private void UpdateGraphics()
		{
			if(GameCoefficients.ShowCollisionAreas)
			{
			graphic.Clear ();

            graphic.DrawRect (
            	new CCRect (0, GameCoefficients.BinHeight - collisionHeight, width, collisionHeight),
            	fillColor: fruitColor.ToCCColor());
			}
        }
    }
}

