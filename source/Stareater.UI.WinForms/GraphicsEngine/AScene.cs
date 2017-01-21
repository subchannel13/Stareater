using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using OpenTK;
using Stareater.GLData;
using Stareater.GLRenderers;

namespace Stareater.GraphicsEngine
{
	abstract class AScene
	{
		protected const int NoCallList = -1;

		private HashSet<SceneObject> children = new HashSet<SceneObject>();
		private HashSet<float> dirtyLayers = new HashSet<float>();
		private Dictionary<float, List<IDrawable>> drawables = new Dictionary<float, List<IDrawable>>();
		private Dictionary<float, List<VertexArray>> Vaos = new Dictionary<float, List<VertexArray>>();
		
		public void Draw(double deltaTime)
		{
			this.FrameUpdate(deltaTime);
			
			if (this.dirtyLayers.Count > 0)
				this.setupDrawables();
			
			foreach(var drawable in this.drawables.OrderByDescending(x => x.Key).SelectMany(x => x.Value))
				drawable.Draw(this.projection);
		}
		
		protected abstract void FrameUpdate(double deltaTime);
		
		#region Scene events
		public virtual void Activate()
		{ }
		
		public virtual void Deactivate()
		{ }
		
		public void ResetProjection(float screenWidth, float screenHeigth, float canvasWidth, float canvasHeigth)
		{
			this.canvasSize = new Vector2(canvasWidth, canvasHeigth);
			this.screenSize = new Vector2(screenWidth, screenHeigth);
			this.setupPerspective();
		}
		#endregion
		
		#region Input handling
		public virtual void OnKeyPress(System.Windows.Forms.KeyPressEventArgs e)
		{ }
		
		public virtual void OnMouseDoubleClick(MouseEventArgs e)
		{ }
		
		public virtual void OnMouseClick(MouseEventArgs e)
		{ }
		
		public virtual void OnMouseMove(MouseEventArgs e)
		{ }

		public virtual void OnMouseScroll(MouseEventArgs e)
		{ }
		#endregion
		
		#region Scene objects
		protected void Add(SceneObject sceneObject)
		{
			this.children.Add(sceneObject);
			this.dirtyLayers.UnionWith(sceneObject.RenderData.Select(x => x.Z));
		}

		protected void Remove(SceneObject sceneObject)
		{
			this.children.Remove(sceneObject);
			this.dirtyLayers.UnionWith(sceneObject.RenderData.Select(x => x.Z));
		}

		protected void Update(ref SceneObject oldObject, SceneObject newObject)
		{
			if (oldObject != null)
				this.Remove(oldObject);
			this.Add(newObject);
			oldObject = newObject;
		}
		
		protected void ClearScene()
		{
			this.children.Clear();
			this.dirtyLayers.UnionWith(this.drawables.Keys);
		}
		#endregion
		
		#region Perspective and viewport calculation
		protected Vector2 canvasSize { get; private set; }
		protected Vector2 screenSize { get; private set; }
		protected Matrix4 invProjection { get; private set; }
		protected Matrix4 projection { get; private set; }
		
		protected abstract Matrix4 calculatePerspective();
		
		protected static Matrix4 calcOrthogonalPerspective(float width, float height, float farZ, Vector2 originOffset)
		{
			var left = (float)(-width / 2 + originOffset.X);
			var right = (float)(width / 2 + originOffset.X);
			var bottom = (float)(-height / 2 + originOffset.Y);
			var top = (float)(height / 2 + originOffset.Y);
			
			return new Matrix4(
				2 / (right - left), 0, 0, 0,
				0, 2 / (top - bottom), 0, 0,
				0, 0, 2 / farZ, 0,
				-(right + left) / (right - left), -(top + bottom) / (top - bottom), -1, 1
			);
		}
		
		protected Vector4 mouseToView(int x, int y)
		{
			return new Vector4(
				2 * x / canvasSize.X - 1,
				1 - 2 * y / canvasSize.Y, 
				0, 1
			);
		}
		
		protected void setupPerspective()
		{
			this.projection = this.calculatePerspective();
			this.invProjection = Matrix4.Invert(new Matrix4(this.projection.Row0, this.projection.Row1, this.projection.Row2, this.projection.Row3));
		}
		#endregion

		#region Rendering logic
		private void setupDrawables()
		{
			foreach(var layer in this.dirtyLayers)
			{
				if (this.Vaos.ContainsKey(layer))
				{
					foreach (var vao in this.Vaos[layer])
						vao.Delete();
					this.Vaos[layer].Clear();
				}
				else
					this.Vaos[layer] = new List<VertexArray>();
				this.drawables[layer] = new List<IDrawable>();
				
				var vaoBuilders = new Dictionary<AGlProgram, VertexArrayBuilder>();
				var drawableData = new List<PolygonData>();
				foreach(var polygon in this.children.SelectMany(x => x.RenderData).Where(x => Math.Abs(x.Z - layer) < 1e-3))
				{
					if (!vaoBuilders.ContainsKey(polygon.ShaderData.ForProgram))
						vaoBuilders[polygon.ShaderData.ForProgram] = new VertexArrayBuilder();
					
					var builder = vaoBuilders[polygon.ShaderData.ForProgram];
					builder.BeginObject();
					builder.Add(polygon.VertexData, polygon.ShaderData.VertexDataSize);
					builder.EndObject();
					
					drawableData.Add(polygon);
				}
				
				var vaos = new Dictionary<AGlProgram, VertexArray>();
				foreach (var builder in vaoBuilders)
				{
					var vao = builder.Value.Generate(builder.Key);
					vaos.Add(builder.Key, vao);
					this.Vaos[layer].Add(vao);
				}
				
				for (int i = 0; i < drawableData.Count; i++)
				{
					var data = drawableData[i];
					this.drawables[data.Z].Add(data.ShaderData.MakeDrawable(vaos[data.ShaderData.ForProgram], i));
				}
			}
			this.dirtyLayers.Clear();
		}
		#endregion
	}
}
