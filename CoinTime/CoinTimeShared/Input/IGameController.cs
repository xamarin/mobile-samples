using System;

namespace CoinTimeShared
{
	public interface IGameController
	{
		bool IsConnected { get; }

		float HorizontalRatio { get; }

		bool JumpPressed { get; }

		bool BackPressed { get; }

		void UpdateInputValues();
	}
}

