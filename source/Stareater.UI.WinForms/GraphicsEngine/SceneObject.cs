using System;
using System.Collections.Generic;
using OpenTK;

namespace Stareater.GraphicsEngine
{
	class SceneObject
	{
		public IEnumerable<PolygonData> RenderData { get; private set; }
		public PhysicalData PhysicalShape { get; private set; }
		public object Data { get; private set; }
		
		public SceneObject(IEnumerable<PolygonData> renderData, PhysicalData shape = null, object data = null)
		{
			this.Data = data;
			this.PhysicalShape = shape;
			this.RenderData = renderData;
		}
		
		public SceneObject(PolygonData renderData, PhysicalData shape = null, object data = null) :
			this(new [] { renderData }, shape, data)
		{ }
	}
}
