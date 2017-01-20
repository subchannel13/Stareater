using System;
using System.Collections.Generic;
using OpenTK;

namespace Stareater.GraphicsEngine
{
	class SceneObject
	{
		public IEnumerable<PolygonData> Children { get; private set; }
		
		public SceneObject(IEnumerable<PolygonData> children)
		{
			this.Children = children;
		}
	}
}
