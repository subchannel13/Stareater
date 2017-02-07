using System;
using OpenTK;
using OpenTK.Graphics;
using Stareater.GraphicsEngine;

namespace Stareater.GLData.OrbitShader
{
	class OrbitData : IShaderData
	{
		public float MinRadius { get; private set; }
		public float MaxRadius { get; private set; }
		public Color4 Color { get; private set; }
		public Matrix4 LocalTransform { get; set; }

		public OrbitData(float minRadius, float maxRadius, Color4 color, Matrix4 localTransform)
		{
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

		public int VertexDataSize 
		{ 
			get { return PlanetOrbitGlProgram.VertexSize; }
		}
		
		public IDrawable MakeDrawable(VertexArray vao, int objectIndex)
		{
			return new OrbitDrawable(vao, objectIndex, this);
		}
		#endregion
	}
}
