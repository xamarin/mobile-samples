using System;
using CoinTimeGame.Input;

namespace CoinTimeGame.Scenes
{
	public partial class GameScene
	{
		partial void PlatformInit()
		{
			this.controller = new AmazonFireGameController();
		}
	}
}

