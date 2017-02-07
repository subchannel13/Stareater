using System;
using System.IO;
using System.Reflection;
using OpenTK.Graphics.OpenGL;
using Stareater.GLData;
using Stareater.GraphicsEngine;

namespace Stareater.GLData.OrbitShader
{
	class PlanetOrbitGlProgram : AGlProgram
	{
		public const int VertexSize = 4;
			
		private int vertexShaderId;
		private int fragmentShaderId;
		
		public int LocalTransformId { get; private set; }
		public int ZId { get; private set; }
		public int MinRId { get; private set; }
		public int MaxRId { get; private set; }
		public int ColorId { get; private set; }
		public int TextureSamplerId { get; private set; }
		public int TextureTransformId { get; private set; }
	
		public int LocalPositionId { get; private set; }
		public int OrbitPositionId { get; private set; }

		public void Activate()
		{
			GL.UseProgram(this.ProgramId);
		}
		
		public void Load()
		{
			string vertexShaderSource;
			string fragmentShaderSource;
			
			using(var stream = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("OrbitVertexShader")))
				vertexShaderSource = stream.ReadToEnd();
			
			using(var stream = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("OrbitFragmentShader")))
				fragmentShaderSource = stream.ReadToEnd();
			
			this.vertexShaderId = LoadShader(ShaderType.VertexShader, vertexShaderSource);
			this.fragmentShaderId = LoadShader(ShaderType.FragmentShader, fragmentShaderSource);
			this.ProgramId = BuildProgram(this.vertexShaderId, this.fragmentShaderId);
			
			this.ColorId = GL.GetUniformLocation(this.ProgramId, "color");
			this.LocalTransformId = GL.GetUniformLocation(this.ProgramId, "localtransform");
			this.MinRId = GL.GetUniformLocation(this.ProgramId, "minR");
			this.MaxRId = GL.GetUniformLocation(this.ProgramId, "maxR");
			this.TextureSamplerId = GL.GetUniformLocation(this.ProgramId, "textureSampler");
			this.TextureTransformId = GL.GetUniformLocation(this.ProgramId, "textureTransform");
			this.ZId = GL.GetUniformLocation(this.ProgramId, "z");

			this.LocalPositionId = GL.GetAttribLocation(this.ProgramId, "localPosition");
			this.OrbitPositionId = GL.GetAttribLocation(this.ProgramId, "orbitPositionVert");
			
			ShaderLibrary.PrintGlErrors("Load planet orbit program");
		}
		
		public override void SetupAttributes()
		{
			GL.VertexAttribPointer(this.LocalPositionId, 2, VertexAttribPointerType.Float, false, VertexSize * sizeof(float), 0);
			GL.EnableVertexAttribArray(this.LocalPositionId);
			GL.VertexAttribPointer(this.OrbitPositionId, 2, VertexAttribPointerType.Float, false, VertexSize * sizeof(float), 2 * sizeof(float));
			GL.EnableVertexAttribArray(this.OrbitPositionId);
		}
	}
}
