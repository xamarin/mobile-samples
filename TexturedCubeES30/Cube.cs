using System;
using OpenTK;
using OpenTK.Graphics.ES30;

namespace Mono.Samples.TexturedCube
{
	public class Cube
	{
		public float xAngle = 45, yAngle = 45;
		public float xAcc, yAcc;
		public float xSign = 1, ySign = 1;
		float xInc = .01f, yInc = .0033f;
		bool UseTexture = false;
		int Width, Height;
		int textureId;
		int programTexture, programPlain, currentProgram;

		const int UNIFORM_PROJECTION = 0;
		const int UNIFORM_TEXTURE = 1;
		const int UNIFORM_TEX_DEPTH = 2;
		const int UNIFORM_LIGHT = 3;
		const int UNIFORM_VIEW = 4;
		const int UNIFORM_NORMAL_MATRIX = 5;
		const int UNIFORM_COUNT = 6;
		int[] uniforms = new int [UNIFORM_COUNT];
		const int ATTRIB_VERTEX = 0;
		const int ATTRIB_NORMAL = 1;
		const int ATTRIB_TEXCOORD = 2;
		const int ATTRIB_COUNT = 3;
		int vbo, vbi;

		public Cube ()
		{
		}

		internal void Initialize ()
		{
			GL.ClearColor (0, 0, 0, 1);

			GL.ClearDepth (1.0f);
			GL.Enable (EnableCap.DepthTest);
			GL.Enable (EnableCap.CullFace);
			GL.CullFace (CullFaceMode.Back);
			GL.Hint (HintTarget.GenerateMipmapHint, HintMode.Nicest);

			textureId = GL.GenTexture ();
			string vertexShaderSource = LoadResource ("Mono.Samples.TexturedCube.Resources.Shader.vsh");
			LoadShaders (vertexShaderSource, LoadResource ("Mono.Samples.TexturedCube.Resources.Shader.fsh"), out programTexture);
			LoadShaders (vertexShaderSource, LoadResource ("Mono.Samples.TexturedCube.Resources.ShaderPlain.fsh"), out programPlain);
			ToggleTexture ();
			InitModel ();
			Render ();
		}

		internal void DeleteTexture ()
		{
			GL.DeleteTexture (textureId);
		}

