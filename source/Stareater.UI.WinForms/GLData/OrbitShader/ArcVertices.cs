using NGenerics.DataStructures.Mathematical;
using System.Collections.Generic;

namespace Stareater.GLData.OrbitShader
{
	class ArcVertices
	{
		public IEnumerable<float> Vertices { get; private set; }
		public Vector2D Center { get; private set; }
		public float Radius { get; private set; }

		public ArcVertices(IEnumerable<float> vertices, Vector2D center, float radius)
		{
			this.Vertices = vertices;
			this.Center = center;
			this.Radius = radius;
		}
	}
}
