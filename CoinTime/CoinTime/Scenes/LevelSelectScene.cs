using System;
using CoinTimeGame.Input;

namespace CoinTimeGame.Scenes
{
	public partial class LevelSelectScene
	{
		partial void PlatformInit()
		{
			this.menuController = new AmazonFireGameController();
		}
	}
}

