using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK;
using Stareater.GLData;

namespace Stareater.GraphicsEngine
{
	class PolygonData
	{
		public float Z { get; private set; }
		public IShaderData ShaderData { get; private set; }
		public ICollection<float> VertexData { get; private set; }
		private IDrawable spawnedDrawable = null;
		
		public PolygonData(float z, IShaderData shaderUniforms, ICollection<float> vertexData)
		{
			this.Z = z;
			this.ShaderData = shaderUniforms;
			this.VertexData = vertexData;
		}

		//TODO(later) try to remove
		public PolygonData(float z, IShaderData shaderUniforms, IEnumerable<Vector2> vertexData) :
			this(z, shaderUniforms, vertexData.SelectMany(v => new [] {v.X, v.Y}).ToList())
		{ }
		
		public IDrawable MakeDrawable(VertexArray vao, int objectIndex)
		{
			this.spawnedDrawable = this.ShaderData.MakeDrawable(vao, objectIndex);
			return this.spawnedDrawable;
		}
		
		public void UpdateDrawable(IShaderData shaderUniforms)
		{
			this.ShaderData = shaderUniforms;
			
			if (this.spawnedDrawable != null)
				this.spawnedDrawable.Update(shaderUniforms);
		}
	}
}
