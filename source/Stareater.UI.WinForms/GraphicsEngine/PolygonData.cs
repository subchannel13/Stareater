using System;
using System.Collections.Generic;

namespace Stareater.GraphicsEngine
{
	class PolygonData
	{
		public float Z { get; private set; }
		public IShaderData ShaderData { get; private set; }
		public ICollection<float> VertexData { get; private set; }
		
		public PolygonData(float z, IShaderData shaderUniforms, ICollection<float> vertexData)
		{
			this.Z = z;
			this.ShaderData = shaderUniforms;
			this.VertexData = vertexData;
		}
	}
}
