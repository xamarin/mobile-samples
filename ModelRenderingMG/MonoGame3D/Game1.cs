#region Using Statements
using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;

#endregion

namespace MonoGame3D
{
	public class Game1 : Game
	{
		GraphicsDeviceManager graphics;

		// This is the model instance that we'll load
		// our XNB into:
		Model model;

		public Game1 ()
		{
			graphics = new GraphicsDeviceManager (this);
			graphics.IsFullScreen = true;
			                    
			Content.RootDirectory = "Content";
		}

		protected override void LoadContent ()
		{
			// Notice that loading a model is very similar
			// to loading any other XNB (like a Texture2D).
			// The only difference is the generic type.
			model = Content.Load<Model> ("robot");
		}

		protected override void Update (GameTime gameTime)
		{
			base.Update (gameTime);
		}

		protected override void Draw (GameTime gameTime)
		{
			GraphicsDevice.Clear (Color.CornflowerBlue);

			// A model is composed of "Meshes" which are
			// parts of the model which can be positioned
			// independently, which can use different textures,
			// and which can have different rendering states
			// such as lighting applied.
			foreach (var mesh in model.Meshes)
			{
				// "Effect" refers to a shader. Each mesh may
				// have multiple shaders applied to it for more
				// advanced visuals. 
				foreach (BasicEffect effect in mesh.Effects)
				{
					// We could set up custom lights, but this
					// is the quickest way to get somethign on screen:
					effect.EnableDefaultLighting ();
					// This makes lighting look more realistic on
					// round surfaces, but at a slight performance cost:
					effect.PreferPerPixelLighting = true;

					// The world matrix can be used to position, rotate
					// or resize (scale) the model. Identity means that
					// the model is unrotated, drawn at the origin, and
					// its size is unchanged from the loaded content file.
					effect.World = Matrix.Identity;

					// Move the camera 8 units away from the origin:
					var cameraPosition = new Vector3 (0, 8, 0);
					// Tell the camera to look at the origin:
					var cameraLookAtVector = Vector3.Zero;
					// Tell the camera that positive Z is up
					var cameraUpVector = Vector3.UnitZ;

					effect.View = Matrix.CreateLookAt (
						cameraPosition, cameraLookAtVector, cameraUpVector);

					// We want the aspect ratio of our display to match
					// the entire screen's aspect ratio:
					float aspectRatio = 
						graphics.PreferredBackBufferWidth / (float)graphics.PreferredBackBufferHeight;
					// Field of view measures how wide of a view our camera has.
					// Increasing this value means it has a wider view, making everything
					// on screen smaller. This is conceptually the same as "zooming out".
					// It also 
					float fieldOfView = Microsoft.Xna.Framework.MathHelper.PiOver4;
					// Anything closer than this will not be drawn (will be clipped)
					float nearClipPlane = 1;
					// Anything further than this will not be drawn (will be clipped)
					float farClipPlane = 200;

					effect.Projection = Matrix.CreatePerspectiveFieldOfView (
						fieldOfView, aspectRatio, nearClipPlane, farClipPlane);

				}

				// Now that we've assigned our properties on the effects we can
				// draw the entire mesh
				mesh.Draw ();
			}
			base.Draw (gameTime);
		}
	}
}

