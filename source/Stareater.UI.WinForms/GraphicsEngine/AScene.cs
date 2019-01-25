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
using System;

namespace Stareater.GraphicsEngine
{
	abstract class AScene
	{
		private readonly HashSet<SceneObject> sceneObjects = new HashSet<SceneObject>();
		private readonly QuadTree<SceneObject> physicalObjects = new QuadTree<SceneObject>();
		private readonly HashSet<IAnimator> animators = new HashSet<IAnimator>();
		private readonly HashSet<float> dirtyLayers = new HashSet<float>();
		private readonly Dictionary<float, List<IDrawable>> drawables = new Dictionary<float, List<IDrawable>>();
		private readonly Dictionary<float, List<VertexArray>> layerVaos = new Dictionary<float, List<VertexArray>>();

		protected Vector2 canvasSize { get; private set; }
		protected Vector2 screenSize { get; private set; }
		protected Matrix4 invProjection { get; private set; }
		protected Matrix4 projection { get; private set; }
		private Matrix4 guiProjection;
		private Matrix4 guiInvProjection;

		private readonly Dictionary<AGuiElement, HashSet<AGuiElement>> guiHierarchy = new Dictionary<AGuiElement, HashSet<AGuiElement>>();
		private readonly AGuiElement rootParent;
		private AGuiElement mouseHovered;
		private readonly Dictionary<MouseButtons, AGuiElement> mousePressed = new Dictionary<MouseButtons, AGuiElement>();

		protected AScene()
		{
			this.rootParent = new GuiPanel();
			this.rootParent.Position.FixedCenter(0, 0);
			this.rootParent.SetDepth(this.guiLayerThickness, this.guiLayerThickness);

			this.mouseHovered = this.rootParent;
		}

		public void Draw(double deltaTime)
		{
			foreach (var animator in this.animators)
				animator.OnUpdate(deltaTime);
			
			this.frameUpdate(deltaTime);
			
			if (this.dirtyLayers.Count > 0)
				this.setupDrawables();

			var guiLayer = this.guiLayerThickness;
			var viewportTransform = 
				Matrix4.CreateTranslation(1, 1, 0) * 
				Matrix4.CreateScale(canvasSize.X / 2, canvasSize.Y / 2, 1);

			foreach (var layer in this.drawables.OrderByDescending(x => x.Key))
			{
				var view = layer.Key > guiLayer ? this.projection : this.guiProjection;

				foreach (var drawable in layer.Value)
					drawable.Draw(view, layer.Key, viewportTransform);
			}
		}
		
		protected virtual void frameUpdate(double deltaTime)
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
			if (canvas.Width == 0 || canvas.Height == 0)
				return;

			var screen = Screen.FromControl(canvas);
			GL.Viewport(canvas.ClientRectangle);
			
			this.canvasSize = new Vector2(canvas.Width, canvas.Height);
			this.screenSize = new Vector2(screen.Bounds.Width, screen.Bounds.Height);
			this.setupPerspective();
			this.onResize();
		}
		#endregion

		#region Input handling
		public void HandleKeyPress(System.Windows.Forms.KeyPressEventArgs e)
		{
			this.onKeyPress(e.KeyChar);
		}

		public void HandleMouseDoubleClick(MouseEventArgs e)
		{
			//TODO(v0.8) differentiate between left and right click
			this.onMouseDoubleClick(Vector4.Transform(this.mouseToView(e.X, e.Y), this.invProjection).Xy);
		}

		public void HandleMouseDown(MouseEventArgs e)
		{
			var mouseGuiPoint = Vector4.Transform(this.mouseToView(e.X, e.Y), this.guiInvProjection).Xy;
			this.mousePressed[e.Button] = this.rootParent;

			var handler = this.eventHandlerSearch(mouseGuiPoint).FirstOrDefault();
			//TODO(v0.8) differentiate between left and right click
			if (handler != null && handler.OnMouseDown(mouseGuiPoint))
				this.mousePressed[e.Button] = handler;
		}

		public void HandleMouseUp(MouseEventArgs e)
		{
			if (!this.mousePressed.ContainsKey(e.Button))
				this.mousePressed[e.Button] = this.rootParent;

			var mouseGuiPoint = Vector4.Transform(this.mouseToView(e.X, e.Y), this.guiInvProjection).Xy;
			var handler = this.eventHandlerSearch(mouseGuiPoint).FirstOrDefault();

			if (handler != null)
				if (this.mousePressed[e.Button] == handler)
					handler.OnMouseUp(); //TODO(v0.8) differentiate between left and right click
				else
					handler.OnMouseDownCanceled(); //TODO(v0.8) differentiate between left and right click
			else
			{
				this.onMouseClick(Vector4.Transform(this.mouseToView(e.X, e.Y), this.invProjection).Xy);
			}

			this.mousePressed[e.Button] = null;
		}

