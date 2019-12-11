using OpenTK;
using Stareater.GLData;
using Stareater.GLData.SpriteShader;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Stareater.GraphicsEngine.GuiElements
{
	class ListPanel : AGuiElement
	{
		private readonly GridPositionBuilder positionBuilder;

		public ListPanel(int columns, float elementWidth, float elementHeight, float elementSpacing) : base()
		{
			this.positionBuilder = new GridPositionBuilder(columns, elementWidth, elementHeight, elementSpacing);
		}

		private readonly List<AGuiElement> mChildren = new List<AGuiElement>();
		public IEnumerable<AGuiElement> Children
		{
			private get => this.mChildren;
			set
			{
				if (scene != null)
					foreach (var child in this.mChildren)
						scene.RemoveElement(child);

				this.mChildren.Clear();
				this.positionBuilder.Restart();

				foreach (var child in value)
				{
					this.mChildren.Add(child);
					child.Position.ParentRelative(-1, 1).WithMargins(this.mPadding, this.mPadding);
					this.positionBuilder.Add(child.Position);
				}

				if (scene != null)
					foreach (var child in mChildren)
						scene.AddElement(child, this);
			}
		}

		private BackgroundTexture mBackground = null;
		public BackgroundTexture Background
		{
			get { return this.mBackground; }
			set
			{
				this.apply(ref this.mBackground, value);
			}
		}

		private float mPadding = 0;
		public float Padding
		{
			set
			{
				this.apply(ref this.mPadding, value);
			}
		}

		public override void Attach(AScene scene, AGuiElement parent)
		{
			base.Attach(scene, parent);

			foreach (var child in mChildren)
				scene.AddElement(child, this);
		}

		public override bool OnMouseDown(Vector2 mousePosition)
		{
			return true;
		}

		protected override SceneObject makeSceneObject()
		{
			if (this.mBackground != null)
				return new SceneObjectBuilder().
					Clip(this.Position.ClipArea).
					StartSprite(this.Z0, this.mBackground.Sprite.Id, Color.White).
					Translate(this.Position.Center).
					AddVertices(SpriteHelpers.GuiBackground(this.mBackground, this.Position.Size.X, this.Position.Size.Y)).
					Build();
			else
				return null;
		}

		protected override Vector2 measureContent()
		{
			foreach (var child in this.mChildren)
				child.Position.Recalculate();

			return new Vector2(
				this.mChildren.Max(x => x.Position.Center.X + x.Position.Size.X / 2) - this.mChildren.Min(x => x.Position.Center.X - x.Position.Size.X / 2),
				this.mChildren.Max(x => x.Position.Center.Y + x.Position.Size.Y / 2) - this.mChildren.Min(x => x.Position.Center.Y - x.Position.Size.Y / 2)
			);
		}
	}
}
