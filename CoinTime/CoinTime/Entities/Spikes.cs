using System;

namespace CoinTimeGame.Entities
{
	public class Spikes : AnimatedSpriteEntity, IDamageDealer
	{
		public Spikes ()
		{
			LoadAnimations ("Content/animations/propanimations.achx");

			CurrentAnimation = animations [0];
		}
	}
}

