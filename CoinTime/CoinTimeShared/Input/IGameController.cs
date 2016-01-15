using System;

namespace CoinTimeShared
{
	public interface IGameController
	{
		float HorizontalRatio { get; }

		bool JumpPressed { get; }

		bool BackPressed { get; }

		void UpdateInputValues();
	}
}

