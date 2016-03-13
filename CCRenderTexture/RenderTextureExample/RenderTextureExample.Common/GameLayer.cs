using System;
using System.Collections.Generic;
using CocosSharp;

namespace RenderTextureExample
{
	public class GameLayer : CCLayerColor
	{
		public GameLayer () : base (CCColor4B.Black)
		{

		}

		protected override void AddedToScene ()
		{
            base.AddedToScene();

            GameView.Stats.Enabled = true;

            const bool useRenderTextures = true;
            const byte opacity = 255;

            const float positionIncrement = 140;

            Card card = new Card();
            card.ColorIconTexture = CCTextureCache.SharedTextureCache.AddImage("GreenIcon.png");
            card.MonsterTexture = CCTextureCache.SharedTextureCache.AddImage("GreenGuy.png");
            card.DescriptionText = "Does extra damage to water units";
            card.MonsterName = "Grobot";
            card.PositionX = 20;
            card.PositionY = 100;
            card.UsesRenderTexture = useRenderTextures;
            card.Opacity = opacity;
            this.AddChild(card);

            card = new Card();
            card.ColorIconTexture = CCTextureCache.SharedTextureCache.AddImage("BlueIcon.png");
            card.MonsterTexture = CCTextureCache.SharedTextureCache.AddImage("BlueGuy.png");
            card.DescriptionText = "Takes reduced damage from earth attacks";
            card.MonsterName = "Bluchirp";
            card.PositionX = 20 + positionIncrement;
            card.PositionY = 100;
            card.UsesRenderTexture = useRenderTextures;
            card.Opacity = opacity;
            this.AddChild(card);

            card = new Card();
            card.ColorIconTexture = CCTextureCache.SharedTextureCache.AddImage("OrangeIcon.png");
            card.MonsterTexture = CCTextureCache.SharedTextureCache.AddImage("OrangeGuy.png");
            card.DescriptionText = "High chance of preemptive attack";
            card.MonsterName = "Orange-U-Glad";
            card.PositionX = 20 + positionIncrement * 2;
            card.PositionY = 100;
            card.UsesRenderTexture = useRenderTextures;
            card.Opacity = opacity;
            this.AddChild(card);

        }
	}
}
