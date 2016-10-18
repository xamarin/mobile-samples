using System;

using CoreAnimation;
using CoreGraphics;
using Foundation;
using Mono.Samples.TexturedCube;
using ObjCRuntime;
using OpenGLES;
using OpenTK;
using OpenTK.Graphics.ES30;
using OpenTK.Platform.iPhoneOS;
using UIKit;

namespace TexturedCubeiOS
{
	[Register ("EAGLView")]
	public class EAGLView : iPhoneOSGameView
	{
		readonly Cube cube = new Cube ();

		[Export ("initWithCoder:")]
		public EAGLView (NSCoder coder) : base (coder)
		{
			LayerRetainsBacking = true;
			LayerColorFormat = EAGLColorFormat.RGBA8;

			// retina support
			ContentScaleFactor = UIScreen.MainScreen.Scale;
		}

		[Export ("layerClass")]
		public static new Class GetLayerClass ()
		{
			return iPhoneOSGameView.GetLayerClass ();
		}

		protected override void ConfigureLayer (CAEAGLLayer eaglLayer)
		{
			eaglLayer.Opaque = true;
		}

		void LoadBitmapData (int texId)
		{
			NSData texData = NSData.FromFile (NSBundle.MainBundle.PathForResource ("texture1", "png"));

			UIImage image = UIImage.LoadFromData (texData);
			if (image == null)
				return;

			nint width = image.CGImage.Width;
			nint height = image.CGImage.Height;

			CGColorSpace colorSpace = CGColorSpace.CreateDeviceRGB ();
			byte[] imageData = new byte[height * width * 4];
			CGContext context = new CGBitmapContext (imageData, width, height, 8, 4 * width, colorSpace,
				                    CGBitmapFlags.PremultipliedLast | CGBitmapFlags.ByteOrder32Big);

			context.TranslateCTM (0, height);
			context.ScaleCTM (1, -1);
			colorSpace.Dispose ();
			context.ClearRect (new CGRect (0, 0, width, height));
			context.DrawImage (new CGRect (0, 0, width, height), image.CGImage);

			GL.TexImage2D (TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, (int)width, (int)height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, imageData);
			context.Dispose ();
		}


		protected override void CreateFrameBuffer ()
		{
			nfloat screenScale = UIScreen.MainScreen.Scale;
			CAEAGLLayer eaglLayer = (CAEAGLLayer) Layer;
			CGSize size = new CGSize (
				(int) Math.Round (screenScale * eaglLayer.Bounds.Size.Width), 
				(int) Math.Round (screenScale * eaglLayer.Bounds.Size.Height));

			try {
				ContextRenderingApi = EAGLRenderingAPI.OpenGLES3;

				base.CreateFrameBuffer ();

				int depthRenderbuffer;
				GL.GenRenderbuffers (1, out depthRenderbuffer);
				GL.BindRenderbuffer (RenderbufferTarget.Renderbuffer, depthRenderbuffer);
				GL.RenderbufferStorage (RenderbufferTarget.Renderbuffer, RenderbufferInternalFormat.DepthComponent16, (int)size.Width, (int)size.Height);
				GL.FramebufferRenderbuffer (FramebufferTarget.Framebuffer, FramebufferSlot.DepthAttachment, RenderbufferTarget.Renderbuffer, depthRenderbuffer);

				Console.WriteLine ("using ES 3.0");
				Console.WriteLine ("version: {0} glsl version: {1}", GL.GetString (StringName.Version), GL.GetString (StringName.ShadingLanguageVersion));
			} catch (Exception e) {
				throw new Exception ("Looks like OpenGL ES 3.0 not available", e);
			}

			cube.Initialize ();
			cube.LoadTexture (LoadBitmapData);
			SetupProjection ();
		}

		void SetupProjection ()
		{
			var eaglLayer = (CAEAGLLayer)Layer;
			var width = (int)(UIScreen.MainScreen.Scale * eaglLayer.Bounds.Size.Width);
			var height = (int)(UIScreen.MainScreen.Scale * eaglLayer.Bounds.Size.Height);
			cube.SetupProjection (width, height);
			GL.Viewport (0, 0, width, height);
		}

		protected override void DestroyFrameBuffer ()
		{
			base.DestroyFrameBuffer ();
			cube.DeleteTexture ();
		}

		#region DisplayLink support

		int frameInterval;
		CADisplayLink displayLink;

		public bool IsAnimating { get; private set; }
		// How many display frames must pass between each time the display link fires.
		public int FrameInterval {
			get {
				return frameInterval;
			}
			set {
				if (value <= 0)
					throw new ArgumentException ();
				frameInterval = value;
				if (IsAnimating) {
					StopAnimating ();
					StartAnimating ();
				}
			}
		}

		public void StartAnimating ()
		{
			if (IsAnimating)
				return;
			
			CreateFrameBuffer ();
			displayLink = UIScreen.MainScreen.CreateDisplayLink (this, new Selector ("drawFrame"));
			displayLink.FrameInterval = frameInterval;
			displayLink.AddToRunLoop (NSRunLoop.Current, NSRunLoop.NSDefaultRunLoopMode);
			
			IsAnimating = true;
		}

		public void StopAnimating ()
		{
			if (!IsAnimating)
				return;

			displayLink.Invalidate ();
			displayLink = null;
			DestroyFrameBuffer ();
			IsAnimating = false;
		}

		[Export ("drawFrame")]
		void DrawFrame ()
		{
			OnRenderFrame (new FrameEventArgs ());
		}

		#endregion

		protected override void OnRenderFrame (FrameEventArgs e)
		{
			base.OnRenderFrame (e);

			if (touchesActive)
				return;

			MakeCurrent ();
			cube.UpdateWorld ();
			cube.Render ();
			SwapBuffers ();
		}

		bool touchesMoved;
		bool touchesActive;
		public override void TouchesBegan (NSSet touches, UIEvent evt)
		{
			base.TouchesBegan (touches, evt);
			touchesMoved = false;
			touchesActive = true;
		}

		public override void TouchesMoved (NSSet touches, UIEvent evt)
		{
			base.TouchesMoved (touches, evt);
			touchesMoved = true;

			var touch = touches.AnyObject as UITouch;
			nfloat xdiff = touch.PreviousLocationInView(this).X - touch.LocationInView(this).X;
			nfloat ydiff = touch.PreviousLocationInView(this).Y - touch.LocationInView(this).Y;
			cube.Move ((float)xdiff, (float)ydiff);
			MakeCurrent ();
			SetupProjection ();
			cube.Render ();
			SwapBuffers ();
		}

		public override void TouchesEnded (NSSet touches, UIEvent evt)
		{
			base.TouchesEnded (touches, evt);
			if (!touchesMoved)
				cube.ToggleTexture ();
			touchesActive = false;
		}
	}
}