		public void Render ()
		{
			GL.Clear (ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			// Use shader program.
			GL.UseProgram (currentProgram);

			// Update uniform value.
			GL.UniformMatrix4 (uniforms [UNIFORM_PROJECTION], false, ref projection);
			GL.UniformMatrix4 (uniforms [UNIFORM_VIEW], false, ref view);
			GL.UniformMatrix4 (uniforms [UNIFORM_NORMAL_MATRIX], false, ref normalMatrix);
			GL.Uniform3 (uniforms [UNIFORM_LIGHT], 25f, 25f, 28f);

			DrawModel ();
		}

		string LoadResource (string name)
		{
			return new System.IO.StreamReader (System.Reflection.Assembly.GetExecutingAssembly ().GetManifestResourceStream (name)).ReadToEnd ();
		}

		public void InitModel ()
		{
			GL.GenBuffers (1, out vbo);
			GL.BindBuffer (BufferTarget.ArrayBuffer, vbo);
			GL.BufferData (BufferTarget.ArrayBuffer, (IntPtr)(CubeModel.vertices.Length * sizeof(float)), CubeModel.vertices, BufferUsage.StaticDraw);
			GL.BindBuffer (BufferTarget.ArrayBuffer, 0);

			GL.GenBuffers (1, out vbi);
			GL.BindBuffer (BufferTarget.ElementArrayBuffer, vbi);
			GL.BufferData (BufferTarget.ElementArrayBuffer, (IntPtr)(CubeModel.faceIndexes.Length * sizeof(ushort)), CubeModel.faceIndexes, BufferUsage.StaticDraw);
			GL.BindBuffer (BufferTarget.ElementArrayBuffer, 0);
		}

		internal void DrawModel ()
		{
			GL.ActiveTexture (TextureUnit.Texture0);
			GL.BindTexture (TextureTarget.Texture2D, textureId);
			GL.Uniform1 (uniforms [UNIFORM_TEXTURE], 0);

			// Update attribute values.
			GL.BindBuffer (BufferTarget.ArrayBuffer, vbo);
			GL.VertexAttribPointer (ATTRIB_VERTEX, 3, VertexAttribPointerType.Float, false, sizeof(float)*8, IntPtr.Zero);
			GL.EnableVertexAttribArray (ATTRIB_VERTEX);

			GL.VertexAttribPointer (ATTRIB_NORMAL, 3, VertexAttribPointerType.Float, false, sizeof(float)*8, new IntPtr (sizeof (float)*3));
			GL.EnableVertexAttribArray (ATTRIB_NORMAL);

			GL.VertexAttribPointer (ATTRIB_TEXCOORD, 3, VertexAttribPointerType.Float, false, sizeof(float)*8, new IntPtr (sizeof (float)*6));
			GL.EnableVertexAttribArray (ATTRIB_TEXCOORD);

			GL.BindBuffer (BufferTarget.ElementArrayBuffer, vbi);
			GL.DrawElementsInstanced (PrimitiveType.Triangles, CubeModel.faceIndexes.Length, DrawElementsType.UnsignedShort, IntPtr.Zero, 24);

			GL.BindBuffer (BufferTarget.ArrayBuffer, 0);
			GL.BindBuffer (BufferTarget.ElementArrayBuffer, 0);
		}

		public delegate void LoadBitmapData (int texId);
		public void LoadTexture (LoadBitmapData loadBitmapData)
		{
			GL.BindTexture (TextureTarget.Texture2D, textureId);

			// setup texture parameters
			GL.TexParameter (TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)All.NearestMipmapLinear);
			GL.TexParameter (TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
			GL.TexParameter (TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
			GL.TexParameter (TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

			loadBitmapData (textureId);

			GL.GenerateMipmap (TextureTarget.Texture2D);
		}

		Matrix4 view = new Matrix4 ();
		Matrix4 normalMatrix = new Matrix4 ();
		Matrix4 projection = new Matrix4 ();

		internal void SetupProjection (int width, int height)
		{
			Matrix4 model = Matrix4.Mult (Matrix4.CreateRotationX (-xAngle), Matrix4.CreateRotationZ (-yAngle));

			float aspect = (float)width / height;
			if (aspect > 1) {
				Matrix4 scale = Matrix4.Scale (aspect);
				model = Matrix4.Mult (model, scale);
			}
			view = Matrix4.Mult (model, Matrix4.LookAt (0, -70, 5, 0, 10, 0, 0, 1, 0));
			projection = Matrix4.CreatePerspectiveFieldOfView (OpenTK.MathHelper.DegreesToRadians (42.0f), aspect, 1.0f, 200.0f);
			projection = Matrix4.Mult (view, projection);
			normalMatrix = Matrix4.Invert (view);
			normalMatrix.Transpose ();

			Width = width;
			Height = height;
		}

		public void ToggleTexture ()
		{
			UseTexture = !UseTexture;
			currentProgram = UseTexture ? programTexture : programPlain;
		}

		bool LoadShaders (string vertShaderSource, string fragShaderSource, out int program)
		{
			int vertShader, fragShader;

			// Create shader program.
			program = GL.CreateProgram ();

			// Create and compile vertex shader.
			if (!CompileShader (ShaderType.VertexShader, vertShaderSource, out vertShader)) {
				Console.WriteLine ("Failed to compile vertex shader");
				return false;
			}
			// Create and compile fragment shader.
			if (!CompileShader (ShaderType.FragmentShader, fragShaderSource, out fragShader)) {
				Console.WriteLine ("Failed to compile fragment shader");
				return false;
			}

			// Attach vertex shader to program.
			GL.AttachShader (program, vertShader);

			// Attach fragment shader to program.
			GL.AttachShader (program, fragShader);

			// Bind attribute locations.
			// This needs to be done prior to linking.
			GL.BindAttribLocation (program, ATTRIB_VERTEX, "position");
			GL.BindAttribLocation (program, ATTRIB_NORMAL, "normal");
			GL.BindAttribLocation (program, ATTRIB_TEXCOORD, "texcoord");

			// Link program.
			if (!LinkProgram (program)) {
				Console.WriteLine ("Failed to link program: {0:x}", program);

				if (vertShader != 0)
					GL.DeleteShader (vertShader);

				if (fragShader != 0)
					GL.DeleteShader (fragShader);

				if (program != 0) {
					GL.DeleteProgram (program);
					program = 0;
				}
				return false;
			}

			// Get uniform locations.
			uniforms [UNIFORM_PROJECTION] = GL.GetUniformLocation (program, "projection");
			uniforms [UNIFORM_VIEW] = GL.GetUniformLocation (program, "view");
			uniforms [UNIFORM_NORMAL_MATRIX] = GL.GetUniformLocation (program, "normalMatrix");
			uniforms [UNIFORM_TEXTURE] = GL.GetUniformLocation (program, "text");
			uniforms [UNIFORM_TEX_DEPTH] = GL.GetUniformLocation (program, "texDepth");
			uniforms [UNIFORM_LIGHT] = GL.GetUniformLocation (program, "light");

			// Release vertex and fragment shaders.
			if (vertShader != 0) {
				GL.DetachShader (program, vertShader);
				GL.DeleteShader (vertShader);
			}

			if (fragShader != 0) {
				GL.DetachShader (program, fragShader);
				GL.DeleteShader (fragShader);
			}

			return true;
		}

		internal void DestroyShaders ()
		{
			if (programTexture != 0) {
				GL.DeleteProgram (programTexture);
				programTexture = 0;
			}
			if (programPlain != 0) {
				GL.DeleteProgram (programPlain);
				programPlain = 0;
			}
		}

		static bool CompileShader (ShaderType type, string src, out int shader)
		{
			shader = GL.CreateShader (type);
			GL.ShaderSource (shader, src);
			GL.CompileShader (shader);

			#if DEBUG || true
			int logLength = 0;
			GL.GetShader (shader, ShaderParameter.InfoLogLength, out logLength);
			if (logLength > 0) {
				Console.WriteLine ("Shader compile log:\n{0}", GL.GetShaderInfoLog (shader));
			}
			#endif

			int status = 0;
			GL.GetShader (shader, ShaderParameter.CompileStatus, out status);
			if (status == 0) {
				GL.DeleteShader (shader);
				return false;
			}

			return true;
		}

		static bool LinkProgram (int prog)
		{
			GL.LinkProgram (prog);

			#if DEBUG
			int logLength = 0;
			GL.GetProgram (prog, ProgramParameter.InfoLogLength, out logLength);
			if (logLength > 0)
				Console.WriteLine ("Program link log:\n{0}", GL.GetProgramInfoLog (prog));
			#endif
			int status = 0;
			GL.GetProgram (prog, ProgramParameter.LinkStatus, out status);
			if (status == 0)
				return false;

			return true;
		}

		static void CheckGLError ()
		{
			ErrorCode code = GL.GetErrorCode ();
			if (code != ErrorCode.NoError) {
				Console.WriteLine ("GL Error {0}", code);
			}
		}

		static bool ValidateProgram (int prog)
		{
			GL.ValidateProgram (prog);
			CheckGLError ();

			int logLength = 0;
			GL.GetProgram (prog, ProgramParameter.InfoLogLength, out logLength);
			CheckGLError ();
			if (logLength > 0) {
				var infoLog = new System.Text.StringBuilder (logLength);
				GL.GetProgramInfoLog (prog, logLength, out logLength, infoLog);
				Console.WriteLine ("Program validate log:\n{0}", infoLog);
			}

			int status = 0;
			GL.GetProgram (prog, ProgramParameter.LinkStatus, out status);
			CheckGLError ();
			if (status == 0)
				return false;

			return true;
		}

		public void UpdateWorld ()
		{
			xAngle += xSign * (xInc + xAcc * xAcc);
			yAngle += ySign * (yInc + yAcc * yAcc);
			SetupProjection (Width, Height);
			xAcc = System.Math.Max (0, xAcc - 0.001f);
			yAcc = System.Math.Max (0, yAcc - 0.001f);
		}

		public void Move (float xDiff, float yDiff)
		{
			xSign = yDiff > 0 ? 1 : -1;
			ySign = xDiff > 0 ? 1 : -1;
			xDiff = ySign*System.Math.Min (30, System.Math.Abs (xDiff));
			yDiff = xSign*System.Math.Min (30, System.Math.Abs (yDiff));
			xAngle += yDiff / 200;
			yAngle += xDiff / 200;
			xAcc = System.Math.Abs (yDiff / 100);
			yAcc = System.Math.Abs (xDiff / 100);
		}
	}
}

