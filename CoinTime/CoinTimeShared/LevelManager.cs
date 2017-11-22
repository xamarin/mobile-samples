using System;
using Microsoft.Xna.Framework;

namespace CoinTimeGame
{
	public class LevelManager
	{
		static Lazy<LevelManager> self = 
			new Lazy<LevelManager>(()=>new LevelManager());

		public int NumberOfLevels
		{
			get;
			private set;
		}

		public int CurrentLevel
		{
			get;
			set;
		}

		public static LevelManager Self
		{
			get
			{
				return self.Value;
			}
		}

		private LevelManager()
		{
			DetermineAvailableLevels ();


		}

		private void DetermineAvailableLevels()
		{
			// This game relies on levels being named "levelx.tmx" where x is an integer beginning with
			// 1. We have to rely on MonoGame's TitleContainer which doesn't give us a GetFiles method - we simply
			// have to check if a file exists, and if we get an exception on the call then we know the file doesn't
			// exist. 
			NumberOfLevels = 0;

			while (true)
			{
				bool fileExists = false;

				try
				{
					using(var stream = TitleContainer.OpenStream("Content/levels/level" + NumberOfLevels + ".tmx"))
					{

					}
					// if we got here then the file exists!
					fileExists = true;
				}
				catch
				{
					// do nothing, fileExists will remain false
				}

				if (!fileExists)
				{
					break;
				}
				else
				{
					NumberOfLevels++;
				}
			}
		}
	}
}

