using System;
using OpenTK;
using OpenTK.Graphics.ES30;

namespace GLKeysES30
{
	public class ES30Keys
	{
		const int UNIFORM_PROJECTION = 0;
		const int UNIFORM_TEX_PROJECTION = 1;
		const int UNIFORM_TEXTURE = 2;
		const int UNIFORM_TEX_DEPTH = 3;
		const int UNIFORM_LIGHT = 4;
		const int UNIFORM_VIEW = 5;
		const int UNIFORM_NORMAL_MATRIX = 6;
		const int UNIFORM_COUNT = 7;
		int[] uniforms = new int [UNIFORM_COUNT];
		const int ATTRIB_VERTEX = 0;
		const int ATTRIB_NORMAL = 1;
		const int ATTRIB_COUNT = 2;

		int vbo, vbi;
		internal void InitModel ()
		{
			GL.GenBuffers (1, out vbo);
			GL.BindBuffer (BufferTarget.ArrayBuffer, vbo);
			GL.BufferData (BufferTarget.ArrayBuffer, (IntPtr)(KeyModel.vertices.Length * sizeof(float)), KeyModel.vertices, BufferUsage.StaticDraw);
			GL.BindBuffer (BufferTarget.ArrayBuffer, 0);

			GL.GenBuffers (1, out vbi);
			GL.BindBuffer (BufferTarget.ElementArrayBuffer, vbi);
			GL.BufferData (BufferTarget.ElementArrayBuffer, (IntPtr)(KeyModel.faceIndexes.Length * sizeof(ushort)), KeyModel.faceIndexes, BufferUsage.StaticDraw);
			GL.BindBuffer (BufferTarget.ElementArrayBuffer, 0);
		}

		internal void Start ()
		{
			depthCounter = 0;
			textureDepth = new int[24];
			GL.Enable (EnableCap.DepthTest);
			GL.Enable (EnableCap.CullFace);
			GL.CullFace (CullFaceMode.Back);
		}

		internal void DrawModel ()
		{
			GL.ActiveTexture (TextureUnit.Texture0);
			GL.BindTexture ((TextureTarget)All.Texture2DArray, texturesId);
			GL.Uniform1 (uniforms [UNIFORM_TEXTURE], 0);

			// Update attribute values.
			GL.BindBuffer (BufferTarget.ArrayBuffer, vbo);
			GL.VertexAttribPointer (ATTRIB_VERTEX, 3, VertexAttribPointerType.Float, false, sizeof(float)*8, IntPtr.Zero);
			GL.EnableVertexAttribArray (ATTRIB_VERTEX);

			GL.VertexAttribPointer (ATTRIB_NORMAL, 3, VertexAttribPointerType.Float, false, sizeof(float)*8, new IntPtr (sizeof (float)*3));
			GL.EnableVertexAttribArray (ATTRIB_NORMAL);

			GL.BindBuffer (BufferTarget.ElementArrayBuffer, vbi);
			GL.DrawElementsInstanced (All.Triangles, KeyModel.faceIndexes.Length, All.UnsignedShort, IntPtr.Zero, 24);

			GL.BindBuffer (BufferTarget.ArrayBuffer, 0);
			GL.BindBuffer (BufferTarget.ElementArrayBuffer, 0);
		}

