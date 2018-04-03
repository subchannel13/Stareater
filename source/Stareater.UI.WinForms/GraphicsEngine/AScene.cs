using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Stareater.Utils.Collections;
using Stareater.GLData;
using Stareater.GraphicsEngine.GuiElements;
using Stareater.AppData;
using Stareater.Utils;

namespace Stareater.GraphicsEngine
{
	abstract class AScene
	{
		private HashSet<SceneObject> sceneObjects = new HashSet<SceneObject>();
		private QuadTree<SceneObject> physicalObjects = new QuadTree<SceneObject>();
		private HashSet<IAnimator> animators = new HashSet<IAnimator>();
		private HashSet<float> dirtyLayers = new HashSet<float>();
		private Dictionary<float, List<IDrawable>> drawables = new Dictionary<float, List<IDrawable>>();
		private Dictionary<float, List<VertexArray>> Vaos = new Dictionary<float, List<VertexArray>>();

		private HashSet<AGuiElement> guiElements = new HashSet<AGuiElement>();
		
		public void Draw(double deltaTime)
		{
			foreach (var animator in this.animators)
				animator.OnUpdate(deltaTime);
			
			this.FrameUpdate(deltaTime);
			
			if (this.dirtyLayers.Count > 0)
				this.setupDrawables();

			var guiLayer = this.GuiLayerThickness;

			foreach (var layer in this.drawables.OrderByDescending(x => x.Key))
			{
				var view = layer.Key > guiLayer ? this.projection : this.guiProjection;

				foreach (var drawable in layer.Value)
					drawable.Draw(view, layer.Key);
			}
		}
		
		protected virtual void FrameUpdate(double deltaTime)
		{
			//no operation
		}
		
		#region Scene events
		public virtual void Activate()
		{ }
		
		public virtual void Deactivate()
		{ }
		
		public void ResetProjection(Control canvas)
		{
			var screen = Screen.FromControl(canvas);
			GL.Viewport(canvas.ClientRectangle);
			
			this.canvasSize = new Vector2(canvas.Width, canvas.Height);
			this.screenSize = new Vector2(screen.Bounds.Width, screen.Bounds.Height);
			this.setupPerspective();
		}
		#endregion

		#region Input handling
		public void HandleKeyPress(System.Windows.Forms.KeyPressEventArgs e)
		{
			this.onKeyPress(e.KeyChar);
		}

		public void HandleMouseDoubleClick(MouseEventArgs e)
		{
			//TODO(later) differentiate between left and right click
			this.onMouseDoubleClick(Vector4.Transform(this.mouseToView(e.X, e.Y), this.invProjection).Xy);
		}

		public void HandleMouseDown(MouseEventArgs e)
		{
			//TODO(later) differentiate between left and right click
			var mouseGuiPoint = Vector4.Transform(this.mouseToView(e.X, e.Y), this.guiInvProjection).Xy;

			foreach (var element in this.guiElements)
				if (element.OnMouseDown(mouseGuiPoint))
					return;
		}

		public void HandleMouseUp(MouseEventArgs e)
		{
			//TODO(later) differentiate between left and right click
			var mouseGuiPoint = Vector4.Transform(this.mouseToView(e.X, e.Y), this.guiInvProjection).Xy;

			foreach (var element in this.guiElements)
				if (element.OnMouseUp(mouseGuiPoint))
					return;

			this.onMouseClick(Vector4.Transform(this.mouseToView(e.X, e.Y), this.invProjection).Xy);
		}

		public void HandleMouseMove(MouseEventArgs e)
		{
			var mouseGuiPoint = Vector4.Transform(this.mouseToView(e.X, e.Y), this.guiInvProjection).Xy;

			foreach (var element in this.guiElements)
				element.OnMouseMove(mouseGuiPoint);

			this.onMouseMove(this.mouseToView(e.X, e.Y), e.Button);
		}

		public void HandleMouseScroll(MouseEventArgs e)
		{
			this.onMouseScroll(Vector4.Transform(this.mouseToView(e.X, e.Y), this.invProjection).Xy, e.Delta);
		}

		protected virtual void onKeyPress(char c)
		{ }

		protected virtual void onMouseDoubleClick(Vector2 mousePoint)
		{ }

		protected virtual void onMouseClick(Vector2 mousePoint)
		{ }

		protected virtual void onMouseMove(Vector4 mouseViewPosition, MouseButtons mouseClicks)
		{ }

		protected virtual void onMouseScroll(Vector2 mousePoint, int delta)
		{ }
		#endregion

