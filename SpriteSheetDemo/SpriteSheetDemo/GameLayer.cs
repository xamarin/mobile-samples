using System;
using System.Collections.Generic;
using CocosSharp;

namespace SpriteSheetDemo
{
	public class GameLayer : CCLayerColor
	{
		public GameLayer () : base (CCColor4B.AliceBlue)
		{
			
		}

		protected override void AddedToScene ()
		{
			base.AddedToScene ();

			CreateLayout ();
		}

		void CreateLayout()
		{
			CCSpriteSheet sheet = new CCSpriteSheet ("sheet.plist", "sheet.png");
			CCSpriteFrame frame;

			// create the far background
			for (int i = 0; i < 3; i++)
			{
				frame = sheet.Frames.Find(item=>item.TextureFilename == "farBackground.png");
				CCSprite sprite = new CCSprite (frame);
				sprite.AnchorPoint = new CCPoint (0, 0);
				sprite.Scale = 3;
				sprite.PositionX = i * sprite.ScaledContentSize.Width;
				// offset it a bit upward to fill the empty space
				sprite.PositionY = 100;
				this.AddChild (sprite);
			}

			// create the forest background
			for (int i = 0; i < 3; i++)
			{
				frame = sheet.Frames.Find(item=>item.TextureFilename == "forestBackground.png");
				CCSprite sprite = new CCSprite (frame);
				sprite.AnchorPoint = new CCPoint (0, 0);
				sprite.Scale = 3;
				sprite.PositionX = i * sprite.ScaledContentSize.Width;
				this.AddChild (sprite);
			}


			// create the foreground:
			frame = sheet.Frames.Find(item=>item.TextureFilename == "foreground.png");
			CCSprite foreground = new CCSprite (frame);
			foreground.Scale = 3;
			foreground.AnchorPoint = new CCPoint (0, 0);
			this.AddChild (foreground);
		}
	}
}
