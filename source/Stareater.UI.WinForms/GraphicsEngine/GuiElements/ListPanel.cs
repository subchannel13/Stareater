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
		private readonly List<AGuiElement> children = new List<AGuiElement>();

		public ListPanel(int columns, float elementWidth, float elementHeight, float elementSpacing) : base()
		{
			this.positionBuilder = new GridPositionBuilder(columns, elementWidth, elementHeight, elementSpacing);
		}

		public void AddChild(AGuiElement child)
		{
			this.children.Add(child);
			this.positionBuilder.Add(child.Position);
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

		public override void Attach(AScene scene, AGuiElement parent)
		{
			base.Attach(scene, parent);

			foreach (var child in children)
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
			foreach (var child in this.children)
				child.Position.Recalculate();

			return new Vector2(
				this.children.Max(x => x.Position.Center.X + x.Position.Size.X / 2) - this.children.Min(x => x.Position.Center.X - x.Position.Size.X / 2),
				this.children.Max(x => x.Position.Center.Y + x.Position.Size.Y / 2) - this.children.Min(x => x.Position.Center.Y - x.Position.Size.Y / 2)
			);
		}
	}
}
