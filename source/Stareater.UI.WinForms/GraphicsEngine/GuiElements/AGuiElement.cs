using OpenTK;
using System;
using System.Collections.Generic;

namespace Stareater.GraphicsEngine.GuiElements
{
	abstract class AGuiElement
	{
		private AScene scene;
		private SceneObject graphicObject = null;
		private HashSet<AGuiElement> dependentElements = new HashSet<AGuiElement>();

		public float Z0 { get; private set; }
		public float ZRange { get; private set; }

		public AGuiElement Parent { get; private set; }
		public ElementPosition Position { get; private set; }

		protected AGuiElement()
		{
			this.Position = new ElementPosition(this.contentWidth, this.contentHeight);
		}

		public virtual void Attach(AScene scene, AGuiElement parent)
		{
			this.scene = scene;
			this.Parent = parent;

			this.Parent.dependentElements.Add(this);
			foreach(var element in this.Position.Dependencies)
				element.dependentElements.Add(this);

			this.SetDepth(parent.Z0 - parent.ZRange, parent.ZRange);
			this.updateScene();
		}

		public void Detach()
		{
			this.Parent.dependentElements.Remove(this);
			foreach (var element in this.Position.Dependencies)
				element.dependentElements.Remove(this);

			this.scene.RemoveFromScene(ref this.graphicObject);
			this.scene = null;
		}

		public void SetDepth(float z0, float zRange)
		{
			this.Z0 = z0;
			this.ZRange = zRange;

			this.updateScene();
		}

		public void RecalculatePosition()
		{
			this.Position.Recalculate((this.Parent != null) ? this.Parent.Position : null);

			foreach (var element in this.dependentElements)
				element.RecalculatePosition();

			this.updateScene();
		}

		public virtual bool OnMouseDown(Vector2 mousePosition)
		{
			return false;
		}

		public virtual bool OnMouseUp(Vector2 mousePosition)
		{
			return false;
		}

		public virtual void OnMouseMove(Vector2 mousePosition)
		{
			//No operation
		}

		protected void updateScene()
		{
			if (this.scene == null)
				return;

			if (this.graphicObject != null && this.Position.ClipArea.IsEmpty)
			{
				this.scene.RemoveFromScene(this.graphicObject);
				this.graphicObject = null;
				return;
			}

			var sceneObject = this.makeSceneObject();

			if (sceneObject != null)
				this.scene.UpdateScene(ref this.graphicObject, sceneObject);
		}

		protected void apply<T>(ref T state, T newValue)
		{
			var oldValue = state;
			state = newValue;

			if (!object.Equals(oldValue, newValue))
				this.updateScene();
		}

		protected bool isInside(Vector2 point)
		{
			var innerPoint = point - this.Position.Center;
			return Math.Abs(innerPoint.X) <= this.Position.Size.X / 2 &&
				Math.Abs(innerPoint.Y) <= this.Position.Size.Y / 2;
		}

		protected void reposition()
		{
			if (this.scene == null)
				return;

			this.RecalculatePosition();
		}

		protected abstract SceneObject makeSceneObject();

		protected virtual float contentWidth()
		{
			return 0;
		}

		protected virtual float contentHeight()
		{
			return 0;
		}
	}
}
