using System;
using System.IO;
using System.Reflection;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using Stareater.GraphicsEngine;

namespace Stareater.GLData
{
	class PlanetOrbitGlProgram : AGlProgram
	{
		public const int VertexSize = 4 * sizeof(float);
			
		private int vertexShaderId;
		private int fragmentShaderId;
		
		public int LocalTransformId { get; private set; }
		public int ZId { get; private set; }
		public int MinRId { get; private set; }
		public int MaxRId { get; private set; }
		public int ColorId { get; private set; }
	
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
			this.ZId = GL.GetUniformLocation(this.ProgramId, "z");

			this.LocalPositionId = GL.GetAttribLocation(this.ProgramId, "localPosition");
			this.OrbitPositionId = GL.GetAttribLocation(this.ProgramId, "orbitPositionVert");
			
			ShaderLibrary.PrintGlErrors("Load planet orbit program");
		}
		
		public override void SetupAttributes()
		{
			GL.VertexAttribPointer(this.LocalPositionId, 2, VertexAttribPointerType.Float, false, VertexSize, 0);
			GL.EnableVertexAttribArray(this.LocalPositionId);
			GL.VertexAttribPointer(this.OrbitPositionId, 2, VertexAttribPointerType.Float, false, VertexSize, 2 * sizeof(float));
			GL.EnableVertexAttribArray(this.OrbitPositionId);
		}
		
		public class ObjectData : IShaderUniformData
		{
			public float Z { get; private set; } //TODO(v0.6) remove, redundant
			public float MinRadius { get; private set; }
			public float MaxRadius { get; private set; }
			public Color4 Color { get; private set; }
			public Matrix4 LocalTransform { get; set; }

			public ObjectData(float z, float minRadius, float maxRadius, Color4 color, Matrix4 localTransform)
			{
				this.Z = z;
				this.MinRadius = minRadius;
				this.MaxRadius = maxRadius;
				this.Color = color;
				this.LocalTransform = localTransform;
			}

			#region IShaderUniformData implementation

			public AGlProgram ForProgram
			{
				get { return ShaderLibrary.PlanetOrbit; }
			}

			#endregion
		}
	}
}
