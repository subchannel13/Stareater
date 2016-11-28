using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Stareater.GLData
{
	class SpriteGlProgram : AGlProgram
	{
		public const int VertexSize = 4 * sizeof(float);
			
		private int vertexShaderId;
		private int fragmentShaderId;
		
		public int LocalTransformId { get; private set; }
		public int ZId { get; private set; }
		public int TextureSamplerId { get; private set; }
		public int ColorId { get; private set; }
	
		public int LocalPositionId { get; private set; }
		public int TexturePositionId { get; private set; }

		public void Activate()
		{
			GL.UseProgram(this.ProgramId);
		}
		
		public void Load()
		{
			string vertexShaderSource;
			string fragmentShaderSource;
			
			using(var stream = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("SpriteVertexShader")))
				vertexShaderSource = stream.ReadToEnd();
			
			using(var stream = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("SpriteFragmentShader")))
				fragmentShaderSource = stream.ReadToEnd();
			
			this.vertexShaderId = LoadShader(ShaderType.VertexShader, vertexShaderSource);
			this.fragmentShaderId = LoadShader(ShaderType.FragmentShader, fragmentShaderSource);
			this.ProgramId = BuildProgram(this.vertexShaderId, this.fragmentShaderId);
			
			this.ColorId = GL.GetUniformLocation(this.ProgramId, "color");
			this.LocalTransformId = GL.GetUniformLocation(this.ProgramId, "localtransform");
			this.TextureSamplerId = GL.GetUniformLocation(this.ProgramId, "textureSampler");
			this.ZId = GL.GetUniformLocation(this.ProgramId, "z");

			this.LocalPositionId = GL.GetAttribLocation(this.ProgramId, "localPosition");
			this.TexturePositionId = GL.GetAttribLocation(this.ProgramId, "texturePosition");
			
			ShaderLibrary.PrintGlErrors("Load sprite program");
		}
		
		public override void SetupAttributes()
		{
			GL.VertexAttribPointer(this.LocalPositionId, 2, VertexAttribPointerType.Float, false, VertexSize, 0);
			GL.EnableVertexAttribArray(this.LocalPositionId);
			GL.VertexAttribPointer(this.TexturePositionId, 2, VertexAttribPointerType.Float, false, VertexSize, 2 * sizeof(float));
			GL.EnableVertexAttribArray(this.TexturePositionId);
		}
		
		public class ObjectData
		{
			public float Z { get; private set; }
			public int TextureId { get; private set; }
			public Color4 Color { get; private set; }
			public Matrix4 LocalTransform { get; set; }

			public ObjectData(Matrix4 localTransform, float z, int textureId, Color color)
			{
				this.LocalTransform = localTransform;
				this.Z = z;
				this.TextureId = textureId;
				this.Color = new Color4(color.R, color.G, color.B, color.A);
			}
		}
	}
}