		public void HandleMouseMove(MouseEventArgs e)
		{
			if (this.mousePressed.Values.Any(x => x != null))
			{
				handleMouseDrag(e);
				return;
			}

			var mouseGuiPoint = Vector4.Transform(this.mouseToView(e.X, e.Y), this.guiInvProjection).Xy;
			AGuiElement handler = this.eventHandlerSearch(mouseGuiPoint).FirstOrDefault();

			if (handler != null && handler != this.rootParent)
				handler.OnMouseMove(mouseGuiPoint);
			else
			{
				this.onMouseMove(this.mouseToView(e.X, e.Y));
				handler = this.rootParent;
			}

			if (this.mouseHovered != handler)
			{
				if (this.mouseHovered != null)
					if (this.mouseHovered != this.rootParent)
						this.mouseHovered.OnMouseLeave();
					else
						this.onMouseLeave();

				this.mouseHovered = handler;
			}
		}

		//TODO(v0.8) pass last mouse position to handlers
		private void handleMouseDrag(MouseEventArgs e)
		{
			var mouseGuiPoint = Vector4.Transform(this.mouseToView(e.X, e.Y), this.guiInvProjection).Xy;

			foreach (var element in this.mousePressed.Values.Where(x => x != null))
				if (element != this.rootParent)
					element.OnMouseDrag(mouseGuiPoint);
				else
					this.onMouseDrag(this.mouseToView(e.X, e.Y));
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

		protected virtual void onMouseMove(Vector4 mouseViewPosition)
		{ }

		protected virtual void onMouseLeave()
		{ }

		protected virtual void onMouseDrag(Vector4 mouseViewPosition)
		{ }

		protected virtual void onMouseScroll(Vector2 mousePoint, int delta)
		{ }
		#endregion

		#region Scene objects
		public void RemoveFromScene(SceneObject sceneObject)
		{
			if (sceneObject == null)
				return;

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

		public void RemoveFromScene(ref IEnumerable<SceneObject> sceneObject)
		{
			if (sceneObject != null)
				foreach (var obj in sceneObject)
					this.RemoveFromScene(obj);
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
		
		protected IEnumerable<SceneObject> queryScene(Vector2D center)
		{
			return this.physicalObjects.Query(center, new Vector2D(0, 0));
		}
		
		protected IEnumerable<SceneObject> queryScene(Vector2D center, double radius)
		{
			return this.physicalObjects.Query(center, new Vector2D(radius, radius));
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
		protected abstract Matrix4 calculatePerspective();

		protected virtual void onResize()
		{ }

		protected static Matrix4 calcOrthogonalPerspective(float width, float height, float farZ, Vector2 originOffset)
		{
			var left = (-width / 2 + originOffset.X);
			var right = (width / 2 + originOffset.X);
			var bottom = (-height / 2 + originOffset.Y);
			var top = (height / 2 + originOffset.Y);
			
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
				new Vector2D(1, 1),
				new Vector2D(-1, -1),
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
			this.rootParent.Position.FixedSize(width, height);
			this.rootParent.RecalculatePosition();
		}
		#endregion

		#region GUI
		protected abstract float guiLayerThickness { get; }

		public void AddElement(AGuiElement element)
		{
			this.AddElement(element, this.rootParent);
		}

		public void AddElement(AGuiElement element, AGuiElement parent)
		{
			if (!this.guiHierarchy.ContainsKey(parent))
				this.guiHierarchy[parent] = new HashSet<AGuiElement>();

			this.guiHierarchy[parent].Add(element);
			element.Attach(this, parent);
			this.updateGuiZ(element);
		}

		public void RemoveElement(AGuiElement element)
		{
			if (this.guiHierarchy.ContainsKey(element))
				foreach (var child in this.guiHierarchy[element].ToList())
					this.RemoveElement(child);

			this.guiHierarchy[element.Parent].Remove(element);
			element.Detach();

			if (!this.guiHierarchy[element.Parent].Any())
				this.guiHierarchy.Remove(element.Parent);
		}

		public void HideElement(AGuiElement element)
		{
			if (this.guiHierarchy[element.Parent].Contains(element))
				this.RemoveElement(element);
		}

		public void ShowElement(AGuiElement element)
		{
			if (this.guiHierarchy[element.Parent].Contains(element))
				return;

			this.AddElement(element, element.Parent);
			element.RecalculatePosition();
		}

		private IEnumerable<AGuiElement> eventHandlerSearch(Vector2 point)
		{
			if (this.guiHierarchy.Count == 0)
				yield break;

			foreach (var parent in this.guiHierarchy[this.rootParent])
				foreach (var element in this.guiPostfixSearch(parent))
					if (element.Position.ClipArea.Contains(point))
						yield return element;
		}

		private IEnumerable<AGuiElement> guiPostfixSearch(AGuiElement parent)
		{
			if (this.guiHierarchy.ContainsKey(parent))
				foreach (var child in this.guiHierarchy[parent])
					foreach (var element in guiPostfixSearch(child))
						yield return element;

			yield return parent;
		}

		//TODO(v0.8) check users
		private IEnumerable<AGuiElement> guiPrefixSearch()
		{
			if (this.guiHierarchy.Count == 0)
				yield break;

			var parents = new Stack<AGuiElement>();
			parents.Push(this.rootParent);

			while (parents.Any())
			{
				var parent = parents.Pop();
				yield return parent;

				if (this.guiHierarchy.ContainsKey(parent))
					foreach (var element in this.guiHierarchy[parent])
						parents.Push(element);
			}
		}

		private void updateGuiZ(AGuiElement element)
		{
			var layers = 1;
			var root = element;

			while (root.Parent != this.rootParent)
			{
				root = root.Parent;
				layers++;
			}

			var zRange = this.guiLayerThickness / layers;
			if (root.ZRange <= zRange && root.Z0 == this.guiLayerThickness)
				return;

			root.SetDepth(this.guiLayerThickness, zRange);
			var subtrees = new Queue<AGuiElement>();
			subtrees.Enqueue(root);

			while (subtrees.Count > 0)
			{
				root = subtrees.Dequeue();

				if (this.guiHierarchy.ContainsKey(root))
					foreach (var item in this.guiHierarchy[root])
					{
						item.SetDepth(root.Z0 - root.ZRange, root.ZRange);
						subtrees.Enqueue(item);
					}
			}
		}
		#endregion

		#region Rendering logic
		private void setupDrawables()
		{
			//TODO(v0.8) clip child UI elements
			foreach(var layer in this.dirtyLayers)
			{
				if (this.layerVaos.ContainsKey(layer))
				{
					foreach (var vao in this.layerVaos[layer])
						vao.Delete();
					this.layerVaos[layer].Clear();
				}
				else
					this.layerVaos[layer] = new List<VertexArray>();
				this.drawables[layer] = new List<IDrawable>();
				
				var vaoBuilders = new Dictionary<AGlProgram, VertexArrayBuilder>();
				var drawableData = new List<PolygonData>();
				var drawableIndices = new List<int>();
				foreach(var polygon in this.sceneObjects.SelectMany(x => x.RenderData).Where(x => x.Z == layer))
				{
					if (!vaoBuilders.ContainsKey(polygon.ShaderData.ForProgram))
						vaoBuilders[polygon.ShaderData.ForProgram] = new VertexArrayBuilder();
					
					var builder = vaoBuilders[polygon.ShaderData.ForProgram];
					builder.BeginObject();
					builder.Add(polygon.VertexData, polygon.ShaderData.VertexDataSize);
					builder.EndObject();

					drawableData.Add(polygon);
					drawableIndices.Add(builder.Count - 1);
				}
				
				var vaos = new Dictionary<AGlProgram, VertexArray>();
				foreach (var builder in vaoBuilders)
				{
					var vao = builder.Value.Generate(builder.Key);
					vaos.Add(builder.Key, vao);
					this.layerVaos[layer].Add(vao);
				}
				
				for (int i = 0; i < drawableData.Count; i++)
				{
					var vaoI = drawableIndices[i];
					var data = drawableData[vaoI];
					this.drawables[data.Z].Add(data.MakeDrawable(vaos[data.ShaderData.ForProgram], vaoI)); 
				}
			}
			this.dirtyLayers.Clear();
		}
		#endregion

		#region Math helpers
		protected static Vector2 convert(Vector2D v)
		{
			return new Vector2((float)v.X, (float)v.Y);
		}

		protected static Vector2D convert(Vector2 v)
		{
			return new Vector2D(v.X, v.Y);
		}
		#endregion
	}
}
