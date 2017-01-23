using System;
using OpenTK;

namespace Stareater.GraphicsEngine
{
	class PhysicalData
	{
		public Vector2 Center { get; private set; }
		public Vector2 Size { get; private set; }
		
		public PhysicalData(Vector2 center, Vector2 size)
		{
			this.Center = center;
			this.Size = size;
		}
	}
}