		int texturesId;
		internal delegate void CreateTextBitmapData (string str, out byte[] bitmapData, out int width, out int height);
		internal void CreateTextTextures (CreateTextBitmapData createData)
		{
			string[] texts = { " ", "O", "p", "e", "n", "G", "L", "E", "S", "3", ".", "0" };

			texturesId = GL.GenTexture ();

			GL.BindTexture ((TextureTarget)All.Texture2DArray, texturesId);
			GL.TexImage3D (All.Texture2DArray, 0, (int)All.Rgba, 256, 256, texts.Length, 0, All.Rgba, All.UnsignedByte, IntPtr.Zero);

			// setup texture parameters
			GL.TexParameter ((TextureTarget)All.Texture2DArray, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear);
			GL.TexParameter ((TextureTarget)All.Texture2DArray, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
			GL.TexParameter ((TextureTarget)All.Texture2DArray, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
			GL.TexParameter ((TextureTarget)All.Texture2DArray, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);

			for (int i = 0; i < texts.Length; i++) {
				byte[] bitmapData;
				int width, height;
				createData (texts [i], out bitmapData, out width, out height);
				GL.TexSubImage3D (All.Texture2DArray, 0, 0, 0, i, width, height, 1, All.Rgba, All.UnsignedByte, bitmapData);
				GL.GenerateMipmap ((TextureTarget)All.Texture2DArray);
			}
		}

		internal Matrix4 view = new Matrix4 ();
		internal Matrix4 normalMatrix = new Matrix4 ();
		internal Matrix4 projection = new Matrix4 ();
		internal Matrix4 textProjection = new Matrix4 ();

		int program;
		int depthCounter;
		int[] textureDepth = new int[24];
		int[] textureFinalIndex = {12, 13, 14, 15, 16, 17, 6, 7, 9, 10, 11};

		internal void RenderFrame ()
		{

			//projection = Matrix4.Mult (Matrix4.Scale (.99f), projection);

			if (depthCounter >= 20 && depthCounter < 12 * 20) {
				int offset = depthCounter / 20 - 1;
				textureDepth [textureFinalIndex [offset]] = offset + 1;
			}
			// Replace the implementation of this method to do your own custom drawing.
			GL.ClearColor (0.5f, 0.5f, 0.5f, 1.0f);
			GL.Clear (ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			CheckGLError ();

			// Use shader program.
			GL.UseProgram (program);
			CheckGLError ();

			// Update uniform value.
			GL.UniformMatrix4 (uniforms [UNIFORM_PROJECTION], false, ref projection);
			GL.UniformMatrix4 (uniforms [UNIFORM_VIEW], false, ref view);
			GL.UniformMatrix4 (uniforms [UNIFORM_NORMAL_MATRIX], false, ref normalMatrix);
			GL.UniformMatrix4 (uniforms [UNIFORM_TEX_PROJECTION], false, ref textProjection);
			GL.Uniform1 (uniforms [UNIFORM_TEX_DEPTH], textureDepth.Length, textureDepth);
			GL.Uniform3 (uniforms [UNIFORM_LIGHT], 25f, 25f, 28f);
			CheckGLError ();

			DrawModel ();
			CheckGLError ();

			// Validate program before drawing. This is a good check, but only really necessary in a debug build.
			#if DEBUG
			if (!ValidateProgram (program)) {
				Console.WriteLine ("Failed to validate program {0:x}\nGL Error: {1}", program, GL.GetError ());
				throw new Exception ("Invalid shaders program");
			}
			#endif

			depthCounter ++;
		}

		internal bool LoadShaders (string vertShaderSource, string fragShaderSource)
		{
			Console.WriteLine ("load shaders");
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
			uniforms [UNIFORM_TEX_PROJECTION] = GL.GetUniformLocation (program, "texProjection");
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
			if (program != 0) {
				GL.DeleteProgram (program);
				program = 0;
			}
		}

		static bool CompileShader (ShaderType type, string src, out int shader)
		{
			shader = GL.CreateShader (type);
			GL.ShaderSource (shader, src);
			GL.CompileShader (shader);

			#if DEBUG
			int logLength = 0;
			GL.GetShader (shader, ShaderParameter.InfoLogLength, out logLength);
			if (logLength > 0)
				Console.WriteLine ("Shader compile log:\n{0}", GL.GetShaderInfoLog (shader));
			#endif

			int status = 0;
			GL.GetShader (shader, ShaderParameter.CompileStatus, out status);
			if (status == 0) {
				GL.DeleteShader (shader);
				return false;
			}

			return true;
		}

		internal static bool LinkProgram (int prog)
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
			if (code != ErrorCode.NoError)
				Console.WriteLine ("GL Error {0}", code);
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
			}

			int status = 0;
			GL.GetProgram (prog, ProgramParameter.LinkStatus, out status);
			CheckGLError ();
			if (status == 0)
				return false;

			return true;
		}
	}
}

