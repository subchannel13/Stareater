using OpenTK;
using System.Collections.Generic;

namespace Stareater.GLData.OrbitShader
{
	class ArcVertices
	{
		public IEnumerable<float> Vertices { get; private set; }
		public Vector2 Center { get; private set; }
		public float Radius { get; private set; }

		public ArcVertices(IEnumerable<float> vertices, Vector2 center, float radius)
		{
			this.Vertices = vertices;
			this.Center = center;
			this.Radius = radius;
		}
	}
}
