using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.ES30;
using GL1 = OpenTK.Graphics.ES11.GL;
using All1 = OpenTK.Graphics.ES11.All;
using OpenTK.Platform.iPhoneOS;
using Foundation;
using CoreAnimation;
using CoreGraphics;
using ObjCRuntime;
using OpenGLES;
using UIKit;

namespace GLKeysES30
{
	[Register ("EAGLView")]
	public class EAGLView : iPhoneOSGameView
	{
		ES30Keys keys;

		[Export ("initWithCoder:")]
		public EAGLView (NSCoder coder) : base (coder)
		{
			LayerRetainsBacking = true;
			LayerColorFormat = EAGLColorFormat.RGBA8;

			// retina support
			ContentScaleFactor = UIScreen.MainScreen.Scale;

			keys = new ES30Keys ();
		}

		[Export ("layerClass")]
		public static new Class GetLayerClass ()
		{
			return iPhoneOSGameView.GetLayerClass ();
		}

		protected override void ConfigureLayer (CAEAGLLayer eaglLayer)
		{
			eaglLayer.Opaque = true;
			eaglLayer.AllowsEdgeAntialiasing = true;
			eaglLayer.EdgeAntialiasingMask = CAEdgeAntialiasingMask.All;
		}

		int depthRenderbuffer;
		protected override void CreateFrameBuffer ()
		{
			try {
				ContextRenderingApi = EAGLRenderingAPI.OpenGLES3;

				nfloat screenScale = UIScreen.MainScreen.Scale;
				CAEAGLLayer eaglLayer = (CAEAGLLayer) Layer;
				Size size = new Size (
					(int) Math.Round (screenScale * eaglLayer.Bounds.Size.Width), 
					(int) Math.Round (screenScale * eaglLayer.Bounds.Size.Height));

				base.CreateFrameBuffer ();

				GL.GenRenderbuffers (1, out depthRenderbuffer);
				GL.BindRenderbuffer (RenderbufferTarget.Renderbuffer, depthRenderbuffer);
				GL.RenderbufferStorage (RenderbufferTarget.Renderbuffer, RenderbufferInternalFormat.DepthComponent16, size.Width, size.Height);
				GL.FramebufferRenderbuffer (FramebufferTarget.Framebuffer, FramebufferSlot.DepthAttachment, RenderbufferTarget.Renderbuffer, depthRenderbuffer);

				Console.WriteLine ("using ES 3.0");
				Console.WriteLine ("version: {0} glsl version: {1}", GL.GetString (StringName.Version), GL.GetString (StringName.ShadingLanguageVersion));
			} catch (Exception e) {
				throw new Exception ("Looks like OpenGL ES 3.0 not available", e);
			}
			
			if (ContextRenderingApi == EAGLRenderingAPI.OpenGLES2 || ContextRenderingApi == EAGLRenderingAPI.OpenGLES3)
				keys.LoadShaders ();
		}

		protected override void DestroyFrameBuffer ()
		{
			base.DestroyFrameBuffer ();
			keys.DestroyShaders ();
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
			SetupProjection ();
			
			keys.CreateTextTextures (CreateBitmapData);
			keys.InitModel ();
			keys.Start ();
			CADisplayLink displayLink = UIScreen.MainScreen.CreateDisplayLink (this, new Selector ("drawFrame"));
			displayLink.FrameInterval = frameInterval;
			displayLink.AddToRunLoop (NSRunLoop.Current, NSRunLoop.NSDefaultRunLoopMode);
			this.displayLink = displayLink;
			
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

		void CreateBitmapData (string str, out byte[] bitmapData, out int width, out int height)
		{
			using(CGBitmapContext bitmapContext = CreateTextBitmapContext (str, out bitmapData)) {
				width = bitmapContext.Width; height = bitmapContext.Height;
			}
		}

		internal void SetupProjection ()
		{
			nfloat screenScale = UIScreen.MainScreen.Scale;
			CGRect bounds = UIScreen.MainScreen.Bounds;
			nfloat aspect = bounds.Width / bounds.Height;
			UIInterfaceOrientation orientation = UIApplication.SharedApplication.StatusBarOrientation;
			keys.view = Matrix4.LookAt (0, -20, 22, 0f, 0f, 0f, 0f, 1.0f, 0.0f);
			GL.Viewport (0, 0, (int)(screenScale * bounds.Width), (int)(screenScale * bounds.Height));
			if (orientation == UIInterfaceOrientation.LandscapeLeft || orientation == UIInterfaceOrientation.LandscapeRight) {
				aspect = 1 / aspect;
				Matrix4 scale = Matrix4.Scale ((float)aspect);
				keys.view = Matrix4.Mult (scale, keys.view);
				keys.textProjection = Matrix4.Mult (Matrix4.CreateTranslation (1, 1, 0), Matrix4.Scale (.5f, .5f*(float)aspect, 1));
			} else {
				GL.Viewport (0, 0, (int)(screenScale * bounds.Width), (int)(screenScale * bounds.Height));
				keys.textProjection = Matrix4.Mult (Matrix4.CreateTranslation (1, 1, 0), Matrix4.Scale (.5f, .5f / (float)aspect, 1));
			}

			keys.projection = Matrix4.CreatePerspectiveFieldOfView (OpenTK.MathHelper.DegreesToRadians (42.0f), (float)aspect, (float)1.0f, 70.0f);
			keys.projection = Matrix4.Mult (keys.view, keys.projection);
			keys.normalMatrix = Matrix4.Invert (keys.view);
			keys.normalMatrix.Transpose ();

			keys.Start ();
		}

		void CheckGLError ()
		{
			ErrorCode code = GL.GetErrorCode ();
			if (code != ErrorCode.NoError) {
				Console.WriteLine ("GL Error {0}", code);
			}
		}

		CGBitmapContext CreateTextBitmapContext (string str, out byte[] bitmapData)
		{
			NSString text = new NSString (str);
			UIFont font = UIFont.FromName ("HelveticaNeue-Light", 128);
			CGSize size = text.StringSize (font);
			int width = (int)size.Width;
			int height = (int)size.Height;
			bitmapData = new byte[256*256*4];
			CGBitmapContext bitmapContext = new CGBitmapContext (bitmapData, 256, 256, 8, 256*4, CGColorSpace.CreateDeviceRGB (), CGImageAlphaInfo.PremultipliedLast);
			//Console.WriteLine ("bitmap context size: {0} x {1}", bitmapContext.Width, bitmapContext.Height);
			UIGraphics.PushContext (bitmapContext);
			float grayLevel = str == " " ? .8f : 1;
			bitmapContext.SetFillColor(grayLevel, grayLevel, grayLevel, 1);
			bitmapContext.FillRect (new RectangleF (0, 0, 256.0f, 256.0f));
			bitmapContext.SetFillColor(0, 0, 0, 1);

			text.DrawString (new CoreGraphics.CGPoint((256.0f - width) / 2.0f, (256.0f - height) / 2.0f + font.Descender), font);
			UIGraphics.PopContext ();

			return bitmapContext;
		}


		protected override void OnRenderFrame (FrameEventArgs e)
		{
			base.OnRenderFrame (e);

			MakeCurrent ();
			if (ContextRenderingApi == EAGLRenderingAPI.OpenGLES2 || ContextRenderingApi == EAGLRenderingAPI.OpenGLES3)
				keys.RenderFrame ();
			SwapBuffers ();
		}
	}
}
