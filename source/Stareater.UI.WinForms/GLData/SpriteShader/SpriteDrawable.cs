using System;
using System.Linq;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Stareater.GraphicsEngine;

namespace Stareater.GLData.SpriteShader
{
	class SpriteDrawable : IDrawable
	{
		private readonly VertexArray vao;
		private readonly int objectIndex;
		private SpriteData objectData;

		public SpriteDrawable(VertexArray vao, int objectIndex, SpriteData objectData)
		{
			this.vao = vao;
			this.objectIndex = objectIndex;
			this.objectData = objectData;
		}

		public void Draw(Matrix4 view)
		{
			var program = ShaderLibrary.Sprite;
			GL.UseProgram(program.ProgramId);
			this.vao.Bind(); //TODO(v0.6) set program and bind VAO outside
			GL.ActiveTexture(TextureUnit.Texture0);
			GL.Uniform1(program.TextureSamplerId, 0);

			var mvp = this.objectData.LocalTransform * view;
			GL.UniformMatrix4(program.LocalTransformId, false, ref mvp);
			GL.BindTexture(TextureTarget.Texture2D, this.objectData.TextureId);
			GL.Uniform1(program.ZId, this.objectData.Z);
			GL.Uniform4(program.ColorId, this.objectData.Color);

			GL.DrawArrays(BeginMode.Triangles, vao.ObjectStart(this.objectIndex), vao.ObjectSize(this.objectIndex));
			ShaderLibrary.PrintGlErrors("Draw sprites");
		}
		
		public void Update(IShaderData shaderUniforms)
		{
			this.objectData = shaderUniforms as SpriteData;
		}
	}
}
