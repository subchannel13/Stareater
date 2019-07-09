using OpenTK;
using OpenTK.Graphics;
using Stareater.GraphicsEngine;

namespace Stareater.GLData.OrbitShader
{
	class OrbitData : IShaderData
	{
		public float MinRadius { get; private set; }
		public float MaxRadius { get; private set; }
		public Color4 Color { get; private set; }
		public Matrix4 LocalTransform { get; set; }
		public int TextureId { get; private set; }
		public Matrix3 TextureTransform { get; private set; }

		public OrbitData(float minRadius, float maxRadius, Color4 color, Matrix4 localTransform, TextureInfo sprite)
		{
			this.MinRadius = minRadius;
			this.MaxRadius = maxRadius;
			this.Color = color;
			this.LocalTransform = localTransform;
			this.TextureId = sprite.Id;
			
			var corner = sprite.Coordinates[0];
			var size = sprite.Coordinates[2] - sprite.Coordinates[0];
			this.TextureTransform = new Matrix3(
				size.X, 0, 0,
				0, size.Y, 0,
				corner.X, corner.Y, 1
			);
		}

		#region IShaderUniformData implementation
		public float Alpha
		{
			get { return this.Color.A; }
			set { this.Color = new Color4(this.Color.R, this.Color.G, this.Color.B, value); }
		}

		public AGlProgram ForProgram
		{
			get { return ShaderLibrary.PlanetOrbit; }
		}

		public int VertexDataSize 
		{ 
			get { return PlanetOrbitGlProgram.VertexSize; }
		}
		
		public IDrawable MakeDrawable(VertexArray vao, int objectIndex)
		{
			return new OrbitDrawable(vao, objectIndex, this);
		}
		#endregion
	}
}
