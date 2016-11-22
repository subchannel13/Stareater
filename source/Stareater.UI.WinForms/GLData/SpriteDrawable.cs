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
			Vao.Bind();
			GL.ActiveTexture(TextureUnit.Texture0);
			GL.Uniform1(program.TextureSamplerId, 0);
			
			var mvp = view * objectData.LocalTransform;
			GL.UniformMatrix4(program.LocalTransformId, false, ref mvp);
			GL.BindTexture(TextureTarget.Texture2D, objectData.TextureId);
			GL.Uniform1(program.ZId, objectData.Z);
			GL.Uniform4(program.ColorId, objectData.Color);
		
			GL.DrawArrays(BeginMode.Triangles, Vao.ObjectStart(objectIndex), Vao.ObjectSize(objectIndex));
			ShaderLibrary.PrintGlErrors("Draw sprites");
		}
	}
}
