using OpenTK;

namespace Stareater.GraphicsEngine.GuiElements
{
	abstract class AGuiElement
	{
		private AScene scene;
		private SceneObject graphicObject = null;

		protected float Z { get; private set; }

		public ElementPosition Position { get; private set; }

		public AGuiElement()
		{
			this.Position = new ElementPosition(this.ContentWidth, this.ContentHeight);
		}

		protected abstract SceneObject MakeSceneObject();

		public void Attach(AScene scene, float z)
		{
			this.scene = scene;
			this.Z = z;

			this.UpdateScene();
		}

		public void RecalculatePosition(float parentWidth, float parentHeight)
		{
			this.Position.Recalculate(parentWidth, parentHeight);
			this.UpdateScene();
		}

		protected void UpdateScene()
		{
			if (this.scene == null)
				return;

			var sceneObject = this.MakeSceneObject();

			if (sceneObject != null)
				this.scene.UpdateScene(ref this.graphicObject, sceneObject);
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

		protected virtual float ContentWidth()
		{
			return 0;
		}

		protected virtual float ContentHeight()
		{
			return 0;
		}

		protected void Apply<T>(ref T state, T newValue)
		{
			var oldValue = state;
			state = newValue;

			if ((oldValue == null && newValue != null) || !oldValue.Equals(newValue))
				this.UpdateScene();
		}
	}
}
