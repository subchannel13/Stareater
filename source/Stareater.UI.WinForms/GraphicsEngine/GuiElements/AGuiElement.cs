using OpenTK;

namespace Stareater.GraphicsEngine.GuiElements
{
	abstract class AGuiElement
	{
		private AScene scene;
		private SceneObject graphicObject = null;

		protected float z { get; private set; }

		public ElementPosition Position { get; private set; }
		public AGuiElement Parent { get; private set; }

		protected AGuiElement()
		{
			this.Position = new ElementPosition(this.contentWidth, this.contentHeight);
		}

		//TODO(v0.8) redo how z is distributed
		public void Attach(AScene scene, float z, AGuiElement parent)
		{
			this.scene = scene;
			this.z = z;
			this.Parent = parent;

			this.updateScene();
		}

		public void Detach()
		{
			this.scene.RemoveFromScene(ref this.graphicObject);
		}

		public void RecalculatePosition(float parentWidth, float parentHeight)
		{
			this.Position.Recalculate(parentWidth, parentHeight);
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

			var sceneObject = this.makeSceneObject();

			if (sceneObject != null)
				this.scene.UpdateScene(ref this.graphicObject, sceneObject);
		}

		protected void apply<T>(ref T state, T newValue)
		{
			var oldValue = state;
			state = newValue;

			if (oldValue == null || !oldValue.Equals(newValue))
				this.updateScene();
		}

		protected void reposition()
		{
			if (this.scene == null)
				return;

			this.scene.UpdatePosition(this);
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
