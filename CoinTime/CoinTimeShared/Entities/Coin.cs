using System;
using CocosSharp;
using System.Collections.Generic;
using CoinTimeGame.ContentRuntime.Animations;

namespace CoinTimeGame.Entities
{
	public class Coin : AnimatedSpriteEntity
	{

		public Coin ()
		{
			LoadAnimations ("Content/animations/coinanimations.achx");

			CurrentAnimation = animations [0];
		}
	}
}

