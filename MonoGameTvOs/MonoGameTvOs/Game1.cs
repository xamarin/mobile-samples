using System;
using Microsoft.Xna.Framework;

namespace MonoGameTvOs
{
	public class Game1 : Game
	{
		GraphicsDeviceManager graphics;

		public Game1 ()
		{
			graphics = new GraphicsDeviceManager (this);
			graphics.IsFullScreen = true;
		}

		protected override void Initialize ()
		{
			base.Initialize ();
		}

		protected override void Update (GameTime gameTime)
		{
			base.Update (gameTime);
		}

		protected override void Draw (GameTime gameTime)
		{
			graphics.GraphicsDevice.Clear (Color.CornflowerBlue);

			base.Draw (gameTime);
		}
	}
}

