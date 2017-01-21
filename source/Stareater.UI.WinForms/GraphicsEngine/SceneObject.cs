using System;
using System.Collections.Generic;
using OpenTK;

namespace Stareater.GraphicsEngine
{
	class SceneObject
	{
		public IEnumerable<PolygonData> RenderData { get; private set; }
		
		public SceneObject(IEnumerable<PolygonData> renderData)
		{
			this.RenderData = renderData;
		}
	}
}
