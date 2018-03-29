using System.Collections.Generic;
using System.Linq;

namespace Stareater.GraphicsEngine
{
	class SceneObject
	{
		public IEnumerable<PolygonData> RenderData { get; private set; }
		public PhysicalData PhysicalShape { get; private set; }
		public object Data { get; private set; }
		public IAnimator Animator { get; private set; }

		public SceneObject(PolygonData[] renderData, PhysicalData shape = null, object data = null, IAnimator animator = null)
		{
			this.Data = data;
			this.PhysicalShape = shape;
			this.RenderData = renderData;
			this.Animator = animator;
		}
		
		public SceneObject(PolygonData renderData, PhysicalData shape = null, object data = null, IAnimator animator = null) :
			this(new [] { renderData }, shape, data, animator)
		{ }
		
		public SceneObject(IEnumerable<PolygonData> renderData, PhysicalData shape = null, object data = null, IAnimator animator = null) :
			this(renderData.ToArray() , shape, data, animator)
		{ }
	}
}
