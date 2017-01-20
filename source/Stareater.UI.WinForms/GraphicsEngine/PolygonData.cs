using System;
using System.Collections.Generic;

namespace Stareater.GraphicsEngine
{
	class PolygonData
	{
		public float Z { get; private set; }
		public IShaderUniformData ShaderUniforms { get; private set; }
		public IEnumerable<float> VertexData { get; private set; }
		
		public PolygonData(float z, IShaderUniformData shaderUniforms, IEnumerable<float> vertexData)
		{
			this.Z = z;
			this.ShaderUniforms = shaderUniforms;
			this.VertexData = vertexData;
		}
	}
}
