using Stareater.GraphicsEngine;
using OpenTK.Graphics.OpenGL;
using System.IO;
using System.Reflection;

namespace Stareater.GLData.SdfShader
{
	class SdfGlProgram : AGlProgram
	{
		public const int VertexSize = 4;

		private int vertexShaderId;
		private int fragmentShaderId;

		public int LocalTransformId { get; private set; }
		public int ZId { get; private set; }
		public int TextureSamplerId { get; private set; }
		public int ColorId { get; private set; }
		public int SmoothDistId { get; private set; }

		public int LocalPositionId { get; private set; }
		public int TexturePositionId { get; private set; }

		public void Load()
		{
			string vertexShaderSource;
			string fragmentShaderSource;

			using (var stream = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("SdfVertexShader")))
				vertexShaderSource = stream.ReadToEnd();

			using (var stream = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("SdfFragmentShader")))
				fragmentShaderSource = stream.ReadToEnd();

			this.vertexShaderId = LoadShader(ShaderType.VertexShader, vertexShaderSource);
			this.fragmentShaderId = LoadShader(ShaderType.FragmentShader, fragmentShaderSource);
			this.ProgramId = BuildProgram(this.vertexShaderId, this.fragmentShaderId);

			this.ColorId = GL.GetUniformLocation(this.ProgramId, "color");
			this.LocalTransformId = GL.GetUniformLocation(this.ProgramId, "localtransform");
			this.SmoothDistId = GL.GetUniformLocation(this.ProgramId, "smoothDist");
			this.TextureSamplerId = GL.GetUniformLocation(this.ProgramId, "textureSampler");
			this.ZId = GL.GetUniformLocation(this.ProgramId, "z");

			this.LocalPositionId = GL.GetAttribLocation(this.ProgramId, "localPosition");
			this.TexturePositionId = GL.GetAttribLocation(this.ProgramId, "texturePosition");

			ShaderLibrary.PrintGlErrors("Load sprite program");
		}

		public override void SetupAttributes()
		{
			GL.VertexAttribPointer(this.LocalPositionId, 2, VertexAttribPointerType.Float, false, VertexSize * sizeof(float), 0);
			GL.EnableVertexAttribArray(this.LocalPositionId);
			GL.VertexAttribPointer(this.TexturePositionId, 2, VertexAttribPointerType.Float, false, VertexSize * sizeof(float), 2 * sizeof(float));
			GL.EnableVertexAttribArray(this.TexturePositionId);
		}
	}
}
