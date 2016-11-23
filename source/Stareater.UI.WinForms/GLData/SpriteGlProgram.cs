using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Stareater.GLData
{
	class SpriteGlProgram : IGlProgram
	{
		public const int VertexSize = 4 * sizeof(float);
			
		private int vertexShaderId;
		private int fragmentShaderId;
		public int ProgramId { get; private set; }
		
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
			var bla = Assembly.GetExecutingAssembly().GetManifestResourceNames();
			
			using(var stream = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("SpriteVertexShader")))
				vertexShaderSource = stream.ReadToEnd();
			
			using(var stream = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("SpriteFragmentShader")))
				fragmentShaderSource = stream.ReadToEnd();
			
			this.vertexShaderId = loadShader(ShaderType.VertexShader, vertexShaderSource);
			this.fragmentShaderId = loadShader(ShaderType.FragmentShader, fragmentShaderSource);
			this.ProgramId = buildProgram(this.vertexShaderId, this.fragmentShaderId);
			
			this.ColorId = GL.GetUniformLocation(this.ProgramId, "color");
			this.LocalTransformId = GL.GetUniformLocation(this.ProgramId, "localtransform");
			this.TextureSamplerId = GL.GetUniformLocation(this.ProgramId, "textureSampler");
			this.ZId = GL.GetUniformLocation(this.ProgramId, "z");

			this.LocalPositionId = GL.GetAttribLocation(this.ProgramId, "localPosition");
			this.TexturePositionId = GL.GetAttribLocation(this.ProgramId, "texturePosition");
			
			ShaderLibrary.PrintGlErrors("Load sprite program");
		}
		
		public void SetupAttributes()
		{
			GL.VertexAttribPointer(this.LocalPositionId, 2, VertexAttribPointerType.Float, false, VertexSize, 0);
			GL.EnableVertexAttribArray(this.LocalPositionId);
			GL.VertexAttribPointer(this.TexturePositionId, 2, VertexAttribPointerType.Float, false, VertexSize, 2 * sizeof(float));
			GL.EnableVertexAttribArray(this.TexturePositionId);
		}
		
		private int loadShader(ShaderType type, string source)
		{
			var shader = GL.CreateShader(type);
			if (shader == 0)
			{
				System.Diagnostics.Trace.WriteLine("Error creating shader\n");
				return 0;
			}
		
			GL.ShaderSource(shader, source);
			GL.CompileShader(shader);
			int compiled;
			GL.GetShader(shader, ShaderParameter.CompileStatus, out compiled);
			
			if (compiled == 0)
			{
				System.Diagnostics.Trace.WriteLine("Shader compilation error\n");
				System.Diagnostics.Trace.WriteLine("Reason: " + GL.GetShaderInfoLog(shader));
				GL.DeleteShader(shader);

				return 0;
			}
		
			return shader;
		}
		
		private int buildProgram(int vertexShader, int fragmentShader)
		{
			var programObject = GL.CreateProgram();
			GL.AttachShader(programObject, vertexShader);
			GL.AttachShader(programObject, fragmentShader);
			GL.BindFragDataLocation(programObject, 0, "outputF");
			GL.LinkProgram(programObject);
		
			int linked;
			GL.GetProgram(programObject, ProgramParameter.LinkStatus, out linked);
			if (linked == 0)
			{
				System.Diagnostics.Trace.WriteLine("Program link error");
				GL.DeleteProgram(programObject);
				
				return 0;
			}
			
			return programObject;
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
