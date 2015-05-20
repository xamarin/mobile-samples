using System;
using System.Linq;

namespace CoinTimeGame.Entities
{
	public class Door : AnimatedSpriteEntity
	{
		bool isOpen = false;

		public bool IsOpen
		{
			get
			{
				return isOpen;
			}
			set
			{
				isOpen = value;
				UpdateToIsOpen ();
			}
		}


		public Door ()
		{
			LoadAnimations ("Content/animations/dooranimations.achx");

			CurrentAnimation = animations [0];
		}

		void UpdateToIsOpen ()
		{
			if (isOpen)
			{
				IsLoopingAnimation = false;
				CurrentAnimation = animations.Find (item => item.Name == "Open");
			}
			else
			{
				CurrentAnimation = animations.Find (item => item.Name == "Closed");
			}
		}

	}
}

