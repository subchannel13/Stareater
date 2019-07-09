using OpenTK;
using OpenTK.Graphics;
using Stareater.GraphicsEngine;
using System;
using System.Drawing;

namespace Stareater.GLData.SdfShader
{
	class SdfData : IShaderData
	{
		public int TextureId { get; private set; }
		public Color4 Color { get; set; }
		public Matrix4 LocalTransform { get; set; }
		public ClipWindow ClipArea { get; private set; }

		public SdfData(Matrix4 localTransform, int textureId, Color color, ClipWindow clipArea)
		{
			this.LocalTransform = localTransform;
			this.TextureId = textureId;
			this.Color = new Color4(color.R, color.G, color.B, color.A);
			this.ClipArea = clipArea;
		}

		public float Alpha
		{
			get { return this.Color.A; }
			set { this.Color = new Color4(this.Color.R, this.Color.G, this.Color.B, value); }
		}

		public AGlProgram ForProgram => ShaderLibrary.Sdf;

		public int VertexDataSize => SdfGlProgram.VertexSize;

		public IDrawable MakeDrawable(VertexArray vao, int objectIndex)
		{
			return new SdfDrawable(vao, objectIndex, this);
		}
	}
}
