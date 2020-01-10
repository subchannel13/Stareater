using OpenTK;
using Stareater.GraphicsEngine.GuiPositioners;
using System;
using System.Linq;
using System.Windows.Forms;

namespace Stareater.GraphicsEngine.GuiElements
{
	abstract class AGuiElement : IGuispaceElement
	{
		private SceneObject graphicObject = null;
		
		protected AScene scene { get; private set; }
		public float Z0 { get; private set; }
		public float ZRange { get; private set; }

		public AGuiElement Parent { get; private set; }
		public AGuiElement Below { get; set; }
		public ElementPosition Position { get; private set; }
		public bool HandlesMouse { get; set; }
		public ITooltip Tooltip { get; set; }

		protected AGuiElement()
		{
			this.Position = new ElementPosition(this.measureContent);
			this.HandlesMouse = true;
			this.Position.OnReposition += this.updateScene;
		}

		public virtual void Attach(AScene scene, AGuiElement parent)
		{
			this.scene = scene;
			this.Parent = parent;

			this.Position.Attach(parent);
			this.updateScene();
		}

		public virtual void Detach()
		{
			this.Position.Detach();
			this.scene.RemoveFromScene(ref this.graphicObject);
			this.scene = null;
		}

		public void SetDepth(float z0, float zRange)
		{
			this.Z0 = z0;
			this.ZRange = zRange;

			this.updateScene();
		}

		public Vector2 Margins 
		{
			get => this.Position.Margins;
			set => this.Position.Margins = value;
		}

		public bool IsShown => this.scene != null;

		public virtual bool OnMouseDown(Vector2 mousePosition)
		{
			return false;
		}

		public virtual void OnMouseUp(Keys modiferKeys)
		{
			//No operation
		}

		public virtual void OnMouseDownCanceled()
		{
			//No operation
		}

		public virtual void OnMouseMove(Vector2 mousePosition, Keys modiferKeys)
		{
			//No operation
		}

		public virtual void OnMouseDrag(Vector2 mousePosition)
		{
			//No operation
		}

		public virtual void OnMouseLeave()
		{
			//No operation
		}

		public virtual void OnMouseScroll(Vector2 mousePosition, int delta)
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

			if (this.Position.ClipArea.IsEmpty)
				return;

			var sceneObject = this.makeSceneObject();

			if (sceneObject != null)
				this.scene.UpdateScene(ref this.graphicObject, sceneObject);
		}

		protected bool apply<T>(ref T state, T newValue)
		{
			var oldValue = state;
			state = newValue;

			if (!object.Equals(oldValue, newValue))
			{
				this.updateScene();
				return true;
			}

			return false;
		}

		protected void reposition()
		{
			if (this.scene == null)
				return;

			this.Position.Recalculate();
		}

		protected abstract SceneObject makeSceneObject();

		protected virtual Vector2 measureContent()
		{
			var children = this.scene.ElementChildren(this);

			if (!children.Any())
				return new Vector2();

			foreach (var child in children)
				child.Position.Recalculate();

			return new Vector2(
				children.Max(x => x.Position.Center.X + x.Position.Size.X / 2) - children.Min(x => x.Position.Center.X - x.Position.Size.X / 2),
				children.Max(x => x.Position.Center.Y + x.Position.Size.Y / 2) - children.Min(x => x.Position.Center.Y - x.Position.Size.Y / 2)
			);
		}
	}
}
