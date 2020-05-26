using OpenTK;
using Stareater.GLData;
using Stareater.GLData.SpriteShader;
using System.Collections.Generic;
using System.Drawing;

namespace Stareater.GraphicsEngine.GuiElements
{
	class GuiPanel : AGuiElement
	{
		private readonly List<AGuiElement> children = new List<AGuiElement>();

		public GuiPanel()
		{
			this.MaskMouseClick = true;
		}

		public void AddChild(AGuiElement child)
		{
			this.children.Add(child);
		}

		public void Clear()
		{
			this.children.Clear();
		}

		public bool Empty => this.children.Count == 0;

		private BackgroundTexture mBackground = null;

		public BackgroundTexture Background
		{
			get { return this.mBackground; }
			set
			{
				this.apply(ref this.mBackground, value);
			}
		}

		public bool MaskMouseClick { get; set; }

		public override void Attach(AScene scene, AGuiElement parent)
		{
			base.Attach(scene, parent);

			foreach(var child in children)
				scene.AddElement(child, this);
		}

		public override bool OnMouseDown(Vector2 mousePosition)
		{
			return this.MaskMouseClick;
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
	}
}
