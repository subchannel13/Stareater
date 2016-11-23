using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Stareater.GLData
{
	class SpriteDrawable
	{
		public VertexArray Vao { get; private set; }
		private readonly int objectIndex;
		private readonly SpriteGlProgram.ObjectData objectData;
		
		public SpriteDrawable(VertexArray vao, int objectIndex, SpriteGlProgram.ObjectData objectData)
		{
			this.Vao = vao;
			this.objectIndex = objectIndex;
			this.objectData = objectData;
		}
		
		public void Draw(Matrix4 view)
		{
			var program = ShaderLibrary.Sprite;
			GL.UseProgram(program.ProgramId);
			this.Vao.Bind();
			GL.ActiveTexture(TextureUnit.Texture0);
			GL.Uniform1(program.TextureSamplerId, 0);
			
			var mvp = this.objectData.LocalTransform * view;
			GL.UniformMatrix4(program.LocalTransformId, false, ref mvp);
			GL.BindTexture(TextureTarget.Texture2D, this.objectData.TextureId);
			GL.Uniform1(program.ZId, this.objectData.Z);
			GL.Uniform4(program.ColorId, this.objectData.Color);
		
			GL.DrawArrays(BeginMode.Triangles, Vao.ObjectStart(this.objectIndex), Vao.ObjectSize(this.objectIndex));
			ShaderLibrary.PrintGlErrors("Draw sprites");
		}
	}
}
