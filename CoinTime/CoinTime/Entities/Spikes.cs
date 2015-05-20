using System;

namespace CoinTimeGame.Entities
{
	public class Spikes : AnimatedSpriteEntity, IDamageDealer
	{
		public Spikes ()
		{
			LoadAnimations ("Content/animations/props.achx");

			CurrentAnimation = animations [0];
		}
	}
}

