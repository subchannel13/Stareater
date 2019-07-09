using Stareater.GraphicsEngine;
using OpenTK.Graphics;
using OpenTK;
using System.Drawing;

namespace Stareater.GLData.SpriteShader
{
	class SpriteData : IShaderData
	{
		public int TextureId { get; private set; }
		public Color4 Color { get; set; }
		public Matrix4 LocalTransform { get; set; }
		public ClipWindow ClipArea { get; private set; }

		public SpriteData(Matrix4 localTransform, int textureId, Color color, ClipWindow clipArea)
		{
			this.LocalTransform = localTransform;
			this.TextureId = textureId;
			this.Color = new Color4(color.R, color.G, color.B, color.A);
			this.ClipArea = clipArea;
		}

		#region IShaderUniformData implementation
		public float Alpha
		{
			get { return this.Color.A; }
			set { this.Color = new Color4(this.Color.R, this.Color.G, this.Color.B, value); }
		}

		public AGlProgram ForProgram 
		{
			get { return ShaderLibrary.Sprite; }
		}

		public int VertexDataSize 
		{ 
			get { return SpriteGlProgram.VertexSize; }
		}
			
		public IDrawable MakeDrawable(VertexArray vao, int objectIndex)
		{
			return new SpriteDrawable(vao, objectIndex, this);
		}
		#endregion
	}
}
