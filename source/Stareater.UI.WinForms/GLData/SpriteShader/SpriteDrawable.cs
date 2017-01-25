using System;
using System.Linq;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Stareater.GraphicsEngine;

namespace Stareater.GLData.SpriteShader
{
	class SpriteDrawable : IDrawable
	{
		public VertexArray Vao { get; private set; }
		private readonly int objectIndex;
		public SpriteData ObjectData { get; private set; }

		public SpriteDrawable(VertexArray vao, int objectIndex, SpriteData objectData)
		{
			this.Vao = vao;
			this.objectIndex = objectIndex;
			this.ObjectData = objectData;
		}

		public void Draw(Matrix4 view)
		{
			var program = ShaderLibrary.Sprite;
			GL.UseProgram(program.ProgramId);
			this.Vao.Bind();
			GL.ActiveTexture(TextureUnit.Texture0);
			GL.Uniform1(program.TextureSamplerId, 0);

			var mvp = this.ObjectData.LocalTransform * view;
			GL.UniformMatrix4(program.LocalTransformId, false, ref mvp);
			GL.BindTexture(TextureTarget.Texture2D, this.ObjectData.TextureId);
			GL.Uniform1(program.ZId, this.ObjectData.Z);
			GL.Uniform4(program.ColorId, this.ObjectData.Color);

			GL.DrawArrays(BeginMode.Triangles, Vao.ObjectStart(this.objectIndex), Vao.ObjectSize(this.objectIndex));
			ShaderLibrary.PrintGlErrors("Draw sprites");
		}
	}
}
