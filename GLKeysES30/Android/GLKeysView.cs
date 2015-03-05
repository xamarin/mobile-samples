using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.ES30;
using OpenTK.Platform.Android;
using Android.Graphics;
using Android.Content;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace GLKeysES30
{
	class GLKeysView : AndroidGameView
	{
		ES30Keys keys;

		public GLKeysView (Context context, IAttributeSet attrs) :
		base (context, attrs)
		{
			Initialize ();
		}

		public GLKeysView (IntPtr handle, Android.Runtime.JniHandleOwnership transfer)
			: base (handle, transfer)
		{
			Initialize ();
		}

		void Initialize ()
		{
			keys = new ES30Keys ();
			AutoSetContextOnRenderFrame = false;
			RenderOnUIThread = false;
		}

		protected override void OnContextSet (EventArgs e)
		{
			base.OnContextSet (e);
			Console.WriteLine ("OpenGL version: {0} GLSL version: {1}", GL.GetString (StringName.Version), GL.GetString (StringName.ShadingLanguageVersion));
			keys.LoadShaders ();
			keys.CreateTextTextures (CreateBitmapData);
			keys.InitModel ();
			keys.Start ();
		}

		protected override void OnRenderThreadExited (EventArgs e)
		{
			base.OnRenderThreadExited (e);

			global::Android.App.Application.SynchronizationContext.Send (_ => {
				Console.WriteLine ("render thread exited\nexception:\n{0}", RenderThreadException);
				TextView view = ((LinearLayout) Parent).FindViewById (Resource.Id.TextNotSupported) as TextView;
				view.LayoutParameters = new LinearLayout.LayoutParams (LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.MatchParent);
				view.Visibility = ViewStates.Visible;
				Parent.RequestLayout ();
			}, null);
		}

		protected override void OnLoad (EventArgs e)
		{
			// Run the render loop
			Run (60);
		}

		protected override void OnResize (EventArgs e)
		{
			base.OnResize (e);
			SetupProjection ();
			//keys.RenderFrame ();
		}

		void CreateBitmapData (string str, out byte[] bitmapData, out int width, out int height)
		{
			Paint paint = new Paint ();
			paint.TextSize = 128;
			paint.TextAlign = Paint.Align.Left;
			paint.SetTypeface (Typeface.Default);
			width = height = 256;
			float textWidth = paint.MeasureText (str);

			using (Bitmap bitmap = Bitmap.CreateBitmap (width, height, Bitmap.Config.Argb8888)) {
				Canvas canvas = new Canvas (bitmap);
				paint.Color = str != " " ? Color.White : Color.LightGray;
				canvas.DrawRect (new Rect (0, 0, width, height), paint);
				paint.Color = Color.Black;
				canvas.DrawText (str, (256 - textWidth) / 2f, (256 - paint.Descent () - paint.Ascent ()) / 2f, paint);
				bitmapData = new byte [width * height * 4];
				Java.Nio.ByteBuffer buffer = Java.Nio.ByteBuffer.Allocate (bitmapData.Length);
				bitmap.CopyPixelsToBuffer (buffer);
				buffer.Rewind ();
				buffer.Get (bitmapData, 0, bitmapData.Length);
			}
		}

		// This method is called everytime the context needs
		// to be recreated. Use it to set any egl-specific settings
		// prior to context creation
		//
		// In this particular case, we demonstrate how to set
		// the graphics mode and fallback in case the device doesn't
		// support the defaults
		protected override void CreateFrameBuffer ()
		{
			ContextRenderingApi = GLVersion.ES3;
			// the default GraphicsMode that is set consists of (16, 16, 0, 0, 2, false)
			try {
				Log.Verbose ("GLCube", "Loading with default settings");

				// use multisample if possible
				GraphicsMode = new AndroidGraphicsMode (new ColorFormat (32), 16, 0, 4, 2, false); 
				// if you don't call this, the context won't be created
				base.CreateFrameBuffer ();
			} catch (Exception ex) {
				Log.Verbose ("GLCube", "{0}", ex);
				// this is a graphics setting that sets everything to the lowest mode possible so
				// the device returns a reliable graphics setting.
				try {
					Log.Verbose ("GLCube", "Loading with custom Android settings (low mode)");
					GraphicsMode = new AndroidGraphicsMode (0, 0, 0, 0, 0, false);

					// if you don't call this, the context won't be created
					base.CreateFrameBuffer ();
				} catch (Exception ex1) {
					Log.Verbose ("GLCube", "{0}", ex1);
					throw new Exception ("Can't load egl, aborting");
				}
			}

			SetupProjection ();
		}

		// This gets called on each frame render
		protected override void OnRenderFrame (FrameEventArgs e)
		{
			// you only need to call this if you have delegates
			// registered that you want to have called
			base.OnRenderFrame (e);
			keys.RenderFrame ();
			SwapBuffers ();
		}

		internal void SetupProjection ()
		{
			if (Width <= 0 || Height <= 0)
				return;

			float aspect = (float)Width / Height;
			keys.view = Matrix4.LookAt (0, -20, 22.0f, 0f, 0f, 0f, 0f, 1.0f, 0.0f);
			GL.Viewport (0, 0, Width, Height);
			if (aspect > 1) {
				Matrix4 scale = Matrix4.Scale (aspect);
				keys.view = Matrix4.Mult (scale, keys.view);
				keys.textProjection = Matrix4.Mult (Matrix4.CreateTranslation (1, -1, 0), Matrix4.Scale (.5f, -.5f, 1));
			} else {
				keys.textProjection = Matrix4.Mult (Matrix4.CreateTranslation (1, -1, 0), Matrix4.Scale (.5f, -.5f, 1));
			}
			keys.projection = Matrix4.CreatePerspectiveFieldOfView (OpenTK.MathHelper.DegreesToRadians (42.0f), aspect, 1.0f, 70.0f);
			keys.projection = Matrix4.Mult (keys.view, keys.projection);
			keys.normalMatrix = Matrix4.Invert (keys.view);
			keys.normalMatrix.Transpose ();
		}
	}
}