		#region Scene objects
		public void RemoveFromScene(SceneObject sceneObject)
		{
			this.sceneObjects.Remove(sceneObject);
			this.dirtyLayers.UnionWith(sceneObject.RenderData.Select(x => x.Z));
			
			if (sceneObject.PhysicalShape != null)
				this.physicalObjects.Remove(sceneObject);

			if (sceneObject.Animator != null)
				this.animators.Remove(sceneObject.Animator);
		}

		public void RemoveFromScene(ref SceneObject sceneObject)
		{
			this.RemoveFromScene(sceneObject);
			sceneObject = null;
		}

		public void UpdateScene(ref SceneObject oldObject, SceneObject newObject)
		{
			if (oldObject != null)
				this.RemoveFromScene(oldObject);
			
			this.addToScene(newObject);
			oldObject = newObject;
		}

		public void UpdateScene(ref IEnumerable<SceneObject> oldObjects, ICollection<SceneObject> newObjects)
		{
			if (oldObjects != null)
				foreach(var obj in oldObjects)
					this.RemoveFromScene(obj);
			
			foreach(var obj in newObjects)
				this.addToScene(obj);
			oldObjects = newObjects;
		}
		
		protected IEnumerable<SceneObject> QueryScene(NGenerics.DataStructures.Mathematical.Vector2D center)
		{
			return this.physicalObjects.Query(center, new NGenerics.DataStructures.Mathematical.Vector2D(0, 0));
		}
		
		protected IEnumerable<SceneObject> QueryScene(NGenerics.DataStructures.Mathematical.Vector2D center, double radius)
		{
			return this.physicalObjects.Query(center, new NGenerics.DataStructures.Mathematical.Vector2D(radius, radius));
		}

		private void addToScene(SceneObject sceneObject)
		{
			this.sceneObjects.Add(sceneObject);
			this.dirtyLayers.UnionWith(sceneObject.RenderData.Select(x => x.Z));

			if (sceneObject.PhysicalShape != null)
				this.physicalObjects.Add(
					sceneObject,
					sceneObject.PhysicalShape.Center.X, sceneObject.PhysicalShape.Center.Y,
					sceneObject.PhysicalShape.Size.X, sceneObject.PhysicalShape.Size.Y
				);

			if (sceneObject.Animator != null)
				this.animators.Add(sceneObject.Animator);
		}
		#endregion

		#region Perspective and viewport calculation
		protected Vector2 canvasSize { get; private set; }
		protected Vector2 screenSize { get; private set; }
		protected Matrix4 invProjection { get; private set; }
		protected Matrix4 projection { get; private set; }
		private Matrix4 guiProjection;
		private Matrix4 guiInvProjection;

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

		protected bool isVisible(Vector2 point)
		{
			var viewPoint = convert(Vector4.Transform(new Vector4(point.X, point.Y, 0, 1), this.projection).Xy);

			return Methods.IsRectEnveloped(
				new NGenerics.DataStructures.Mathematical.Vector2D(1, 1),
				new NGenerics.DataStructures.Mathematical.Vector2D(-1, -1),
				viewPoint,
				viewPoint
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

			var width = canvasSize.X / SettingsWinforms.Get.GuiScale;
			var height = canvasSize.Y / SettingsWinforms.Get.GuiScale;

			this.guiProjection = calcOrthogonalPerspective(width, height, 1, new Vector2());
			this.guiInvProjection = Matrix4.Invert(new Matrix4(this.guiProjection.Row0, this.guiProjection.Row1, this.guiProjection.Row2, this.guiProjection.Row3));
			foreach (var element in this.guiElements)
				element.RecalculatePosition(width / 2, height / 2);
		}
		#endregion

		#region GUI
		protected abstract float GuiLayerThickness { get; }

		protected void AddElement(AGuiElement element)
		{
			this.guiElements.Add(element);
			element.Attach(this, this.GuiLayerThickness);
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
				foreach(var polygon in this.sceneObjects.SelectMany(x => x.RenderData).Where(x => x.Z == layer))
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
					//TODO(later) wrong index when multiple programs draw in same "z" layer
					var data = drawableData[i];
					this.drawables[data.Z].Add(data.MakeDrawable(vaos[data.ShaderData.ForProgram], i)); 
				}
			}
			this.dirtyLayers.Clear();
		}
		#endregion

		#region Math helpers
		protected static Vector2 convert(NGenerics.DataStructures.Mathematical.Vector2D v)
		{
			return new Vector2((float)v.X, (float)v.Y);
		}

		protected static NGenerics.DataStructures.Mathematical.Vector2D convert(Vector2 v)
		{
			return new NGenerics.DataStructures.Mathematical.Vector2D(v.X, v.Y);
		}
		#endregion
	}
}
